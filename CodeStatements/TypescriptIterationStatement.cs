using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using TypescriptCodeDom.CodeExpressions;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptIterationStatement : IStatement
    {
        private readonly IStatementFactory _statementFactory;
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeIterationStatement _statement;
        private readonly CodeGeneratorOptions _options;

        public TypescriptIterationStatement(
            IStatementFactory statementFactory,
            IExpressionFactory expressionFactory,
            CodeIterationStatement statement,
            CodeGeneratorOptions options)
        {
            _statementFactory = statementFactory;
            _expressionFactory = expressionFactory;
            _statement = statement;
            _options = options;
        }

        public string Expand()
        {
            var statements = _statement.Statements.GetStatementsFromCollection(_statementFactory, _options);
            var initStatement = _statementFactory.GetStatement(_statement.InitStatement, _options).Expand().Trim();
            var incrementStatement = _statementFactory.GetStatement(_statement.IncrementStatement, _options).Expand().Trim();
            var testExpression = _expressionFactory.GetExpression(_statement.TestExpression, _options).Evaluate().Trim();

            if (initStatement.EndsWith(";"))
            {
                initStatement = initStatement.Substring(0, initStatement.Length - 1);
            }
            if (incrementStatement.EndsWith(";"))
            {
                incrementStatement = incrementStatement.Substring(0, incrementStatement.Length - 1);
            }

            string indentLevel = $"{_options.IndentString}{_options.IndentString}{_options.IndentString}";
            return $"{indentLevel}for ({initStatement}; {testExpression}; {incrementStatement}) {{{Environment.NewLine}{_options.IndentString}{statements}{Environment.NewLine}{indentLevel}}}";
        }
    }
}