using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using static FluentAssertions.FluentActions;

namespace Ns.CodeGen.Tests
{
    public class CSharpCompilerTests
    {
        public class TheCompileMethod
        {
            [Test]
            public void Should_ThrowCompilerNotReadyException_WhenBeginNotCalled()
            {
                // Arrange
                var compiler = new CSharpCompiler();

                // Act
                Invoking(() => compiler.Compile())
                
                // Assert
                .Should().Throw<CompilerNotReadyException>();
            }

            [Test]
            public void Should_ThrowRuntimeBinderInternalCompilerException_WhenInvalidCSharpCompiled()
            {
                // Arrange
                var compiler = new CSharpCompiler();
                compiler.Begin();
                compiler.RegisterFile("I am invalid code!!", "test.cs");

                // Act
                Invoking(() => compiler.Compile())

                // Asset
                .Should().Throw<RuntimeCompilerException>();
            }
            

            [Test]
            public void Should_Call_PostCompletionEvent()
            {
                // Arrange
                var compiler = new CSharpCompiler();
                compiler.Begin();
                compiler.RegisterFile("class Test {}", "test.cs");

                // Act
                bool result = false;
                compiler.PostCompilation(_ => result = true);
                compiler.Compile();

                // Assert
                result.Should().BeTrue();
            }
        }
    }
}
