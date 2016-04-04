using System.CodeDom;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptGotoStatement : IStatement
    {
        private readonly CodeGotoStatement _statement;
        
        public TypescriptGotoStatement(CodeGotoStatement statement)
        {
            _statement = statement;
        }

        public string Expand()
        {
            return $"goto {_statement.Label};";
        }
    }
}