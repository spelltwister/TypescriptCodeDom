using Microsoft.Practices.Unity;
using TypescriptCodeDom.Common.Keyword;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom
{
    public class KnockoutStartup : Startup
    {
        public KnockoutStartup(IUnityContainer unityContainer) : base(unityContainer)
        {
        }

        protected override void RegisterCommonTypes()
        {
            RegisterSingletonType<ITypescriptKeywordsHandler, TypescriptKeywordsHandler>();
            RegisterSingletonType<ITypescriptTypeMapper, KnockoutTypescriptTypeMapper>();
        }
    }
}