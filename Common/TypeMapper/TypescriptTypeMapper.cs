using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TypescriptCodeDom.Common.TypeMapper
{
    public class TypescriptTypeMapper : ITypescriptTypeMapper
    {
        private readonly Dictionary<string, string> _typeMap;
        private static readonly Regex _baseTypeRegex = new Regex(@"(?<TypeName>([a-zA-Z]+[0-9.]*)+)");
        private static readonly Regex _arrayRegex = new Regex(@"(?<TypeName>[a-zA-Z0-9\.]+)(?<TypeArguments>[`0-9]*)(\[(?<JaggedRank>\]\[)*|(?<DimensionalRank>,)*\])+$");

        public TypescriptTypeMapper()
        {
            _typeMap = new Dictionary<string, string>();            

            AddAllKnownTypes();
            System.Diagnostics.Debug.WriteLine("TypescriptTypeMapper Created");
        }

        private static class TypeScriptTypeNames
        {
            public static readonly string Number  = "number";
            public static readonly string String  = "string";
            public static readonly string Boolean = "boolean";
            public static readonly string Void    = "void";
            public static readonly string Any     = "any";
            public static readonly string Date    = "Date";
            public static readonly string Array   = "Array";
        }

        private static readonly Type[] NumberTypes = new Type[]
        {
            typeof(int    ),
            typeof(uint   ),
            typeof(long   ),
            typeof(ulong  ),
            typeof(short  ),
            typeof(ushort ),
            typeof(float  ),
            typeof(double ),
            typeof(decimal),
            typeof(byte   )
        };

        private static readonly Type[] StringTypes = new Type[]
        {
            typeof(string),
            typeof(Guid  ),
            typeof(char  )
        };

        public static bool IsNumberType(Type type)
        {
            return NumberTypes.Any(x => type == x);
        }

        private void AddAllKnownTypes()
        {
            foreach (Type numberType in NumberTypes)
            {
                this._typeMap[numberType.FullName] = TypeScriptTypeNames.Number;
            }

            foreach (Type stringType in StringTypes)
            {
                this._typeMap[stringType.FullName] = TypeScriptTypeNames.String;
            }

            _typeMap[typeof (bool).FullName] = TypeScriptTypeNames.Boolean;
            _typeMap[typeof (void).FullName] = TypeScriptTypeNames.Void;
            _typeMap[typeof(object).FullName] = TypeScriptTypeNames.Any;
            _typeMap[typeof(DateTime).FullName] = TypeScriptTypeNames.Date;
            _typeMap[typeof(DateTimeOffset).FullName] = TypeScriptTypeNames.Date;

            _typeMap["System.Collections.Generic.List"]               = TypeScriptTypeNames.Array;
            _typeMap["System.Collections.Generic.IList"]              = TypeScriptTypeNames.Array;
            _typeMap["System.Collections.Generic.Collection"]         = TypeScriptTypeNames.Array;
            _typeMap["System.Collections.Generic.ICollection"]        = TypeScriptTypeNames.Array;
            _typeMap[typeof(System.Collections.ICollection).FullName] = TypeScriptTypeNames.Array;
            _typeMap["System.Collections.Generic.IEnumerable"]        = TypeScriptTypeNames.Array;
            _typeMap[typeof(System.Collections.IEnumerable).FullName] = TypeScriptTypeNames.Array;
            _typeMap[typeof(System.Array).FullName]                   = TypeScriptTypeNames.Array;
        }

        public bool IsValidTypeForDerivation(CodeTypeReference type)
        {
            return !type.BaseType.Equals(typeof (object).FullName);
        }

        public string GetTypeOutput(CodeTypeReference type)
        {
            if (!_baseTypeRegex.IsMatch(type.BaseType))
                throw new ArgumentException("Type mismatch");

            var baseTypeName = _baseTypeRegex
                .Match(type.BaseType)
                .Groups["TypeName"]
                .Captures[0]
                .Value;

            string typeOutputString;

            if (baseTypeName.Contains("Nullable"))
            {
                typeOutputString = GetTypeArgument(type);
            }
            else
            {
                typeOutputString = TranslateType(baseTypeName);

                if (type.TypeArguments.Count > 0)
                    typeOutputString = AddTypeArguments(type, typeOutputString);
            }

            if (_arrayRegex.IsMatch(type.BaseType))
                return GetArrayType(type.BaseType, typeOutputString);

            if (type.ArrayRank > 0)
                return GetArrayString(typeOutputString, type.ArrayRank);
                        
            return typeOutputString;
        }

        private string AddTypeArguments(CodeTypeReference type, string typeOutputString)
        {
            var typeArguments = type.TypeArguments
                .OfType<CodeTypeReference>()
                .Select(GetTypeOutput);

            return $"{typeOutputString}<{string.Join(", ", typeArguments)}>";
        }

        private string GetTypeArgument(CodeTypeReference type)
        {
            var typeArguments = type.TypeArguments
                .OfType<CodeTypeReference>()
                .Select(GetTypeOutput);

            return $"{string.Join(", ", typeArguments)}";
        }

        private string TranslateType(string baseTypeName)
        {
            return _typeMap.ContainsKey(baseTypeName) ? _typeMap[baseTypeName] : baseTypeName;
        }

        private string GetArrayType(string baseType, string actualTypeName)
        {
            var matches = _arrayRegex.Match(baseType);
            var jaggedcount = matches.Groups["JaggedRank"].Captures.Count;
            var dimensionalArrayCount = matches.Groups["DimensionalRank"].Captures.Count;
            if (jaggedcount > 0)
            {
                return GetArrayString(actualTypeName, jaggedcount + 1);
            }
            if (dimensionalArrayCount > 0)
            {
                return GetArrayString(actualTypeName, dimensionalArrayCount + 1);
            }

            return GetArrayString(actualTypeName, 1);
        }

        private string GetArrayString(string baseType, int count)
        {
            return count == 0 ? baseType : $"Array<{GetArrayString(baseType, count-1)}>";
        }
    }
}