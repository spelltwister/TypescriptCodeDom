using System.CodeDom;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptSnippetStatement : IStatement
    {
        private readonly CodeSnippetStatement _statement;

        public TypescriptSnippetStatement(CodeSnippetStatement statement)
        {
            _statement = statement;
        }

        public string Expand()
        {
            return _statement.Value;
        }
    }
}