using System.Diagnostics.CodeAnalysis;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public readonly struct  FieldDefinition
    {
        private readonly ArgumentDefinition typeName;

        public FieldDefinition(Type fieldType, string fieldName)
        {
            this.typeName = new ArgumentDefinition(fieldType, fieldName);    
        }

        public override string ToString() => this.typeName.ToString() + ";";
    }
}
