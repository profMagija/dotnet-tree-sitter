using System;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TreeSitter;
using TreeSitter.JavaScript;
using TreeSitter.Python;

namespace Tests
{
    public class TestParser
    {
        [Test]
        public void TestSetLanguage()
        {
            var parser = new Parser
            {
                Language = PythonLanguage.Create()
            };

            var tree = parser.Parse("def foo():\n  bar()");
            
            Assert.AreEqual(Trim(@"(module (function_definition
                name: (identifier)
                parameters: (parameters)
                body: (block (expression_statement (call
                    function: (identifier)
                    arguments: (argument_list))))))"), 
                tree.Root.ToString());

            parser.Language = JavaScriptLanguage.Create();

            tree = parser.Parse("function foo() {\n  bar();\n}");
            
            Assert.AreEqual(Trim(@"(program (function_declaration
                name: (identifier)
                parameters: (formal_parameters)
                body: (statement_block
                    (expression_statement
                         (call_expression
                            function: (identifier)
                            arguments: (arguments))))))"),
                tree.Root.ToString());
        }
        
        [Test]
        public void TestMultibyteCharacters()
        {
            var parser = new Parser {Language = JavaScriptLanguage.Create()};

            var source = Encoding.UTF8.GetBytes("'😎' && '🐍'");

            var tree = parser.Parse(source, InputEncoding.Utf8);

            var rootNode = tree.Root;

            var statementNode = rootNode.Child(0);

            var binaryNode = statementNode.Child(0);

            var snakeNode = binaryNode.Child(2);

            Assert.AreEqual("expression_statement", statementNode.Kind);
            Assert.AreEqual("binary_expression", binaryNode.Kind);
            Assert.AreEqual("string", snakeNode.Kind);

            Assert.AreEqual("'🐍'",
                Encoding.UTF8.GetString(source, snakeNode.StartByte, snakeNode.EndByte - snakeNode.StartByte));
        }

        private static string Trim(string a)
        {
            return Regex.Replace(a, @"\s+", " ");
        }
    }
}