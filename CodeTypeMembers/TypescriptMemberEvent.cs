using System.CodeDom;
using TypescriptCodeDom.Common.TypeMapper;
using System.CodeDom.Compiler;

namespace TypescriptCodeDom.CodeTypeMembers
{
    public sealed class TypescriptMemberEvent : IMember
    {
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;
        private readonly CodeMemberEvent _member;
        private readonly CodeGeneratorOptions _options;

        public TypescriptMemberEvent(
            ITypescriptTypeMapper typescriptTypeMapper,
            CodeMemberEvent member,
            CodeGeneratorOptions options)
        {
            _typescriptTypeMapper = typescriptTypeMapper;
            _member = member;
            _options = options;
        }

        public string Expand()
        {
            string eventDeclaration = $"{_member.Name}: Array<{_typescriptTypeMapper.GetTypeOutput(_member.Type)}>;";
            string accessModifier = _member.GetAccessModifier();
            return $"{_options.IndentString}{accessModifier}{eventDeclaration}";
        }
    }
}