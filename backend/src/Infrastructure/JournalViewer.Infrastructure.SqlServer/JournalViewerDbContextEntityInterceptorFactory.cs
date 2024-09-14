using JournalViewer.Domain.Bootstrap;
using JournalViewer.Infrastructure.SqlServer.Interceptors;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JournalViewer.Infrastructure.SqlServer;

internal class JournalViewerDbContextEntityInterceptorFactory : EntityInterceptorFactoryBase<JournalViewDbContext>
{
    public override Type ChangeType(Type type)
    {
        return base.ChangeType(typeof(EntityEntry<>).MakeGenericType(type));
    }
    public JournalViewerDbContextEntityInterceptorFactory(IServiceProvider serviceProvider)
    {
        Add(Subject.OnInsert, t => GetFromServiceProviderFactory(
            typeof(AddCreatedTimestampInterceptor<>).MakeGenericType(t),
            serviceProvider.GetService));
        Add(Subject.OnUpdate, t => GetFromServiceProviderFactory(
            typeof(UpdateModifiedTimestampInterceptor<>).MakeGenericType(t),
            serviceProvider.GetService));
        Add(Subject.OnSave, t => GetFromServiceProviderFactory(
            typeof(AddEntityToOutboxOnSaveInterceptor<>).MakeGenericType(t),
            serviceProvider.GetService));
    }
}
