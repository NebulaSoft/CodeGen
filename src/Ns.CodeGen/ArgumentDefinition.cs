namespace Ns.CodeGen
{
    public readonly struct ArgumentDefinition
    {
        private readonly Type type;
        private readonly string name;

        public ArgumentDefinition(Type type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public override string ToString() =>$"{this.type.ToCSharpTypeName()} {this.name}";
    }
}
