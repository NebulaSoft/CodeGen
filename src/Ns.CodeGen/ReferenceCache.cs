using Microsoft.CodeAnalysis;

namespace Ns.CodeGen;

internal static class ReferenceCache
{
    static ReferenceCache()
    {
        Refresh();
    }

    public static IReadOnlyList<MetadataReference>? References { get; private set; }

    private static void Refresh()
    {
        var references = new List<MetadataReference>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (!assembly.IsDynamic)
            {
                references.Add(MetadataReference.CreateFromFile(assembly.Location));
            }
        }

        References = references.AsReadOnly();
    }
}