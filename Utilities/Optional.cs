using System;

namespace GherkinToMarkdown
{
    public class Optional<T>
    {
        private readonly T value;
        private readonly bool hasValue;

        private Optional(T value, bool hasValue)
        {
            this.value = value;
            this.hasValue = hasValue;
        }

        public static Optional<T> None()
        {
            return new Optional<T>(default(T), false);
        }

        public static Optional<T> Some(T value)
        {
            return new Optional<T>(value, true);
        }

        public T Value => value;

        public bool HasValue => hasValue;

        public T OrElse(T fallback)
        {
            return hasValue ? value : fallback;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Optional<T>;
            
            return other != null
                && ((!hasValue && !other.hasValue) || (hasValue && other.hasValue && value.Equals(other.value)));
        }

        public override int GetHashCode()
        {
            if (hasValue)
            {
                return value.GetHashCode();
            }
            else
            {
                return 0;
            }
        }
    }
 
}