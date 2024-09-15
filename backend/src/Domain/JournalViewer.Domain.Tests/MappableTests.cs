using JournalViewer.Domain.Tests.Assets;

namespace JournalViewer.Domain.Tests;

[TestFixture]
public class MappableTests
{
    private TestMappableEntity testMappableEntity;
    [SetUp]
    public void SetUp()
    {
        testMappableEntity = new()
        { 
            DontMap = true,
            A = 1,
            B = 1.1M,
            C = "CookieDough",
            D = 0.42
        };

    }

    [Test]
    public void T()
    {
        var mapped = testMappableEntity.MapTo<AnotherTestMappableEntity>(testMappableEntity);

        Assert.Multiple(() =>
        {
            Assert.That(mapped.A, Is.EqualTo(testMappableEntity.A));
            Assert.That(mapped.B, Is.EqualTo(testMappableEntity.B));
            Assert.That(mapped.C, Is.EqualTo(testMappableEntity.C));
            Assert.That(mapped.D, Is.EqualTo(testMappableEntity.D));
            //mapping should not occur
            Assert.That(mapped.DontMap, Is.Not.EqualTo(testMappableEntity.DontMap));
        });
    }
}
