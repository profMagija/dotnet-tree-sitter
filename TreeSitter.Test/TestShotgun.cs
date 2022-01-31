using System;
using System.IO;
using NUnit.Framework;
using TreeSitter.C;
using TreeSitter.C.Nodes;
using TreeSitter.JavaScript;
using TreeSitter.JavaScript.Nodes;
using TreeSitter.Python;
using TreeSitter.Python.Nodes;

namespace TreeSitter.Test;

public class TestShotgun
{
    private static void PerformTest<T>(string examplesDir, Language lang, Func<Node, T> modelCreator)
    {
        foreach (var example in Directory.EnumerateFiles(examplesDir))
        {
            var codeString = File.ReadAllText(example);
            using var parser = new Parser {Language = lang};
            var tree = parser.Parse(codeString);
            Assert.IsFalse(tree.Root.HasError, "has errors: {0} {1}", example, tree.Root);
            var res = modelCreator(tree.Root);
        }
    }

    [Test]
    public void TestJavaScript()
    {
        PerformTest(
            "../../../../langs-native/tree-sitter-javascript/examples",
            JavaScriptLanguage.Create(),
            JavaScriptLanguageNode.FromNode
        );
    }

    [Test]
    public void TestPython()
    {
        PerformTest(
            "../../../../langs-native/tree-sitter-python/examples",
            PythonLanguage.Create(),
            PythonLanguageNode.FromNode
        );
    }

    [Test]
    public void TestC()
    {
        PerformTest(
            "../../../../langs-native/tree-sitter-c/examples",
            CLanguage.Create(),
            CLanguageNode.FromNode
        );
    }
}