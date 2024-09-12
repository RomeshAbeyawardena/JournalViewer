namespace JournalViewer.Domain.Tests.Assets;

internal class TestEntityInterceptorFactory : EntityInterceptorFactoryBase<TestContext>
{
    public TestEntityInterceptorFactory(IServiceProvider serviceProvider)
    {
        AddSubjectInterceptor(Subject.OnSave,
            (t) => (IEntityInterceptor)(serviceProvider
                .GetService(typeof(TestEntityInterceptor<>).MakeGenericType(t)) ?? throw new NullReferenceException()));
    }
}
