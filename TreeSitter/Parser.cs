using System;
using System.Runtime.InteropServices;
using System.Text;
using TreeSitter.Native;
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
            var bytes = Encoding.UTF8.GetBytes(text);

            return Parse(bytes, InputEncoding.Utf8, oldTree);
        }

        public unsafe Tree Parse(byte[] bytes, InputEncoding encoding, Tree oldTree = null)
        {
            fixed (byte* ptr = bytes)
            {
                return new Tree(ts_parser_parse_string_encoding(
                    _handle,
                    oldTree?.Handle ?? IntPtr.Zero,
                    new IntPtr(ptr),
                    (uint) bytes.Length,
                    (TsInputEncoding) encoding));
            }
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

    public enum InputEncoding
    {
        Utf8,
        Utf16
    }
}