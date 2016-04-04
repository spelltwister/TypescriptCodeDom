using System.CodeDom;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeExpressions.ParameterDeclaration
{
    public sealed class TypescriptParameterDeclarationExpression : ITypescriptParameterDeclarationExpression
    {
        private readonly CodeParameterDeclarationExpression _codeExpression;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptParameterDeclarationExpression(
            CodeParameterDeclarationExpression codeExpression,
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _codeExpression = codeExpression;
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptParameterDeclarationExpression Created");
        }

        public string Evaluate()
        {
            var type = _typescriptTypeMapper.GetTypeOutput(_codeExpression.Type);
            return $"{_codeExpression.Name}: {type}";
        }
    }
}  