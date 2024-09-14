using JournalViewer.Domain.Bootstrap;

namespace JournalViewer.Domain.Tests.Assets;

internal class TestEntityInterceptorFactory : EntityInterceptorFactoryBase<TestContext>
{
    public TestEntityInterceptorFactory(IServiceProvider serviceProvider)
    {
        base.Add(Subject.OnSave,
            (t) => GetFromServiceProviderFactory(typeof(TestEntityInterceptor<>)
                .MakeGenericType(t),
            t => serviceProvider.GetService(t)));
    }
}
