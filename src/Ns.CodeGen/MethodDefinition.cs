namespace Ns.CodeGen
{
    public readonly struct MethodDefinition
    {
        public MethodDefinition(TypeDefinition? returnType, string name, ICodeBlock body, bool constructor, params ArgumentDefinition[] arguments)
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
        private readonly TypeDefinition? returnType;
        private readonly ArgumentDefinition[] arguments;

        public override string ToString() 
            => $"\t\tpublic {GetReturnType()} {this.name}({GenerateArguments()})\n{this.body}";

        private string GetReturnType() => this.constructor ? String.Empty : returnType is null ? "void" : returnType.ToString();

        string GenerateArguments() => string.Join(", ", this.arguments.Select(a => a.ToString()));
    }
}
