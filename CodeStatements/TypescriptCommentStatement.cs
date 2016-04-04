using System.CodeDom;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class TypescriptCommentStatement : IStatement
    {
        private readonly CodeCommentStatement _statement;

        public TypescriptCommentStatement(CodeCommentStatement statement)
        {
            _statement = statement;
        }

        public string Expand()
        {
            return $"/*{_statement.Comment.Text}*/";
        }
    }
}