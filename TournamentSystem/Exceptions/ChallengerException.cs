using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TournamentSystem.Exceptions
{
    internal class ChallengerException : Exception
    {
        public ChallengerException()
            : base()
        {
        }

        public ChallengerException(string message)
            : base(message)
        {
        }

        public ChallengerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ChallengerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
