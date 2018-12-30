using System.Collections.Generic;

namespace GherkinToMarkdown.Gherkin
{
    public class Feature
    {
        private readonly string name;
        private readonly Optional<string> description;
        private readonly List<string> tags;
        private readonly List<Scenario> scenarios;
        private readonly List<ScenarioOutline> scenarioOutlines;
        private readonly List<Background> backgrounds;

        public Feature(string name, Optional<string> description, IEnumerable<string> tags)
        {
            this.name = name;
            this.description = description;
            this.tags = new List<string>(tags);
            this.scenarios = new List<Scenario>();
            this.scenarioOutlines = new List<ScenarioOutline>();
            this.backgrounds = new List<Background>();
        }

        public void Add(Scenario scenario)
        {
            scenarios.Add(scenario);   
        }

        public void Add(ScenarioOutline scenarioOutline)
        {
            scenarioOutlines.Add(scenarioOutline);
        }

        public void Add(Background background)
        {
            backgrounds.Add(background);
        }

        public string Name => name;

        public Optional<string> Description => description;

        public List<string> Tags => tags;

        public List<Scenario> Scenarios => scenarios;

        public List<ScenarioOutline> ScenarioOutlines => scenarioOutlines;

        public List<Background> Backgrounds => backgrounds;
    }
}