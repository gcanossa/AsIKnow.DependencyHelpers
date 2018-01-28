using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.DependencyHelpers
{
    public class DependencyCheckerOptions
    {
        /// <summary>
        /// Check interval in seconds
        /// </summary>
        public uint CheckInterval { get; set; } = 5;
        /// <summary>
        /// Check timeout in seconds
        /// </summary>
        public uint CheckTimeout { get; set; } = 60;

        /// <summary>
        /// Parameters for specific checks, matching checks names
        /// </summary>
        public Dictionary<string, Dictionary<string,string>> CheckParameters { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        public T GetCheckParameter<T>(string key) where T : DependencyCheckParameterBase
        {
            if (!CheckParameters.ContainsKey(key))
                return null;
            else
                return (T)Activator.CreateInstance(typeof(T), new object[] { CheckParameters[key] });
        }
    }
}
