using System.CodeDom;

namespace TypescriptCodeDom.CodeTypeMembers
{
    public static class Extensions
    {
        public static string GetAccessModifier(this CodeTypeMember member)
        {
            string modifierString = ((member.Attributes & MemberAttributes.Static) == MemberAttributes.Static) ? "static " : string.Empty;

            if((member.Attributes & MemberAttributes.Public) == MemberAttributes.Public)
            {
                // default is public so don't really need to do anything, but could do as below
                //return modifierString + "public ";
            }

            if ((member.Attributes & MemberAttributes.Private) == MemberAttributes.Private)
            {
                return modifierString + "private ";
            }

            return modifierString;
        }
    }
}