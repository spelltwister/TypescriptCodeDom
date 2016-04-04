using System.CodeDom;

namespace TypescriptCodeDom.CodeExpressions.VariableReference
{
    public sealed class TypescriptVariableReferenceExpression : ITypescriptVariableReferenceExpression
    {
        private readonly CodeVariableReferenceExpression _codeExpression;

        public TypescriptVariableReferenceExpression(CodeVariableReferenceExpression codeExpression)
        {
            _codeExpression = codeExpression;
            System.Diagnostics.Debug.WriteLine("TypescriptVariableReferenceExpression Created");
        }

        public string Evaluate()
        {
            return _codeExpression.VariableName;
        }
    }
}