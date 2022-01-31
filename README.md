# dotnet-tree-sitter

Dotnet bindings for [tree-sitter](https://tree-sitter.github.io/tree-sitter/).

## Cloning

Be sure to clone with `--recurse-submodules`, or run

```shell
git submodule update --init --recursive
```

if you have already cloned.

## Building

To build everything, run the `build.py` script:

```shell
python build.py
```

This will use `gcc` for building native libraries, which are distributed as submodules.

## Usage

(these examples are somewhat taken from the [py-tree-sitter](https://github.com/tree-sitter/py-tree-sitter) repo).

### Basic parsing

Create a parser and set its language to a Python language instance.

```c#
using TreeSitter;
using TreeSitter.Python;

var language = PythonLanguage.Create();
var parser = new Parser {Language = language};
```

Parse some source code:

```c#
var tree = parser.Parse(@"
def foo():
    bar()
");
```

> NOTE: Parsing a string will convert it into UTF-16 bytes, thus all byte indices will be double the character indices in the C# string.

Inspect the resulting tree:

```c#
Debug.Assert(tree.Root.ToString() == Trim(
    @"(module (function_definition
        name: (identifier)
        parameters: (parameters)
        body: (block (expression_statement (call
            function: (identifier)
            arguments: (argument_list))))))"));
```

(`Trim` is a utility function that replaces multiple whicespace characters with a single space).