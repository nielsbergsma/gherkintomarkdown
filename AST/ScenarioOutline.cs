using System.Collections.Generic;
using System.Linq;

namespace GherkinToMarkdown.Gherkin
{
    public class ScenarioOutline
    {
        private readonly string title;
        private readonly Optional<string> description;
        private readonly List<string> tags;
        private readonly List<Step> steps;

        public ScenarioOutline(string title, Optional<string> description, IEnumerable<string> tags)
        {
            this.title = title;
            this.description = description;
            this.tags = new List<string>(tags);
            this.steps = new List<Step>();
        }
        
        public void AddStep(Step step)
        {
            switch (step)
            {
                case And _:
                case But _:
                    if (steps.Any())
                    {
                        steps.Last().AddCombinatorialStep(step);
                    }
                    break;
                
                default:
                    steps.Add(step);
                    break;
            }
        }
        
        public void SetExample(Example example)
        {
            if (steps.Any())
            {
                steps.Last().SetExample(example);
            }
        }

        public string Title => title;

        public Optional<string> Description => description;

        public List<string> Tags => tags;
        
        public List<Step> Steps => steps;
    }
}