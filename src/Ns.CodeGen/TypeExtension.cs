using System.Diagnostics.CodeAnalysis;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public static class TypeExtension
    {
        public static string ToCSharpTypeName(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName.Replace("+", ".");
            }

            var namePrefix = type.FullName
                .Replace("+", ".")
                .Split(new [] {'`'}, StringSplitOptions.RemoveEmptyEntries)[0];
            
            var genericParameters = type.GetGenericArguments().Select(ToCSharpTypeName).ToCsv();
            return namePrefix + "<" + genericParameters + ">";
        }

        private static string ToCsv(this IEnumerable<object> collectionToConvert, string separator = ", ") 
            => string.Join(separator, collectionToConvert.Select(o => o.ToString()));
    }
}
