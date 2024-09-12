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
    public void Test1()
    {
        serviceProvider.GetService(typeof(TestEntityInterceptor<TestEntity>));
        var interceptor = sut.GetInterceptors(Subject.OnSave, typeof(TestEntity));
        Assert.That(interceptor.FirstOrDefault(), 
            Is.InstanceOf<TestEntityInterceptor<TestEntity>>());
    }
}