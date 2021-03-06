﻿using System.CodeDom;
using System.CodeDom.Compiler;

namespace TypescriptCodeDom.CodeExpressions.Primitive
{
    public sealed class TypescriptPrimitiveExpression : ITypescriptPrimitiveExpression
    {
        private readonly CodePrimitiveExpression _codeExpression;
        private readonly CodeGeneratorOptions _options;

        public TypescriptPrimitiveExpression(
            CodePrimitiveExpression codeExpression, 
            CodeGeneratorOptions options)
        {
            _codeExpression = codeExpression;
            _options = options;
            System.Diagnostics.Debug.WriteLine("TypescriptPrimitiveExpression Created");
        }

        public string Evaluate()
        {
            var expressionValue = _codeExpression.Value.ToString();
            return _codeExpression.Value is string ? $"\"{_codeExpression.Value}\"" : expressionValue;
        }
    }
}