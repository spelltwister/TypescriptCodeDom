﻿using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeExpressions.ArrayCreate
{
    public sealed class TypescriptArrayCreateExpression : ITypescriptArrayCreateExpression
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeArrayCreateExpression _codeExpression;
        private readonly CodeGeneratorOptions _options;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptArrayCreateExpression(
            IExpressionFactory expressionFactory,
            CodeArrayCreateExpression codeExpression, 
            CodeGeneratorOptions options,
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _expressionFactory = expressionFactory;
            _codeExpression = codeExpression;
            _options = options;
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptArrayCreateExpression Created");
        }

        public string Evaluate()
        {
            string sizeEvaluationString = string.Empty;
            if (_codeExpression.SizeExpression != null)
            {
                var sizeExpression = _expressionFactory.GetExpression(_codeExpression.SizeExpression, _options);
                sizeEvaluationString = sizeExpression.Evaluate();
            }
            else if (_codeExpression.Size > 0)
            {
                sizeEvaluationString = _codeExpression.Size.ToString();
            }

            var typeString = _typescriptTypeMapper.GetTypeOutput(_codeExpression.CreateType);
            var arrayCreateString = $"Array<{typeString}>({sizeEvaluationString})";

            var initializers = _codeExpression.Initializers
                .OfType<CodeExpression>()
                .Select(expression =>
                {
                    var initializerExpression = _expressionFactory.GetExpression(expression, _options);
                    return initializerExpression.Evaluate();
                })
                .ToList();

            if (initializers.Any())
                return $"{arrayCreateString}({string.Join(",", initializers)})";
            return $"{arrayCreateString}";
        }
    }
}