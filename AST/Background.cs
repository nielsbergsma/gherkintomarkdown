using System.Collections.Generic;
using System.Linq;

namespace GherkinToMarkdown.Gherkin
{
    public class Background
    {
        private readonly string title;
        private readonly Optional<string> description;
        private readonly List<Step> steps;

        public Background(string title, Optional<string> description)
        {
            this.title = title;
            this.description = description;
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

        public List<Step> Steps => steps;
    }
}