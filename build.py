from subprocess import run
from node_generator import generate

SO = "so"

def build_main_lib():
    print(" -- building main libraries")
    native_lib_dir = "tree-sitter/lib/src"
    dotnet_lib_dir = "TreeSitter"
    run([
        "gcc",
        "-fPIC",
        "-shared",
        "-o", f"{dotnet_lib_dir}/tree-sitter.{SO}",
        f"{native_lib_dir}/lib.c",
        "free.c",
        f"-I{native_lib_dir}",
        f"-I{native_lib_dir}/../include",
    ], check=True)


def build_lang(native_name, cs_name, *files):
    print(" -- building", native_name, "language support")
    print("    -- building native library")
    native_dir = f"langs-native/tree-sitter-{native_name}/src"
    dotnet_dir = f"TreeSitter.{cs_name}"
    run([
        "gcc",
        "-fPIC",
        "-shared",
        "-o", f"{dotnet_dir}/tree-sitter-{native_name}.{SO}",
        *[f"{native_dir}/{file}" for file in files],
        f"-I{native_name}"
    ], check=True)

    print("    -- generating support code")
    generate(f"{native_dir}/node-types.json", f"{dotnet_dir}/Generated.cs", cs_name)


def build_managed():
    print(" -- building dotnet libraries")
    run(["dotnet", "build"], check=True)


def main():
    build_main_lib()
    build_lang("c", "C", "parser.c")
    build_lang("javascript", "JavaScript", "parser.c", "scanner.c")
    build_lang("python", "Python", "parser.c", "scanner.cc")
    build_managed()


if __name__ == '__main__':
    main()
