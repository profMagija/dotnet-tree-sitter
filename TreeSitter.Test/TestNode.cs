using NUnit.Framework;
using TreeSitter.Python;
using TreeSitter.Python.Nodes;

namespace TreeSitter.Test
{
    public class TestNode
    {
        [Test]
        public void TestChildByFieldId()
        {
            var language = PythonLanguage.Create();
            var parser = new Parser {Language = language};
            var tree = parser.Parse("def foo():\n  bar()");
            var rootNode = tree.Root;
            var fnNode = tree.Root.Child(0);

            Assert.IsNull(language.FieldIdForName("nameasdf"));
            var nameFieldQ = language.FieldIdForName("name");
            var aliasFieldQ = language.FieldIdForName("alias");
            
            Assert.IsNotNull(nameFieldQ);
            Assert.IsNotNull(aliasFieldQ);

            var nameField = (ushort) nameFieldQ;
            var aliasField = (ushort) aliasFieldQ;
            
            Assert.IsNull(rootNode.ChildByFieldId(aliasField));
            Assert.IsNull(rootNode.ChildByFieldId(nameField));
            
            Assert.IsNull(fnNode.ChildByFieldId(aliasField));
            Assert.AreEqual("identifier", fnNode.ChildByFieldId(nameField).Kind);

            Assert.AreEqual("identifier", fnNode.ChildByFieldName("name").Kind);
            Assert.IsNull(fnNode.ChildByFieldName("asdfasdfname"));

            Assert.AreEqual(
                fnNode.ChildByFieldName("name"),
                fnNode.ChildByFieldName("name")
            );
        }

        [Test]
        public void TestChildren()
        {
            var language = PythonLanguage.Create();
            var parser = new Parser
            {
                Language = language
            };
            var tree = parser.Parse("def foo():\n  bar()");

            var rootNode = tree.Root;
            Assert.AreEqual("module", rootNode.Kind);
            Assert.AreEqual(0, rootNode.StartByte);
            Assert.AreEqual(36, rootNode.EndByte);
            Assert.AreEqual(new Point(0, 0), rootNode.StartPosition);
            Assert.AreEqual(new Point(1, 14), rootNode.EndPosition);

            var fnNode = rootNode.Child(0);
            Assert.AreEqual("function_definition", fnNode.Kind);
            Assert.AreEqual(0, fnNode.StartByte);
            Assert.AreEqual(36, fnNode.EndByte);
            Assert.AreEqual(new Point(0, 0), fnNode.StartPosition);
            Assert.AreEqual(new Point(1, 14), fnNode.EndPosition);
        }

        [Test]
        public void TestNodeConvert()
        {
            var language = PythonLanguage.Create();
            var parser = new Parser
            {
                Language = language
            };
            var tree = parser.Parse("def foo():\n  bar()");

            var res = PythonLanguageNode.FromNode(tree.Root);
            Assert.IsInstanceOf<Module>(res);

            var module = (Module) res!;
            Assert.IsInstanceOf<FunctionDefinition>(module.Children[0]);
        }
    }
}