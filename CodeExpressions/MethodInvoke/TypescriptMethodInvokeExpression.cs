using System.CodeDom;
using System.CodeDom.Compiler;

namespace TypescriptCodeDom.CodeExpressions.MethodInvoke
{
    public sealed class TypescriptMethodInvokeExpression : ITypescriptMethodInvokeExpression
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly CodeMethodInvokeExpression _codeExpression;
        private readonly CodeGeneratorOptions _options;

        public TypescriptMethodInvokeExpression(
            IExpressionFactory expressionFactory,
            CodeMethodInvokeExpression codeExpression, 
            CodeGeneratorOptions options)
        {
            _expressionFactory = expressionFactory;
            _codeExpression = codeExpression;
            _options = options;
            System.Diagnostics.Debug.WriteLine("TypescriptMethodInvokeExpression Created");
        }

        public string Evaluate()
        {
            var methodExpression = _expressionFactory.GetExpression(_codeExpression.Method, _options);
            var parameters = _codeExpression.Parameters.GetParametersFromExpressions(_expressionFactory, _options);
            
            // TODO: move the indent to caller; there are many places where you would not want indentation
            // eg, var a = getValue();
            return $"{_options.IndentString}{_options.IndentString}{_options.IndentString}{methodExpression.Evaluate()}({parameters})";
        }
    }
}