using System;
using System.Runtime.InteropServices;

namespace TreeSitter.Python
{
    public class PythonLanguage
    {    
        private const string DllName = "tree-sitter-python";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_python();

        public static Language Create() => new Language(tree_sitter_python());
    }
}
