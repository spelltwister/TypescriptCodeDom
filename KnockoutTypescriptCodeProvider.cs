using System;
using System.CodeDom.Compiler;
using Microsoft.Practices.Unity;

namespace TypescriptCodeDom
{
    public class KnockoutTypescriptCodeProvider : CodeDomProvider
    {
        private readonly ICodeGenerator _codeGenerator;

        public KnockoutTypescriptCodeProvider()
        {
            var unity = new UnityContainer();
            var startup = new KnockoutStartup(unity);
            startup.Initialize();
            _codeGenerator = unity.Resolve<ICodeGenerator>();
        }

        [Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
        public override ICodeGenerator CreateGenerator()
        {
            return _codeGenerator;
        }

        [Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
        public override ICodeCompiler CreateCompiler()
        {
            throw new NotImplementedException();
        }
    }
}