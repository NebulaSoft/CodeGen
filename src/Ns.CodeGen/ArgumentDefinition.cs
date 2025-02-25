namespace Ns.CodeGen
{
    public readonly struct ArgumentDefinition
    {
        private readonly TypeDefinition type;
        private readonly string name;

        public ArgumentDefinition(TypeDefinition type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public override string ToString() =>$"{type} {this.name}";
    }
}
