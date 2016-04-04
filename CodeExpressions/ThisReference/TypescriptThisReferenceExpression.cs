namespace TypescriptCodeDom.CodeExpressions.ThisReference
{
    public sealed class TypescriptThisReferenceExpression : ITypescriptThisReferenceExpression
    {
        public TypescriptThisReferenceExpression()
        {
            System.Diagnostics.Debug.WriteLine("TypescriptThisReferenceExpression Created");
        }

        public string Evaluate()
        {
            return "this";
        }
    }
}