using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Ns.CodeGen
{
    [ExcludeFromCodeCoverage]
    public static class ExpressionExtension
    {
        public static TExpression RenameParameter<TExpression>(this TExpression expression, string target, string newName) where TExpression : Expression
        {
            return new ParameterRename<TExpression>(expression, target, newName).Result;
        }

        class ParameterRename<TExpression> : ExpressionVisitor where TExpression : Expression
        {
            private readonly string target;
            private readonly string newName;
            public TExpression Result;
            
            public ParameterRename(Expression expression, string target, string newName)
            {
                this.target = target;
                this.newName = newName;
                this.Result = (TExpression)Visit(expression);
            }

            protected override Expression VisitParameter(ParameterExpression node) => node.Name == this.target ? Expression.Parameter(node.Type, this.newName) : node;
        }
    }
}
