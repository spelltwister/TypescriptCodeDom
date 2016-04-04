namespace TypescriptCodeDom.CodeExpressions.BaseReference
{
    public class TypescriptBaseReferenceExpression : ITypescriptBaseReferenceExpression
    {
        public TypescriptBaseReferenceExpression()
        {
            System.Diagnostics.Debug.WriteLine("TypescriptBaseReferenceExpression Created");
        }

        public string Evaluate()
        {
            return "this";
        }
    }
}