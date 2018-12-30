using System.Collections.Generic;

namespace GherkinToMarkdown.Gherkin
{
    public abstract class Step
    {
        private readonly string text;
        private readonly List<Step> combinatorialSteps;
        private Optional<Example> example;

        protected Step(string text)
        {
            this.text = text;
            this.combinatorialSteps = new List<Step>();
            this.example = Optional<Example>.None();
        }

        public void AddCombinatorialStep(Step step)
        {
            combinatorialSteps.Add(step);
        }

        public void SetExample(Example example)
        {
            this.example = Optional<Example>.Some(example);
        }

        public string Text => text;

        public List<Step> CombinatorialSteps => combinatorialSteps;

        public Optional<Example> Example => example;
    }

    public class Given : Step
    {
        public Given(string text) : base(text)
        {
        }
    }

    public class When : Step
    {
        public When(string text) : base(text)
        {
        }
    }

    public class Then : Step
    {
        public Then(string text) : base(text)
        {
        }
    }

    public class And : Step
    {
        public And(string text) : base(text)
        {
        }
    }

    public class But : Step
    {
        public But(string text) : base(text)
        {
        }
    }
}