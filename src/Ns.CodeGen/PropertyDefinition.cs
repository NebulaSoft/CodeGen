using System.Diagnostics.CodeAnalysis;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public readonly struct PropertyDefinition
    {
        public PropertyDefinition(string name, Type returnType, ICodeBlock? getter,ICodeBlock? setter)
        {
            this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
            this.typeName = new ArgumentDefinition(returnType, name);
            this.setter = setter;
            this.readOnly = false;
        }

        public PropertyDefinition(string name, Type returnType, bool readOnly)
        {
            this.typeName = new ArgumentDefinition(returnType, name);
            this.readOnly = readOnly;
            this.getter = null;
            this.setter = null;
        }

        private readonly ArgumentDefinition typeName;
        private readonly bool readOnly;
        private readonly ICodeBlock? getter;
        private readonly ICodeBlock? setter;

        public override string ToString()
            => this.getter == null && this.setter == null ? this.AutoProperty : this.NormalProperty;

        private string NormalProperty => $"public {this.typeName.ToString()} {{ get; {this.Setter}}}"; 

        private string AutoProperty => $"public {this.typeName.ToString()} {{ get; {this.Setter}}}";

        private string Setter => this.readOnly ? string.Empty : "set;";
    }
}
