using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.DependencyHelpers
{
    public class DependencyCheckerBuilderStage<T>
    {
        public static implicit operator DependencyCheckerBuilder(DependencyCheckerBuilderStage<T> obj)
        {
            return obj.Builder;
        }

        public DependencyCheckerBuilder Builder { get; protected set; }
        public IDependencyCheck Check { get; protected set; }

        public DependencyCheckerBuilderStage(DependencyCheckerBuilder builder, IDependencyCheck check)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Check = check ?? throw new ArgumentNullException(nameof(check));
        }
    }
}
