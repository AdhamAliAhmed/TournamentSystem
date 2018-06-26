using System;
using System.Runtime.Serialization;

namespace TournamentSystem.Exceptions
{
    internal class RecordException : Exception
    {
        public RecordException()
            : base()
        {
        }

        public RecordException(string message)
            : base(message)
        {
        }

        public RecordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RecordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}