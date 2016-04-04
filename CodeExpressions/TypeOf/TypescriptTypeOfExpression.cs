using System.CodeDom;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeExpressions.TypeOf
{
    public sealed class TypescriptTypeOfExpression : ITypescriptTypeOfExpression
    {
        private readonly CodeTypeOfExpression _codeExpression;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptTypeOfExpression(
            CodeTypeOfExpression codeExpression, 
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _codeExpression = codeExpression;
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptTypeOfExpression Created");
        }
        
        public string Evaluate()
        {
            var type = _typescriptTypeMapper.GetTypeOutput(_codeExpression.Type);
            return $"instanceof {type}";
        }
    }
}