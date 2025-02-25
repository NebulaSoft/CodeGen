using System.ComponentModel;
using FluentAssertions;

namespace Ns.CodeGen.Tests;

public class MethodDefinitionTests
{
    [Test]
    public void ToString_ShouldReturn_ExpectedMethod()
    {
        // Arrange
        var body = new CodeBlockBuilder()
                .AddLine("// Just a comment")
                .AddExpression("var a = ", () => 1)
            ;

        var sut = new MethodDefinition(TypeDefinition.Create<int>(), "TestMethod", body, false, new ArgumentDefinition(TypeDefinition.Create<string>(), "example"));
        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("\t\tpublic System.Int32 TestMethod(System.String example)\n\t\t{\n\t\t\t// Just a comment;\n\t\t\tvar a = 1;\n\t\t}");
    }
    
    [Test]
    public void ToString_ShouldReturn_ExpectedCtor()
    {
        // Arrange
        var body = new CodeBlockBuilder()
                .AddLine("// Just a comment")
                .AddExpression("var a = ", () => 1)
            ;

        var sut = new MethodDefinition(null, "TestMethod", body, true, new ArgumentDefinition(TypeDefinition.Create<string>(), "example"));
        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("\t\tpublic  TestMethod(System.String example)\n\t\t{\n\t\t\t// Just a comment;\n\t\t\tvar a = 1;\n\t\t}");
    }
}