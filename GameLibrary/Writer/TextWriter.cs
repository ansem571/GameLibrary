using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Writer
{
    public class TextWriter : ITextWriter
    {
        public void WriteMessageToConsole(string message)
        {
            Console.WriteLine(message);
        }
        public void WriteMessageToObject(object obj, string message)
        {
            throw new NotImplementedException();
        }
    }
}
