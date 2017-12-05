using System;
using System.Runtime.Serialization;

namespace GameLibrary.Exceptions
{
    public class CharacterApiException : Exception
    {
        public CharacterApiException()
        {
        }

        public CharacterApiException(string message) : base(message)
        {
        }

        public CharacterApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CharacterApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
