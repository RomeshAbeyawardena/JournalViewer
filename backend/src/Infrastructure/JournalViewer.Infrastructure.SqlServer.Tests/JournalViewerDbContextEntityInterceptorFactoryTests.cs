using JournalViewer.Domain;
using JournalViewer.Domain.Models;
using JournalViewer.Infrastructure.SqlServer.Interceptors;
using NSubstitute;

namespace JournalViewer.Infrastructure.SqlServer.Tests;

public class JournalViewerDbContextEntityInterceptorFactoryTests
{
    private JournalViewerDbContextEntityInterceptorFactory sut;
    private IServiceProvider serviceProvider;
    private TimeProvider timeProvider;
    
    [SetUp]
    public void Setup()
    {
        serviceProvider = Substitute.For<IServiceProvider>();
        timeProvider = Substitute.For<TimeProvider>();
        
        sut = new(serviceProvider);
    }

    [Test]
    public void EnsureFactoryReturnsValidInterceptors()
    {
        serviceProvider.GetService(typeof(AddCreatedTimestampInterceptor<DbElement>))
            .Returns(new AddCreatedTimestampInterceptor<DbElement>(timeProvider));
        serviceProvider.GetService(typeof(AddEntityToOutboxOnSaveInterceptor<DbElement>))
            .Returns(new AddEntityToOutboxOnSaveInterceptor<DbElement>(timeProvider));
        serviceProvider.GetService(typeof(UpdateModifiedTimestampInterceptor<DbElement>))
            .Returns(new UpdateModifiedTimestampInterceptor<DbElement>(timeProvider));
        var interceptor = sut.GetInterceptors(Subject.OnInsert, typeof(DbElement));

        Assert.Multiple(() =>
        {
            Assert.That(interceptor.Count(), Is.EqualTo(1));
            Assert.That(interceptor.ElementAt(0),
                Is.InstanceOf<AddCreatedTimestampInterceptor<DbElement>>());
        });

        interceptor = sut.GetInterceptors(Subject.OnSave, typeof(DbElement));
        Assert.Multiple(() =>
        {
            Assert.That(interceptor.Count(), Is.EqualTo(1));
            Assert.That(interceptor.ElementAt(0),
                    Is.InstanceOf<AddEntityToOutboxOnSaveInterceptor<DbElement>>());
        });

        interceptor = sut.GetInterceptors(Subject.OnUpdate, typeof(DbElement));
        Assert.Multiple(() =>
        {
            Assert.That(interceptor.Count(), Is.EqualTo(1));
            Assert.That(interceptor.ElementAt(0),
                    Is.InstanceOf<UpdateModifiedTimestampInterceptor<DbElement>>());
        });
    }

    [Test]
    public void EnsureFactoryReturnsNoInterceptorsWhenNotRegistered()
    {
        // Not setting up the serviceProvider to return any interceptors, simulating the case where they are not registered.

        var interceptor = sut.GetInterceptors(Subject.OnInsert, typeof(DbElement));

        Assert.That(interceptor.Count(), Is.EqualTo(0), "No interceptor should be returned when not registered.");

        interceptor = sut.GetInterceptors(Subject.OnSave, typeof(DbElement));

        Assert.That(interceptor.Count(), Is.EqualTo(0), "No interceptor should be returned when not registered.");
    }

}