using System.CodeDom;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeExpressions.TypeReference
{
    public sealed class TypescriptTypeReferenceExpression : ITypescriptTypeReferenceExpression
    {
        private readonly CodeTypeReferenceExpression _codeExpression;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptTypeReferenceExpression(
            CodeTypeReferenceExpression codeExpression,
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _codeExpression = codeExpression;
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptTypeReferenceExpression Created");
        }

        public string Evaluate()
        {
            var type = _typescriptTypeMapper.GetTypeOutput(_codeExpression.Type);
            return type;
        }
    }
} 