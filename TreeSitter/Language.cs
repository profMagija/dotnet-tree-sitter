using System;
using System.Runtime.InteropServices;
using static TreeSitter.Native.Native;

namespace TreeSitter
{
    public class Language
    {
        internal IntPtr Handle;

        public Language(IntPtr handle)
        {
            Handle = handle;
        }

        public ushort? FieldIdForName(string fieldName)
        {
            var ptr = Marshal.StringToHGlobalAnsi(fieldName);
            var id = ts_language_field_id_for_name(Handle, ptr, (uint) fieldName.Length);
            Marshal.FreeHGlobal(ptr);
            return id == 0 ? (ushort?) null : id;
        }
    }
}