﻿using System.CodeDom;
using System.CodeDom.Compiler;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeExpressions.ObjectCreate
{
    public sealed class TypescriptObjectCreateExpression : ITypescriptObjectCreateExpression
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeObjectCreateExpression _codeExpression;
        private readonly CodeGeneratorOptions _options;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptObjectCreateExpression(
            IExpressionFactory expressionFactory, 
            CodeObjectCreateExpression codeExpression, 
            CodeGeneratorOptions options, 
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _expressionFactory = expressionFactory;
            _codeExpression = codeExpression;
            _options = options;
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptObjectCreateExpression Created");
        }

        public string Evaluate()
        {
            var type = _typescriptTypeMapper.GetTypeOutput(_codeExpression.CreateType);
            var parameters = _codeExpression.Parameters.GetParametersFromExpressions(_expressionFactory, _options);

            return $"new {type}({parameters})";
        }
    }
}