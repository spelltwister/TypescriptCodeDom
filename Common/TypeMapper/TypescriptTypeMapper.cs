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

        protected static class TypeScriptTypeNames
        {
            public static readonly string Number  = "number";
            public static readonly string String  = "string";
            public static readonly string Boolean = "boolean";
            public static readonly string Void    = "void";
            public static readonly string Any     = "any";
            public static readonly string Date    = "Date";
            public static readonly string Array   = "Array";
        }

        protected static readonly string DictionaryFormatString = "{{ [key: {0}] : {1} }}";

        #region Number Types
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

        protected virtual bool IsNumberType(Type type)
        {
            return NumberTypes.Any(x => type == x);
        }

        protected virtual bool IsNumberType(CodeTypeReference typeReference)
        {
            return NumberTypes.Select(GetTypeBaseName)
                              .Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, GetTypeBaseName(typeReference)));
        }
        #endregion

        #region String Types
        private static readonly Type[] StringTypes = new Type[]
        {
            typeof(string),
            typeof(Guid  ),
            typeof(char  )
        };

        protected virtual bool IsStringType(Type type)
        {
            return StringTypes.Any(x => type == x);
        }

        protected virtual bool IsStringType(CodeTypeReference typeReference)
        {
            return StringTypes.Select(GetTypeBaseName)
                              .Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, GetTypeBaseName(typeReference)));
        }
        #endregion

        #region Array Types
        private static readonly Type[] ArrayTypes = new Type[]
        {
            typeof (System.Collections.Generic.List<>),
            typeof (System.Collections.Generic.IList<>),
            typeof (System.Collections.Generic.ICollection<>),
            typeof (System.Collections.Generic.IEnumerable<>),
            typeof (System.Collections.ObjectModel.Collection<>),
            typeof (System.Collections.ICollection),
            typeof (System.Collections.IEnumerable),
            typeof (System.Array)
        };

        protected virtual bool IsArrayType(Type type)
        {
            return ArrayTypes.Any(x => type == x);
        }

        protected virtual bool IsArrayType(CodeTypeReference typeReference)
        {
            return ArrayTypes.Select(GetTypeBaseName)
                             .Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, GetTypeBaseName(typeReference)));
        }
        #endregion

        #region Dictionary Types
        private static readonly Type[] DictionaryTypes = new Type[]
        {
            typeof (IDictionary<,>),
            typeof ( Dictionary<,>)
        };

        protected virtual bool IsDictionaryType(Type type)
        {
            return DictionaryTypes.Any(x => type == x);
        }

        protected virtual bool IsDictionaryType(CodeTypeReference typeReference)
        {
            return DictionaryTypes.Select(GetTypeBaseName)
                                  .Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, GetTypeBaseName(typeReference)));
        }
        #endregion

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

            _typeMap[typeof(bool).FullName] = TypeScriptTypeNames.Boolean;
            _typeMap[typeof(void).FullName] = TypeScriptTypeNames.Void;
            _typeMap[typeof(object).FullName] = TypeScriptTypeNames.Any;
            _typeMap[typeof(DateTime).FullName] = TypeScriptTypeNames.Date;
            _typeMap[typeof(DateTimeOffset).FullName] = TypeScriptTypeNames.Date;

            foreach (Type arrayType in ArrayTypes)
            {
                _typeMap[GetTypeBaseName(arrayType)] = TypeScriptTypeNames.Array;
            }

            foreach (Type dictionaryType in DictionaryTypes)
            {
                _typeMap[GetTypeBaseName(dictionaryType)] = DictionaryFormatString;
            }
        }

        protected string GetTypeBaseName(Type type)
        {
            return GetTypeBaseNamePrivate(type.FullName);
        }

        private static string GetTypeBaseNamePrivate(string typeFullName)
        {
            int backTickIndex = typeFullName.IndexOf('`');
            if(backTickIndex == -1)
            {
                return typeFullName;
            }
            return typeFullName.Substring(0, backTickIndex);
        }

        public bool IsValidTypeForDerivation(CodeTypeReference type)
        {
            return !type.BaseType.Equals(typeof (object).FullName);
        }

        protected string GetTypeBaseName(CodeTypeReference type)
        {
            if (!_baseTypeRegex.IsMatch(type.BaseType))
                throw new ArgumentException("Type mismatch");

            return _baseTypeRegex
                .Match(type.BaseType)
                .Groups["TypeName"]
                .Captures[0]
                .Value;
        }

        protected virtual string UpdateBaseTypeNameWithTypeArgsInner(string currentTypeName, CodeTypeReference typeReference)
        {
            return AddTypeArguments(typeReference, currentTypeName);
        }

        protected string UpdateBaseTypeNameWithTypeArgs(string currentTypeName, CodeTypeReference typeReference)
        {
            if (GetTypeBaseName(typeof(System.Nullable)).Equals(currentTypeName, StringComparison.OrdinalIgnoreCase))
            {
                return GetTypeOutput(typeReference.TypeArguments.OfType<CodeTypeReference>().Single());
            }

            if (IsDictionaryType(typeReference))
            {
                // current type name should have the dictionary format string
                if (typeReference.TypeArguments.Count == 2)
                {
                    // TODO: consider translate type and check for number or string result
                    string indexTypeIdentifier;
                    if (IsNumberType(typeReference.TypeArguments[0]))
                    {
                        indexTypeIdentifier = TypeScriptTypeNames.Number;
                    }
                    else if (IsStringType(typeReference.TypeArguments[0]))
                    {
                        indexTypeIdentifier = TypeScriptTypeNames.String;
                    }
                    else
                    {
                        throw new ArgumentException("Cannot create dictionary.  Key type must be string or number type.");
                    }

                    return String.Format(currentTypeName, indexTypeIdentifier, GetTypeOutput(typeReference.TypeArguments[1]));
                }

                throw new ArgumentException("Cannot create dictionary.  There must be exactly 2 type arguments.");
            }

            return UpdateBaseTypeNameWithTypeArgsInner(currentTypeName, typeReference);
        }

        public string GetTypeOutput(CodeTypeReference type)
        {
            string typeOutputString = TranslateType(GetTypeBaseName(type));
            if (type.TypeArguments.Count > 0)
            {
                typeOutputString = UpdateBaseTypeNameWithTypeArgs(typeOutputString, type);
            }

            if (_arrayRegex.IsMatch(type.BaseType))
                return GetArrayType(type.BaseType, typeOutputString);

            if (type.ArrayRank > 0)
                return GetArrayString(typeOutputString, type.ArrayRank);
                        
            return typeOutputString;
        }

        protected string AddTypeArguments(CodeTypeReference type, string typeOutputString)
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

        protected virtual string TranslateType(string baseTypeName)
        {
            return _typeMap.ContainsKey(baseTypeName) ? _typeMap[baseTypeName] : baseTypeName;
        }

        private string GetArrayType(string baseType, string actualTypeName)
        {
            var matches = _arrayRegex.Match(baseType);
            var jaggedcount = matches.Groups["JaggedRank"].Captures.Count;
            if (jaggedcount > 0)
            {
                return GetArrayString(actualTypeName, jaggedcount + 1);
            }

            var dimensionalArrayCount = matches.Groups["DimensionalRank"].Captures.Count;
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