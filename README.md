# Gherkin feature to markdown generator

## What is it?
A relaxed, basic gherkin to markdown generator, compatible with gitlab's markdown

## What does it support?
- Usual gherkin syntax, from [here](http://www.pepgotesting.com/wp-content/uploads/2016/01/Cucumber-Gherkin-BDD.pdf) and [here](https://docs.cucumber.io/gherkin/reference)
- Table of contents for features

## How was it written?
C# (dotnet core 2.x)

## Dependencies?
- Microsoft's CommandLineUtils package

## How to use it?
```
dotnet GherkinToMarkdown.dll generate <source directory> <destination directory>
```

## Bugs, improvements?
Create a issue or better yet create a pull request