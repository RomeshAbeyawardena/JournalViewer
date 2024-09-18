using JournalViewer.Domain;
using JournalViewer.Domain.Characteristics;

namespace JournalViewer.Infrastructure.Tests;

internal class Domain : MappableBase<Domain>
{
    public int? A { get; set; }
    public decimal B { get; set; }
    public string C { get; set; } = string.Empty;
    public double? D { get; set; }

}

internal class MappableDomain 
{
    public int? A { get; set; }
    public decimal B { get; set; }
    public string C { get; set; } = string.Empty;
    public double? D { get; set; }
}


public class AsyncPagedListTests
{
    AsyncPagedList<Domain> sut;
    List<Domain> internalQueryable;
    IPagedRequest pagedRequest;
    [SetUp]
    public void Setup()
    {
        internalQueryable = [new Domain { A = 1, B = 1.2m, C = "Test", D = 0.2 }];
        pagedRequest = NSubstitute.Substitute.For<IPagedRequest>();

        sut = new(new PagedResponse<Domain>(internalQueryable.AsQueryable(), pagedRequest));

    }

    [Test]
    public void Test1()
    {
        var s = sut.ProjectTo(s => s.MapTo<MappableDomain>(s), CancellationToken.None);
        Assert.Pass();
    }
}