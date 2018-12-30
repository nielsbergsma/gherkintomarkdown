using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using GherkinToMarkdown.Gherkin;

namespace GherkinToMarkdown.MarkdownGenerator
{
    public class Generator
    {
        private readonly List<Feature> features;

        public Generator(IEnumerable<Feature> features)
        {
            this.features = new List<Feature>(features);
        }

        public void GenerateFiles(string destinationPath)
        {
            foreach (var feature in features)
            {
                var filePath = Path.Join(destinationPath, FeatureToFileName(feature) + ".md");
                var content = new StringBuilder();

                Generate(feature).Write(content);
                
                File.WriteAllText(filePath, content.ToString());
            }
        }

        private string FeatureToFileName(Feature feature)
        {
            return string.Concat(
                feature.Name.Select(c => char.IsLetter(c) || char.IsDigit(c) ? char.ToLower(c) : '_')
            );
        }

        private Element Generate(Feature feature)
        {
            var header = MD.H1(MD.Text(feature.Name));

            var tags = MD.Container(
                feature.Tags.Select(t => MD.Fragment(MD.Link("@" + t), MD.Text(" ")))
            );

            var description = MD.Container(
                feature.Description.HasValue
                    ? MD.Multiline(feature.Description.Value,d => MD.Emphasis(MD.Text(d)))
                    : MD.Empty()
                );
            
            var backgrounds = MD.Container(
                feature.Backgrounds.Select(Generate)
            );

            var scenarioOutlines = MD.Container(
                feature.ScenarioOutlines.Select(Generate)
            );
            
            var scenarios = MD.Container(
                feature.Scenarios.Select(Generate)
            );

            var toc = MD.TOC();

            return MD.Fragment(
                header, 
                tags,
                description,
                backgrounds,
                scenarioOutlines,
                scenarios,
                toc
            );
        }
        
        private Element Generate(ScenarioOutline scenarioOutline)
        {
            var header = MD.H2(MD.Text("Outline " + scenarioOutline.Title));
            
            var tags = MD.Container(
                scenarioOutline.Tags.Select(t => MD.Fragment(MD.Link("@" + t), MD.Text(" ")))
            );

            var description = scenarioOutline.Description.HasValue
                ? MD.Multiline(scenarioOutline.Description.Value, d => MD.Emphasis(MD.Text(d)))
                : MD.Empty();

            var steps = scenarioOutline.Steps.SelectMany(Generate);
            
            return MD.Container(
                header,
                tags,
                description,
                steps
            );
        }
        
        private Element Generate(Background background)
        {
            var header = MD.H2(MD.Text("Background " + background.Title));
            
            var description = background.Description.HasValue
                ? MD.Multiline(background.Description.Value, d => MD.Emphasis(MD.Text(d)))
                : MD.Empty();

            var steps = background.Steps.SelectMany(Generate);
            
            return MD.Container(
                header,
                description,
                steps
            );
        }

        private Element Generate(Scenario scenario)
        {
            var header = MD.H3(MD.Text(scenario.Title));
            
            var tags = MD.Container(
                scenario.Tags.Select(t => MD.Fragment(MD.Link("@" + t), MD.Text(" ")))
            );

            var description = scenario.Description.HasValue
                ? MD.Multiline(scenario.Description.Value, d => MD.Emphasis(MD.Text(d)))
                : MD.Empty();

            var steps = scenario.Steps.SelectMany(Generate);
            
            return MD.Container(
                header,
                tags,
                description,
                steps
            );
        }

        private IEnumerable<Element> Generate(Step step)
        {
            var format = new Func<Step,Element>(s => MD.ListItem(
                MD.Strong(MD.Text(s.GetType().Name)),
                MD.Text(" "),
                MD.Text(s.Text)
            ));

            var list = new List<Element>();
            list.Add(format(step));
            list.AddRange(step.CombinatorialSteps.SelectMany(Generate));

            if (step.Example.HasValue)
            {
                list.Add(MD.Table(step.Example.Value.Header, step.Example.Value.Data));
            }

            return list;
        }
    }
}