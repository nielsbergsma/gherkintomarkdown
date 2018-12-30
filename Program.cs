using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GherkinToMarkdown.Gherkin;
using GherkinToMarkdown.MarkdownGenerator;

//http://www.pepgotesting.com/wp-content/uploads/2016/01/Cucumber-Gherkin-BDD.pdf
//https://docs.cucumber.io/gherkin/reference

namespace GherkinToMarkdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Microsoft.Extensions.CommandLineUtils.CommandLineApplication();
            app.Command("generate", config =>
            {
                config.Description = "generate markdown files from gherkin feature files";
                var source = config.Argument("source", "source directory");
                var destination = config.Argument("destination", "destination directory");
                
                config.OnExecute(() =>
                {
                    Console.WriteLine("generating reading gherkin files from {1}", source.Value, destination.Value);
                    
                    var files = Directory.GetFiles(source.Value, "*.feature");
                    var features = new List<Feature>();

                    foreach (var file in files)
                    {
                        Console.WriteLine("parsing feature file {0}", source);
                        var fileContent = File.ReadAllText(file, Encoding.UTF8);
                        var fileFeatures = Parser.ParseString(fileContent);
                        features.AddRange(fileFeatures);
                    }

                    var generator = new Generator(features);
                    generator.GenerateFiles(destination.Value);
                    
                    return 0;
                });
            });

            app.Execute(args);
        }
    }
}