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
        serviceProvider.GetService(typeof(AddCreatedTimestampInterceptor<Element>))
            .Returns(new AddCreatedTimestampInterceptor<Element>(timeProvider));
        serviceProvider.GetService(typeof(AddEntityToOutboxOnSaveInterceptor<Element>))
            .Returns(new AddEntityToOutboxOnSaveInterceptor<Element>(timeProvider));

        var interceptor = sut.GetInterceptors(Subject.OnInsert, typeof(Element));

        Assert.Multiple(() =>
        {
            Assert.That(interceptor.Count(), Is.EqualTo(1));
            Assert.That(interceptor.ElementAt(0),
                Is.InstanceOf<AddCreatedTimestampInterceptor<Element>>());
        });

        interceptor = sut.GetInterceptors(Subject.OnSave, typeof(Element));
        Assert.Multiple(() =>
        {
            Assert.That(interceptor.Count(), Is.EqualTo(1));
            Assert.That(interceptor.ElementAt(0),
                    Is.InstanceOf<AddEntityToOutboxOnSaveInterceptor<Element>>());
        });
    }
}