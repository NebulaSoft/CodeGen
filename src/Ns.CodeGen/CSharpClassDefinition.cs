using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassDefinition
    {
        private readonly List<string> usingStatements;
        private readonly List<FieldDefinition> fields;
        private readonly List<PropertyDefinition> properties;
        private readonly List<MethodDefinition> methods;
        private readonly List<TypeDefinition> inherits;
        private string Name { get; }
        private string? Namespace { get; }

        public CSharpClassDefinition(string className, string classNamespace)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("A class name is required", nameof(className));
            }
            
            this.fields = new List<FieldDefinition>();
            this.usingStatements = new List<string>();
            this.methods = new List<MethodDefinition>();
            this.properties = new List<PropertyDefinition>();
            this.Name = className;
            this.Namespace = classNamespace;
            this.inherits = new List<TypeDefinition>();
        }

        public CSharpClassDefinition AddUsing(string usingStatement)
        {
            this.usingStatements.Add(usingStatement);
            return this;
        }
        
        public CSharpClassDefinition AddConstructor(ICodeBlock block, params ArgumentDefinition[] arguments)
        {
            this.methods.Add(new MethodDefinition(null, this.Name, block, true, arguments));
            return this;
        }
        
        public CSharpClassDefinition AddField(TypeDefinition fieldType, string fieldName)
        {
            this.fields.Add(new FieldDefinition(fieldType, fieldName));
            return this;
        }
        
        public CSharpClassDefinition AddAutoProperty(TypeDefinition returnType, string propertyName, bool readOnly)
        {
            this.properties.Add(new PropertyDefinition(propertyName, returnType,  readOnly));
            return this;
        }

        public CSharpClassDefinition AddProperty(TypeDefinition returnType, string propertyName, ICodeBlock getter)
        {
            this.properties.Add(new PropertyDefinition(propertyName, returnType, getter, null));
            return this;
        }

        public CSharpClassDefinition AddProperty(TypeDefinition returnType, string propertyName, ICodeBlock getter, ICodeBlock setter)
        {
            this.properties.Add(new PropertyDefinition(propertyName, returnType, getter, setter));
            return this;
        }

        public CSharpClassDefinition AddMethod(TypeDefinition returnType, string methodName, ICodeBlock body, params ArgumentDefinition[] args)
        {
            this.methods.Add(new MethodDefinition(returnType, methodName, body, false, args));
            return this;
        }

        public CSharpClassDefinition AddMethod<TReturnType>(string methodName, ICodeBlock body, params ArgumentDefinition[] args)
        {
            this.methods.Add(new MethodDefinition(TypeDefinition.Create<TReturnType>(), methodName, body, false, args));
            return this;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            AppendUsingStatements(result);
            AppendClassHeader(result);
            AppendFields(result);
            AppendProperties(result);
            AppendMethods(result);
            AppendClassFooter(result);
            
            return result.ToString();
        }

        private void AppendClassFooter(StringBuilder result)
        {
            result.AppendLine("\t}");

            if (this.Namespace != null)
            {
                result.AppendLine("}");
            }
        }

        private void AppendClassHeader(StringBuilder result)
        {
            if (this.Namespace != null)
            {
                result.AppendLine($"namespace {this.Namespace}");
                result.AppendLine("{");
            }
            result.Append($"\tpublic class {this.Name}");
            result.AppendLine(this.inherits.Any() ? $" : {string.Join(", ", this.inherits.Select(i => i.ToString()))}" : string.Empty);
            result.AppendLine("\t{");
        }

        private void AppendMethods(StringBuilder result)
        {
            foreach (var method in this.methods)
            {
                result.AppendLine(method.ToString());
                result.AppendLine();
            }
        }

        private void AppendProperties(StringBuilder result)
        {
            foreach (var property in this.properties)
            {
                result.AppendLine("\t\t" + property);
                result.AppendLine();
            }
        }

        private void AppendFields(StringBuilder result)
        {
            foreach (var field in this.fields)
            {
                result.AppendLine("\t\t" + field);
            }

            result.AppendLine();
        }

        private void AppendUsingStatements(StringBuilder result)
        {
            foreach (var usingStatement in this.usingStatements)
            {
                result.AppendLine($"using {usingStatement};");
            }
            result.AppendLine();
        }

        public CSharpClassDefinition Inherits(Type sourceType) => Inherits(TypeDefinition.Create(sourceType));

        public CSharpClassDefinition Inherits<T>() => Inherits(TypeDefinition.Create<T>());

        public CSharpClassDefinition Inherits(ITypeSymbol typeSyntax) => Inherits(TypeDefinition.Create(typeSyntax));

        public CSharpClassDefinition Inherits(TypeDefinition typeDefinition)
        {
            if (!inherits.Contains(typeDefinition))
            {
                inherits.Add(typeDefinition);
            }
            return this;
        }

        public void SendToCompiler(CSharpCompiler compiler, Action<Assembly, Type?>? postCompilation = null )
        {
            compiler.RegisterFile(this.ToString(), this.Name + ".cs");
            if (postCompilation != null)
            {
                compiler.PostCompilation((assembly) =>
                {
                    var type = assembly.GetType( this.Namespace + "." + this.Name);
                    postCompilation(assembly, type);
                });
            }
        }
    }
}