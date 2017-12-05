namespace GameLibrary.Interfaces
{
    public interface ITextWriter
    {
        void WriteMessageToConsole(string message);
        void WriteMessageToObject(object obj, string message);
    }
}
