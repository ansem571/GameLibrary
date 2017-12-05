using System.Collections.Generic;

namespace GameLibrary.Interfaces
{
    public interface IReader
    {
        int Width { get; }
        int Height { get; }
        List<List<ITile>> Tiles { get; }

        void ReadDocument(string path);
    }
}
