using System.Collections.Generic;

namespace GameLibrary.Interfaces
{
    public interface IMap
    {
        int Width { get; }
        int Height { get; }
        Dictionary<IPoint, ITile> Grid { get; }

        void OpenMap(IPoint loc);
        void RedrawMap(IPoint loc);
        void CloseMap(IPoint loc);
    }
}
