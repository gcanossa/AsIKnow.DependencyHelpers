using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AsIKnow.DependencyHelpers
{
    public class UnavailableDependencyException : Exception
    {
        public UnavailableDependencyException()
            :base()
        {}
        public UnavailableDependencyException(string message)
            : base(message)
        { }
        public UnavailableDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
        public UnavailableDependencyException(SerializationInfo info, StreamingContext ctx)
            :base(info, ctx)
        {}
    }
}
