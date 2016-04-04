using System.CodeDom;

namespace TypescriptCodeDom.CodeTypeMembers
{
    public sealed class TypescriptSnippetTypeMember : IMember
    {
        private readonly CodeSnippetTypeMember _member;

        public TypescriptSnippetTypeMember(CodeSnippetTypeMember member)
        {
            _member = member;
        }

        public string Expand()
        {
            return _member.Text;
        }
    }
}