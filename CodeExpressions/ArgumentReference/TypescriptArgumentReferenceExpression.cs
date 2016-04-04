using System.CodeDom;

namespace TypescriptCodeDom.CodeExpressions.ArgumentReference
{
    public sealed class TypescriptArgumentReferenceExpression : ITypescriptArgumentReferenceExpression
    {
        private readonly CodeArgumentReferenceExpression _codeExpression;

        public TypescriptArgumentReferenceExpression(CodeArgumentReferenceExpression codeExpression)
        {
            _codeExpression = codeExpression;
            System.Diagnostics.Debug.WriteLine("TypescriptArgumentReferenceExpression Created");
        }

        public string Evaluate()
        {
            return _codeExpression.ParameterName;
        }
    }
}
