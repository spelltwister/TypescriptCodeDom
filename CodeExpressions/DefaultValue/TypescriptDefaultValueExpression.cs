using System.CodeDom;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeExpressions.DefaultValue
{
    public sealed class TypescriptDefaultValueExpression : ITypescriptDefaultValueExpression
    {
        private readonly CodeDefaultValueExpression _codeExpression;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptDefaultValueExpression(
            CodeDefaultValueExpression codeExpression,
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _codeExpression = codeExpression;
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptDefaultValueExpression Created");
        }

        public string Evaluate()
        {
            return $"new {_typescriptTypeMapper.GetTypeOutput(_codeExpression.Type)}()";
        }
    }
}