using System.CodeDom;
using System.CodeDom.Compiler;
using TypescriptCodeDom.CodeExpressions;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptVariableDeclarationStatement : IStatement
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeVariableDeclarationStatement _statement;
        private readonly CodeGeneratorOptions _options;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptVariableDeclarationStatement(
            IExpressionFactory expressionFactory,
            CodeVariableDeclarationStatement statement,
            CodeGeneratorOptions options,
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _expressionFactory = expressionFactory;
            _statement = statement;
            _options = options;
            _typescriptTypeMapper = typescriptTypeMapper;
        }

        public string Expand()
        {
            var type = _typescriptTypeMapper.GetTypeOutput(_statement.Type);
            var initializationExpression = _expressionFactory.GetExpression(_statement.InitExpression, _options).Evaluate();

            // TODO: move indent to caller; there are many places where we would not want to indent
            // eg, for(/*No Indent Here*/var i = 0; i < max; i++)
            return $"{_options.IndentString}{_options.IndentString}{_options.IndentString}var {_statement.Name}: {type} = {initializationExpression};";
        }
    }
}