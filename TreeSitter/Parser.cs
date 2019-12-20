using System;
using static TreeSitter.Native.Native;

namespace TreeSitter
{
    public class Parser : IDisposable
    {
        private readonly IntPtr _handle;

        public Parser()
        {
            _handle = ts_parser_new();
        }

        public Language Language
        {
            set
            {
                if (!ts_parser_set_language(_handle, value.Handle))
                    throw new TreeSitterException("Could not set language");
            }
            get => new Language(ts_parser_language(_handle));
        }

        public Tree Parse(string text, Tree oldTree = null)
        {
            return new Tree(ts_parser_parse_string(
                _handle,
                oldTree?.Handle ?? IntPtr.Zero,
                text,
                (uint) text.Length));
        }

        public void Reset()
        {
            ts_parser_reset(_handle);
        }


        public void Dispose()
        {
            ts_parser_delete(_handle);
        }
    }
}