namespace TypescriptCodeDom.CodeExpressions.PropertySetValue
{
    public sealed class TypescriptPropertySetValueReferenceExpression : ITypescriptPropertySetValueReferenceExpression
    {
        public TypescriptPropertySetValueReferenceExpression()
        {
            System.Diagnostics.Debug.WriteLine("TypescriptPropertySetValueReferenceExpression Created");
        }

        public string Evaluate()
        {
            return "value";
        }
    }
}