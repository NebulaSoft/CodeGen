using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public readonly struct PropertyDefinition
    {
        private readonly ArgumentDefinition typeName;
        private readonly bool readOnly;
        private readonly ICodeBlock? getter;
        private readonly ICodeBlock? setter;
        private readonly bool init = false;

        public PropertyDefinition(string name, TypeDefinition returnType, ICodeBlock? getter, ICodeBlock? setter)
        {
            this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
            this.typeName = new ArgumentDefinition(returnType, name);
            this.setter = setter;
            this.readOnly = false;
            this.init = false;
        }

        public PropertyDefinition(string name, TypeDefinition returnType, bool readOnly, bool init)
        {
            this.typeName = new ArgumentDefinition(returnType, name);
            this.readOnly = readOnly;
            this.getter = null;
            this.setter = null;
            this.init = init;
        }

        public override string ToString()
            => this.getter == null && this.setter == null ? this.AutoProperty : this.NormalProperty;

        private string NormalProperty => $"public {this.typeName.ToString()}\n\t\t{{ \n\t\tget\n{this.getter}\n\t\tset\n{this.setter}\n\t\t}}"; 

        private string AutoProperty => $"public {this.typeName.ToString()} {{ get; {this.AutoSetter} }}" + (init ? " = new();" : string.Empty);

        private string AutoSetter => this.readOnly ? string.Empty : "set;";
    }
}
