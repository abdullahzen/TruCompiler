
# TruCompiler 
<p align="left">
  <a href="https://github.com/abdullahzen/TruCompiler/actions"><img alt="GitHub Actions status" src="https://github.com/abdullahzen/TruCompiler/workflows/Build/badge.svg"></a>
</p>

# SOEN 442 Compiler Design: Assignments and Project 

## Project Run Instructions:

1. Make sure you have `dotnet` core installed on your machine. If it's not installed, you may do so from [here](https://dotnet.microsoft.com/download).
1. It's best if the program is run on a windows machine.
1. CD to the root project directory and execute the following for the project to build:
`dotnet build --configuration Release`
1. CD to /TruCompiler/TruCompiler where the TruCompiler.csproj is located and execute the following to run the project:
`dotnet run -input "path_to_files,separated_by_commas,to_compile_multiple_files"` 
 >Note that the outpath will always be the directory of the source code provided
5. CD to the directory of your source code to find the output files in both extensions .outlextokens, .outlexerrors, outderivations, outast, and m that correspond to the tokens, errors, derivations, abstract syntax tree DOT file, and moon generated code, respectively.
6. In order to run the unit tests, you may use the following command at the root of the project where the .sln file exists:
`dotnet test --configuration Release`

>Please note that all the test cases include accepted, edge and extreme cases in the TruCompilerTests.

Private github repo: https://github.com/abdullahzen/TruCompiler
