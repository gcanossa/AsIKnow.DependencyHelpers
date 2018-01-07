using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.DependencyHelpers
{
    public class DependencyCheckerBuilderStage<T> where T : class, IDependencyCheck
    {
        public static implicit operator DependencyCheckerBuilder(DependencyCheckerBuilderStage<T> obj)
        {
            return obj.Builder;
        }

        public DependencyCheckerBuilder Builder { get; protected set; }
        public T Check { get; protected set; }

        public DependencyCheckerBuilderStage(DependencyCheckerBuilder builder, T check)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Check = check ?? throw new ArgumentNullException(nameof(check));
        }
    }
}
