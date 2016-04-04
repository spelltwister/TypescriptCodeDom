using System.CodeDom;

namespace TypescriptCodeDom.CodeTypeParameters
{
    // TODO: why is this internal?
    internal interface ITypescriptTypeParameter
    {
        string Evaluate(CodeTypeParameter codeTypeParameter);
    }
}