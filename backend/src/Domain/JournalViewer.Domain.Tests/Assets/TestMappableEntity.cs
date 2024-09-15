using JournalViewer.Domain.Characteristics;

namespace JournalViewer.Domain.Tests.Assets;

public class TestMappableEntity : MappableBase<TestMappableEntity>
{
    public int? A { get; set; }
    public decimal B { get; set; }
    public string C { get; set; } = string.Empty;
    public double? D { get; set; }
    
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool DontMap { get; set; }
}

public class AnotherTestMappableEntity
{
    public int? A { get; set; }
    public decimal B { get; set; }
    public string C { get; set; } = string.Empty;
    public double? D { get; set; }
    public bool DontMap { get; set; }
}
