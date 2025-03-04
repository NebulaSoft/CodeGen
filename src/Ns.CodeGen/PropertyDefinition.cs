using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public readonly struct PropertyDefinition
    {
        public PropertyDefinition(string name, TypeDefinition returnType, ICodeBlock? getter,ICodeBlock? setter)
        {
            this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
            this.typeName = new ArgumentDefinition(returnType, name);
            this.setter = setter;
            this.readOnly = false;
        }

        public PropertyDefinition(string name, TypeDefinition returnType, bool readOnly)
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

        private string NormalProperty => $"public {this.typeName.ToString()}\n\t\t{{ \n\t\tget\n{this.getter}\n\t\tset\n{this.setter}\n\t\t}}"; 

        private string AutoProperty => $"public {this.typeName.ToString()} {{ get; {this.AutoSetter} }}";

        private string AutoSetter => this.readOnly ? string.Empty : "set;";
    }
}
