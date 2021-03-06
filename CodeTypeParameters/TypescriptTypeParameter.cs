﻿using System.CodeDom;
using System.Linq;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeTypeParameters
{
    public sealed class TypescriptTypeParameter : ITypescriptTypeParameter
    {
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;

        public TypescriptTypeParameter(
            ITypescriptTypeMapper typescriptTypeMapper)
        {
            _typescriptTypeMapper = typescriptTypeMapper;
            System.Diagnostics.Debug.WriteLine("TypescriptTypeParameter Created");
        }

        public string Evaluate(CodeTypeParameter codeTypeParameter)
        {
            var typeParameterConstraint = string.Empty;
            if (codeTypeParameter.Constraints.Count > 0)
            {
                var constraint = codeTypeParameter.Constraints.OfType<CodeTypeReference>().First();
                var type = _typescriptTypeMapper.GetTypeOutput(constraint);
                typeParameterConstraint = $" extends {type}";
            }
                
            return $"{codeTypeParameter.Name}{typeParameterConstraint}";
        }
    }
}