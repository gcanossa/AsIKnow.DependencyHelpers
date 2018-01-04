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
    }
}
