using System.Diagnostics.CodeAnalysis;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public readonly struct SimpleCodeBlock : ICodeBlock
    {
        public SimpleCodeBlock(string body) => this.body = body;
        
        private readonly string body;

        public override string ToString() => $"\t\t{{\n{this.body}\t\t}}";
    }
}
