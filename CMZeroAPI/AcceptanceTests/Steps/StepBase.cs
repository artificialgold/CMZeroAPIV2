using System;
using System.Collections.Generic;
using System.Linq;

using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps
{
    public class StepBase
    {        
        private static string KeyFromType<T>(string tag)
        {
            return String.Format("{0}*{1}", typeof(T).Name, tag ?? String.Empty);
        }

        private static string KeyFromType<T>()
        {
            return KeyFromType<T>(null);
        }

        protected void Remember<T>(T thingToRemember)
        {
            ScenarioContext.Current.Add(KeyFromType<T>(), thingToRemember);
        }

        protected void Remember<T>(T thingToRemember, string tag)
        {
            ScenarioContext.Current.Add(KeyFromType<T>(tag), thingToRemember);
        }

        protected T Recall<T>()
        {
            return Recall<T>(null);
        }

        protected T Recall<T>(string tag)
        {
            if (ScenarioContext.Current.ContainsKey(KeyFromType<T>(tag)))
            {
                return (T)ScenarioContext.Current[KeyFromType<T>(tag)];
            }
            throw new UnknownKeyRecallError(ScenarioContext.Current.Keys, KeyFromType<T>(tag));
        }

        public class UnknownKeyRecallError : Exception
        {
            public UnknownKeyRecallError(IEnumerable<string> knownKeys, string fullKey)
                : base(BuildMessage(knownKeys, fullKey))
            {
            }

            private static string BuildMessage(IEnumerable<string> keys, string fullKey)
            {
                return String.Format(
                    "Could not find key type {0}.  Known keys: [{1}]",
                    fullKey,
                    keys.Aggregate((j, i) => j + "," + i));
            }
        }

        public string CurrentScenarioTitle()
        {
            return ScenarioContext.Current.ScenarioInfo.Title;
        }

        public string CurrentFeatureTitle()
        {
            return FeatureContext.Current.FeatureInfo.Title;
        }
    }
}