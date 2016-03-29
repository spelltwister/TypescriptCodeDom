using System.CodeDom;
using System.CodeDom.Compiler;

namespace TypescriptCodeDom.CodeExpressions
{
    public interface IExpressionFactory
    {
        IExpression GetExpression(CodeExpression expression, CodeGeneratorOptions options);
    }
}