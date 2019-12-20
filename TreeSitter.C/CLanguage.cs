using System;
using System.Runtime.InteropServices;

namespace TreeSitter.C
{
    public class CLanguage
    {    
        private const string DllName = "tree-sitter-c";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_c();

        public static Language Create() => new Language(tree_sitter_c());
    }
}