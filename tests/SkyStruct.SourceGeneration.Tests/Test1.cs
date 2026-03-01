using Inheritance;

namespace SkyStruct.SourceGeneration.Tests;

[TestClass]
public sealed class SourceGenerationTests 
{
    [TestMethod]
    public void TestTestFileNamespace()
    {
        var employee = new Employee();
        
        employee.Boss = new Person();
    }
}