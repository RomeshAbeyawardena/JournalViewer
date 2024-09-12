using JournalViewer.Domain;
using JournalViewer.Infrastructure.SqlServer.Interceptors;

namespace JournalViewer.Infrastructure.SqlServer;

internal class JournalViewerDbContextEntityInterceptorFactory : EntityInterceptorFactoryBase<JournalViewDbContext>
{
    public JournalViewerDbContextEntityInterceptorFactory(IServiceProvider serviceProvider)
    {
        Add(Subject.OnInsert, t => GetFromServiceProviderFactory(
            typeof(AddCreatedTimestampInterceptor<>).MakeGenericType(t),
            serviceProvider.GetService));
        Add(Subject.OnSave, t => GetFromServiceProviderFactory(
            typeof(AddEntityToOutboxOnSaveInterceptor<>).MakeGenericType(t),
            serviceProvider.GetService));
    }
}
