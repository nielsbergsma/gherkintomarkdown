using System;
using System.Collections.Generic;
using System.Linq;

namespace GherkinToMarkdown.Gherkin
{
    public class Parser
    {
        private const string titleDescriptionSeparator = "\n\n";

        private readonly List<string> tags;
        private readonly List<Feature> features;

        private Parser()
        {
            tags = new List<string>();
            features = new List<Feature>();
        }

        public static IEnumerable<Feature> ParseString(string content)
        {
            return new Parser().Parse(content);
        }
        
        private IEnumerable<Feature> Parse(string content)
        {
            var tokens = Lexer.LexString(content);
            while(tokens.HasHead)
            {
                switch (tokens.Head.Type)
                {
                    case TokenType.Comment:
                        tokens.Accept();
                        break;
                    
                    case TokenType.Doc:
                        //TODO
                        tokens.Accept();
                        break;
                    
                    case TokenType.Tag:
                        var tagToken = tokens.Accept();
                        tags.Add(tagToken.Text);
                        break;
                    
                    case TokenType.Feature:
                        features.Add(ParseFeature(tokens));
                        break;
                    
                    default:
                        tokens.Accept();
                        break;
                }
            } 

            return features;
        }

        private Feature ParseFeature(TokenStream tokens)
        {
            var featureToken = tokens.Accept();
            
            var textLines = featureToken.Text.Split(titleDescriptionSeparator);
            var title = textLines.First();
            var description = string
                .Join(titleDescriptionSeparator, textLines.Skip(1))
                .Optional();

            var feature = new Feature(title, description, tags.Purge());
            for (var done = false; !done && tokens.HasHead;)
            {
                switch (tokens.Head.Type)
                {
                    case TokenType.Comment:
                        tokens.Accept();
                        break;
                    
                    case TokenType.Doc:
                        //TODO
                        tokens.Accept();
                        break;
                    
                    case TokenType.Tag:
                        var tagToken = tokens.Accept();
                        tags.Add(tagToken.Text);
                        break;
                    
                    case TokenType.Background:
                        feature.Add(ParseBackground(tokens));
                        break;
                    
                    case TokenType.ScenarioOutline:
                        feature.Add(ParseScenarioOutline(tokens));
                        break;
                    
                    case TokenType.Scenario:
                        feature.Add(ParseScenario(tokens));
                        break;
                    
                    default:
                        done = true;
                        break;
                }
            }

            return feature;
        }
       
        private ScenarioOutline ParseScenarioOutline(TokenStream tokens)
        {
            var scenarioToken = tokens.Accept();
            
            var textLines = scenarioToken.Text.Split(titleDescriptionSeparator);
            var title = textLines.First();
            var description = string
                .Join(titleDescriptionSeparator, textLines.Skip(1))
                .Optional();

            var scenarioOutline = new ScenarioOutline(title, description, tags.Purge());

            for (var done = false; !done && tokens.HasHead;)
            {
                switch (tokens.Head.Type)
                {
                    case TokenType.Comment:
                        tokens.Accept();
                        break;
                    
                    case TokenType.Doc:
                        //TODO
                        tokens.Accept();
                        break;

                    case TokenType.Tag:
                        var tagToken = tokens.Accept();
                        tags.Add(tagToken.Text);
                        break;
                    
                    case TokenType.Given:
                        var givenToken = tokens.Accept();
                        scenarioOutline.AddStep(new Given(givenToken.Text));
                        break;
                    
                    case TokenType.When:
                        var whenToken = tokens.Accept();
                        scenarioOutline.AddStep(new When(whenToken.Text));
                        break;
                    
                    case TokenType.Then:
                        var thenToken = tokens.Accept();
                        scenarioOutline.AddStep(new Then(thenToken.Text));
                        break;
                    
                    case TokenType.And:
                        var andToken = tokens.Accept();
                        scenarioOutline.AddStep(new And(andToken.Text));
                        break;
                    
                    case TokenType.But:
                        var butToken = tokens.Accept();
                        scenarioOutline.AddStep(new But(butToken.Text));
                        break;
                    
                    case TokenType.Example:
                        var exampleToken = tokens.Accept();
                        var example = ParseExample(exampleToken.Text);
                        scenarioOutline.SetExample(example);
                        break;
                    
                    default:
                        done = true;
                        break;
                }
            }

            return scenarioOutline;
        }

