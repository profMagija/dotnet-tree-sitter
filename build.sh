
so="so"

# build main lib

native_lib_dir="tree-sitter/lib/src"
dotnet_lib_dir="TreeSitter"

gcc -fPIC -shared -o $dotnet_lib_dir/tree-sitter.$so $native_lib_dir/lib.c -I$native_lib_dir -I$native_lib_dir/../include

# build C language

native_dir="langs-native/tree-sitter-c/src"
dotnet_dir="TreeSitter.C"

gcc -fPIC -shared -o $dotnet_dir/tree-sitter-c.$so $native_dir/parser.c -I$native_dir

# build jaavscript language

native_dir="langs-native/tree-sitter-javascript/src"
dotnet_dir="TreeSitter.JavaScript"

gcc -fPIC -shared -o $dotnet_dir/tree-sitter-javascript.$so $native_dir/parser.c $native_dir/scanner.c -I$native_dir

# build managed things

dotnet build