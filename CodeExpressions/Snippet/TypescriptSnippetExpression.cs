using System.CodeDom;

namespace TypescriptCodeDom.CodeExpressions.Snippet
{
    public sealed class TypescriptSnippetExpression : ITypescriptSnippetExpression
    {
        private readonly CodeSnippetExpression _codeExpression;

        public TypescriptSnippetExpression(CodeSnippetExpression codeExpression)
        {
            _codeExpression = codeExpression;
            System.Diagnostics.Debug.WriteLine("TypescriptSnippetExpression Created");
        }

        public string Evaluate()
        {
            return _codeExpression.Value;
        }
    }
}