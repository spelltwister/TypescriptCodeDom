using System.CodeDom;
using System.CodeDom.Compiler;
using TypescriptCodeDom.CodeExpressions;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptMethodReturnStatement : IStatement
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeMethodReturnStatement _statement;
        private readonly CodeGeneratorOptions _options;

        public TypescriptMethodReturnStatement(
            IExpressionFactory expressionFactory,
            CodeMethodReturnStatement statement,
            CodeGeneratorOptions options)
        {
            _expressionFactory = expressionFactory;
            _statement = statement;
            _options = options;
        }

        public string Expand()
        {
            var returnExpression = _expressionFactory.GetExpression(_statement.Expression, _options).Evaluate();
            return $"{_options.IndentString}{_options.IndentString}{_options.IndentString}return {returnExpression};";
        }
    }
}