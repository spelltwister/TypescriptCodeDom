﻿using System.CodeDom;
using System.CodeDom.Compiler;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeExpressions.Cast
{
    public sealed class TypescriptCastExpression : ITypescriptCastExpression
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeCastExpression _codeExpression;
        private readonly CodeGeneratorOptions _options;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptCastExpression(
            IExpressionFactory expressionFactory,
            CodeCastExpression codeExpression, 
            CodeGeneratorOptions options,
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _expressionFactory = expressionFactory;
            _codeExpression = codeExpression;
            _options = options;
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptCastExpression Created");
        }

        public string Evaluate()
        {
            var typeOutput = _typescriptTypeMapper.GetTypeOutput(_codeExpression.TargetType);
            var expression = _expressionFactory.GetExpression(_codeExpression.Expression, _options);
            var expressionToCast = expression.Evaluate();
            return $"<{typeOutput}>({expressionToCast})";
        }
    }
}