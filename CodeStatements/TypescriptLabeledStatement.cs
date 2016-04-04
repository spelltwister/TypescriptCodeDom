using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptLabeledStatement : IStatement
    {
        private readonly IStatementFactory _statementFactory;
        private readonly CodeLabeledStatement _statement;
        private readonly CodeGeneratorOptions _options;

        public TypescriptLabeledStatement(
            IStatementFactory statementFactory,
            CodeLabeledStatement statement,
            CodeGeneratorOptions options)
        {
            _statementFactory = statementFactory;
            _statement = statement;
            _options = options;
        }

        public string Expand()
        {
            var statement = _statementFactory.GetStatement(_statement.Statement, _options).Expand();
            return $"{_statement.Label}:{Environment.NewLine}{statement};";
        }
    }
}