        private Scenario ParseScenario(TokenStream tokens)
        {
            var scenarioToken = tokens.Accept();
            
            var textLines = scenarioToken.Text.Split("\n\n");
            var title = textLines.First();
            var description = string
                .Join("\n\n", textLines.Skip(1))
                .Optional();
            
            var scenario = new Scenario(title, description, tags.Purge());

            for (var done = false; !done && tokens.HasHead;)
            {
                switch (tokens.Head.Type)
                {
                    case TokenType.Comment:
                        tokens.Accept();
                        break;
                    
                    case TokenType.Doc:
                        //TODO
                        tokens.Accept();
                        break;

                    case TokenType.Tag:
                        var tagToken = tokens.Accept();
                        tags.Add(tagToken.Text);
                        break;
                    
                    case TokenType.Given:
                        var givenToken = tokens.Accept();
                        scenario.AddStep(new Given(givenToken.Text));
                        break;
                    
                    case TokenType.When:
                        var whenToken = tokens.Accept();
                        scenario.AddStep(new When(whenToken.Text));
                        break;
                    
                    case TokenType.Then:
                        var thenToken = tokens.Accept();
                        scenario.AddStep(new Then(thenToken.Text));
                        break;
                    
                    case TokenType.And:
                        var andToken = tokens.Accept();
                        scenario.AddStep(new And(andToken.Text));
                        break;
                    
                    case TokenType.But:
                        var butToken = tokens.Accept();
                        scenario.AddStep(new But(butToken.Text));
                        break;
                    
                    case TokenType.Example:
                        var exampleToken = tokens.Accept();
                        var example = ParseExample(exampleToken.Text);
                        scenario.SetExample(example);
                        break;
                    
                    default:
                        done = true;
                        break;
                }
            }

            return scenario;
        }
        
        private Background ParseBackground(TokenStream tokens)
        {
            var scenarioToken = tokens.Accept();
            
            var textLines = scenarioToken.Text.Split(titleDescriptionSeparator);
            var title = textLines.First();
            var description = string
                .Join(titleDescriptionSeparator, textLines.Skip(1))
                .Optional();
            
            var background = new Background(title, description);

            for (var done = false; !done && tokens.HasHead;)
            {
                switch (tokens.Head.Type)
                {
                    case TokenType.Comment:
                        tokens.Accept();
                        break;
                    
                    case TokenType.Doc:
                        //TODO
                        tokens.Accept();
                        break;

                    case TokenType.Tag:
                        var tagToken = tokens.Accept();
                        tags.Add(tagToken.Text);
                        break;
                    
                    case TokenType.Given:
                        var givenToken = tokens.Accept();
                        background.AddStep(new Given(givenToken.Text));
                        break;
                    
                    case TokenType.When:
                        var whenToken = tokens.Accept();
                        background.AddStep(new When(whenToken.Text));
                        break;
                    
                    case TokenType.Then:
                        var thenToken = tokens.Accept();
                        background.AddStep(new Then(thenToken.Text));
                        break;
                    
                    case TokenType.And:
                        var andToken = tokens.Accept();
                        background.AddStep(new And(andToken.Text));
                        break;
                    
                    case TokenType.But:
                        var butToken = tokens.Accept();
                        background.AddStep(new But(butToken.Text));
                        break;
                    
                    case TokenType.Example:
                        var exampleToken = tokens.Accept();
                        var example = ParseExample(exampleToken.Text);
                        background.SetExample(example);
                        break;
                    
                    default:
                        done = true;
                        break;
                }
            }

            return background;
        }
        
        private Example ParseExample(string input)
        {
            return new Example(input
                .Split('\n')
                .Where(l => l.Contains('|'))
                .Select(l => l
                    .TrimChar('|') //remove first and last column divider
                    .Split('|')
                    .Select(c => c.Trim())
                    .ToArray()
                ));
        }
    }
}