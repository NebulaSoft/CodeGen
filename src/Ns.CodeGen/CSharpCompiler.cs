using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Ns.CodeGen
{
    public sealed class CSharpCompiler
    {
        private List<string> codeFragments = null!;
        private List<Action<Assembly>> postCompilation = null!;

        public void Begin()
        {
            this.codeFragments = new List<string>();
            this.postCompilation = new List<Action<Assembly>>();
        }
        
        public void RegisterFile(string code, string file)
        {
            this.codeFragments?.Add(code);
        }

        public Assembly Compile(string? assemblyName = null)
        {
            if (this.codeFragments == null)
            {
                throw new CompilerNotReadyException();
            }
            
            if (string.IsNullOrEmpty(assemblyName))
            {
                assemblyName = Guid.NewGuid() + ".dll";
            } 
            
            var syntaxTrees = this.codeFragments.Select(codeFragment => CSharpSyntaxTree.ParseText(codeFragment));

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees,
                ReferenceCache.References,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
                
            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);
                var errors = failures.Aggregate(string.Empty,
                    (current, diagnostic) => current + $"{diagnostic.Id}: {diagnostic.GetMessage()}\n");
                var exception = new RuntimeCompilerException(errors);
                exception.Data.Add("Code Fragments", this.codeFragments);
                throw exception;
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            foreach (var action in this.postCompilation)
            {
                action(assembly);
            }

            return assembly;
        }
        public void PostCompilation(Action<Assembly> action)
        {
            this.postCompilation?.Add(action);
        }
    }
}
