using FluentAssertions;

namespace Ns.CodeGen.Tests;

public class ArgumentDefinitionTests
{

    [Test]
    public void ToString_ShouldReturn_ExpectedResult()
    {
        // Arrange
        var sut = new ArgumentDefinition(typeof(string), "Test");
        
        // Act
        var result = sut.ToString();
        
        // Assert
        result.Should().Be("System.String Test");
    }
}