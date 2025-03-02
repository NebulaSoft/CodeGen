using Microsoft.CodeAnalysis;

namespace Ns.CodeGen;

public class TypeDefinition : IEquatable<TypeDefinition>, IComparable<TypeDefinition>, IEqualityComparer<TypeDefinition>
{
    private readonly string name;
    
    private TypeDefinition(Type type)
    {
        name = type.ToCSharpTypeName();
    }

    private TypeDefinition(ITypeSymbol type)
    {
        name = type.ToCSharpTypeName();
    }

    public override string ToString()
    {
        return name;
    }

    public static TypeDefinition Create<T>() => new(typeof(T));
    public static TypeDefinition Create(Type type) => new(type);
    public static TypeDefinition Create(ITypeSymbol typeSymbol) => new(typeSymbol);

    public bool Equals(TypeDefinition? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return name == other.name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TypeDefinition)obj);
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

    public int CompareTo(TypeDefinition? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return string.Compare(name, other.name, StringComparison.Ordinal);
    }

    public bool Equals(TypeDefinition? x, TypeDefinition? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.name == y.name;
    }

    public int GetHashCode(TypeDefinition obj)
    {
        return obj.name.GetHashCode();
    }
}