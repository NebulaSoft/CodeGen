namespace Ns.CodeGen
{
    public readonly struct MethodDefinition
    {
        public MethodDefinition(Type? returnType, string name, ICodeBlock body, bool constructor, params ArgumentDefinition[] arguments)
        {
            this.returnType = returnType;
            this.arguments = arguments;
            this.body = body;
            this.constructor = constructor;
            this.name = name;
        }

        private readonly string name;
        private readonly ICodeBlock body;
        private readonly bool constructor;
        private readonly Type? returnType;
        private readonly ArgumentDefinition[] arguments;

        public override string ToString() 
            => $"\t\tpublic {GetReturnType()} {this.name}({GenerateArguments()})\n{this.body}";

        private string GetReturnType() => this.constructor ? String.Empty : this.returnType?.ToCSharpTypeName() ?? "void";

        string GenerateArguments() => string.Join(", ", this.arguments.Select(a => a.ToString()));
    }
}
