using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AsIKnow.DependencyHelpers
{
    public class UnavailableDependencyException : AggregateException
    {
        public UnavailableDependencyException()
            :base()
        {}
        public UnavailableDependencyException(string message)
            : base(message)
        { }
        public UnavailableDependencyException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        { }
        public UnavailableDependencyException(SerializationInfo info, StreamingContext ctx)
            :base(info, ctx)
        {}
    }
}
