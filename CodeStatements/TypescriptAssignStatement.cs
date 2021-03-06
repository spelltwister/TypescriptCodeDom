﻿using System.CodeDom;
using System.CodeDom.Compiler;
using TypescriptCodeDom.CodeExpressions;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptAssignStatement : IStatement
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeAssignStatement _codeAssignStatement;
        private readonly CodeGeneratorOptions _options;

        public TypescriptAssignStatement(
            IExpressionFactory expressionFactory,
            CodeAssignStatement codeAssignStatement,
            CodeGeneratorOptions options)
        {
            _expressionFactory = expressionFactory;
            _codeAssignStatement = codeAssignStatement;
            _options = options;
        }

        public string Expand()
        {
            var leftExpression = _expressionFactory.GetExpression(_codeAssignStatement.Left, _options);
            var rightExpression = _expressionFactory.GetExpression(_codeAssignStatement.Right, _options);

            return $"{_options.IndentString}{_options.IndentString}{_options.IndentString}{leftExpression.Evaluate()} = {rightExpression.Evaluate()};";
        }
    }
}