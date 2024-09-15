using JournalViewer.Domain.Features.Categories;
using JournalViewer.Domain.Features.Categories.Queries;
using MediatR;

namespace JournalViewer.Web.Features.Categories;

public class Handler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategories, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(GetCategories request, CancellationToken cancellationToken)
    {
        return await categoryRepository.GetCategories(request, new CategoryFilter
        {
            NameContains = request.NameContains,
            ShowAll = request.ShowAll
        });
    }
}
