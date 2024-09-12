using JournalViewer.Domain.Tests.Assets;
using NSubstitute;

namespace JournalViewer.Domain.Tests;

public class TestEntityInterceptorFactoryTests
{
    private TestEntityInterceptorFactory sut;
    private IServiceProvider serviceProvider;
    [SetUp]
    public void Setup()
    {
        serviceProvider = Substitute.For<IServiceProvider>();
        sut = new(serviceProvider);
    }

    [Test]
    public void GetInterceptors()
    {
        serviceProvider.GetService(typeof(TestEntityInterceptor<TestEntity>))
            .Returns(new TestEntityInterceptor<TestEntity>());

        var interceptor = sut.GetInterceptors(Subject.OnSave, typeof(TestEntity));

        Assert.Multiple(() =>
        {
            Assert.That(interceptor.Count(), Is.EqualTo(1));
            Assert.That(interceptor.FirstOrDefault(),
                Is.InstanceOf<TestEntityInterceptor<TestEntity>>());
        });
    }
}