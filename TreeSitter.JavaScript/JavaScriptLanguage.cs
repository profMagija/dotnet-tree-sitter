using System;
using System.Runtime.InteropServices;

namespace TreeSitter.JavaScript
{
    public class JavaScriptLanguage
    {    
        private const string DllName = "tree-sitter-javascript";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_javascript();

        public static Language Create() => new Language(tree_sitter_javascript());
    }
}
