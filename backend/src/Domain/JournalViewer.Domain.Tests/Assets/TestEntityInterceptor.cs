
using JournalViewer.Domain.Bootstrap;

namespace JournalViewer.Domain.Tests.Assets;

internal class TestEntityInterceptor<TEntity>() 
    : EntityInterceptorBase<TestContext, TEntity>(Subject.OnSave)
{
    public override Task Intercept(Subject subject, TestContext context, TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
