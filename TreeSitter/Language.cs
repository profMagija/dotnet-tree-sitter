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
            var id = ts_language_field_id_for_name(Handle, fieldName, (uint) fieldName.Length);
            return id == 0 ? (ushort?) null : id;
        }
    }
}