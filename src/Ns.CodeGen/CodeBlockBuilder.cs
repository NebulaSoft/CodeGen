using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using ExpressionToCodeLib;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ns.CodeGen
{
    public sealed class CodeBlockBuilder : ICodeBlock
    {
        private const string Start = "\t\t{\n";
        private const string End = "\t\t}";
        private const string LineStart = "\t\t\t";
        private const string LineEnd = ";\n";
            
        private readonly List<string> lines = new List<string>();

        public CodeBlockBuilder AddExpression(LambdaExpression exp)
        {
            AddExpression(String.Empty, exp);
            return this;
        }

        public CodeBlockBuilder AddExpression(string preFix, LambdaExpression exp)
        {
            AddLine($"{preFix}{ExpressionToCode.ToCode(exp.Body)}");
            return this;
        }
        
        public CodeBlockBuilder AddLine(string line, bool disableLineEnd = false)
        {
            this.lines.Add(disableLineEnd ? $"{LineStart}{line}\n" : $"{LineStart}{line}{LineEnd}");
            return this;
        }
        public override string ToString() => Start + string.Join("", this.lines) + End;
    }
}
