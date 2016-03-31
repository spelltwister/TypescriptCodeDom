using System;
using System.CodeDom;
using System.Linq;

namespace TypescriptCodeDom.Common.TypeMapper
{
    public class KnockoutTypescriptTypeMapper : TypescriptTypeMapper
    {
        protected static readonly string KnockoutObservable = "KnockoutObservable";
        protected override string TranslateType(string baseTypeName)
        {
            if ("System.IObservable".Equals(baseTypeName, StringComparison.OrdinalIgnoreCase))
            {
                return KnockoutObservable;
            }
            return base.TranslateType(baseTypeName);
        }

        protected override string UpdateBaseTypeNameWithTypeArgsInner(string currentTypeName, CodeTypeReference typeReference)
        {
            if (KnockoutObservable.Equals(currentTypeName, StringComparison.Ordinal))
            {
                var typeArg = typeReference.TypeArguments.OfType<CodeTypeReference>().Single();
                if (GetTypeOutput(typeArg).StartsWith(TypeScriptTypeNames.Array))
                {
                    currentTypeName += "Array";
                    return AddTypeArguments(typeArg, currentTypeName);
                }
            }
            return base.UpdateBaseTypeNameWithTypeArgsInner(currentTypeName, typeReference);
        }
    }
}