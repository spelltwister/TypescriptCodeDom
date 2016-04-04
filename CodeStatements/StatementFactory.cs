using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using TypescriptCodeDom.CodeExpressions;
using TypescriptCodeDom.Common.TypeMapper;

namespace TypescriptCodeDom.CodeStatements
{
    public sealed class StatementFactory : IStatementFactory
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly ITypescriptTypeMapper _typescriptTypeMapper;
        private readonly Dictionary<Type, Func<CodeStatement, CodeGeneratorOptions, IStatement>> _statementMap;

        public StatementFactory(IExpressionFactory expressionFactory, ITypescriptTypeMapper typescriptTypeMapper)
        {
            _expressionFactory = expressionFactory;
            _typescriptTypeMapper = typescriptTypeMapper;
            _statementMap = new Dictionary<Type, Func<CodeStatement, CodeGeneratorOptions, IStatement>>();
            ConstructStatementsMap();
            System.Diagnostics.Debug.WriteLine("StatementFactory Created");
        }

        private void ConstructStatementsMap()
        {
            _statementMap[typeof(CodeAssignStatement)] = (codeStatement, options) => new TypescriptAssignStatement(_expressionFactory, (CodeAssignStatement)codeStatement, options);
            _statementMap[typeof(CodeAttachEventStatement)] = (codeStatement, options) => new TypescriptAttachEventStatement(_expressionFactory, (CodeAttachEventStatement)codeStatement, options);
            _statementMap[typeof(CodeCommentStatement)] = (codeStatement, options) => new TypescriptCommentStatement((CodeCommentStatement)codeStatement);
            _statementMap[typeof(CodeConditionStatement)] = (codeStatement, options) => new TypescriptConditionStatement(this, _expressionFactory, (CodeConditionStatement)codeStatement, options);
            _statementMap[typeof(CodeExpressionStatement)] = (codeStatement, options) => new TypescriptExpressionStatement(_expressionFactory, (CodeExpressionStatement)codeStatement, options);
            _statementMap[typeof(CodeGotoStatement)] = (codeStatement, options) => new TypescriptGotoStatement((CodeGotoStatement)codeStatement);
            _statementMap[typeof(CodeIterationStatement)] = (codeStatement, options) => new TypescriptIterationStatement(this, _expressionFactory, (CodeIterationStatement)codeStatement, options);
            _statementMap[typeof(CodeLabeledStatement)] = (codeStatement, options) => new TypescriptLabeledStatement(this, (CodeLabeledStatement)codeStatement, options);
            _statementMap[typeof(CodeMethodReturnStatement)] = (codeStatement, options) => new TypescriptMethodReturnStatement(_expressionFactory, (CodeMethodReturnStatement)codeStatement, options);
            _statementMap[typeof(CodeRemoveEventStatement)] = (codeStatement, options) => new TypescriptRemoveEventStatement(_expressionFactory, (CodeRemoveEventStatement)codeStatement, options);
            _statementMap[typeof(CodeSnippetStatement)] = (codeStatement, options) => new TypescriptSnippetStatement((CodeSnippetStatement)codeStatement);
            _statementMap[typeof(CodeThrowExceptionStatement)] = (codeStatement, options) => new TypescriptThrowExceptionStatement(_expressionFactory, (CodeThrowExceptionStatement)codeStatement, options);
            _statementMap[typeof(CodeTryCatchFinallyStatement)] = (codeStatement, options) => new TypescriptTryCatchFinallyStatement(this, (CodeTryCatchFinallyStatement)codeStatement, options, _typescriptTypeMapper);
            _statementMap[typeof(CodeVariableDeclarationStatement)] = (codeStatement, options) => new TypescriptVariableDeclarationStatement(_expressionFactory, (CodeVariableDeclarationStatement)codeStatement, options, _typescriptTypeMapper);
        }

        public IStatement GetStatement(CodeStatement codeStatement, CodeGeneratorOptions codeGeneratorOptions)
        {
            return _statementMap[codeStatement.GetType()](codeStatement, codeGeneratorOptions);
        }
    }
}