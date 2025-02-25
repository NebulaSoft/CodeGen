using Microsoft.CodeAnalysis;

namespace Ns.CodeGen;

public class TypeDefinition
{
    private readonly string _name;
    
    private TypeDefinition(Type type)
    {
        _name = type.ToCSharpTypeName();
    }

    private TypeDefinition(ITypeSymbol type)
    {
        _name = type.ToCSharpTypeName();
    }

    public override string ToString()
    {
        return _name;
    }

    public static TypeDefinition Create<T>() => new(typeof(T));
    public static TypeDefinition Create(Type type) => new(type);
    public static TypeDefinition Create(ITypeSymbol typeSymbol) => new(typeSymbol);
}