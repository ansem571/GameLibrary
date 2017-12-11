using System.Collections.Generic;

namespace GameLibrary.Interfaces
{
    public interface IMap
    {
        void OpenMap(IPoint loc);
        void RedrawMap(IPoint loc);
        void CloseMap(IPoint loc);
        float GetWidth();
        float GetHeight();
        Dictionary<IPoint, ITile> GetMapGrid();
        ITile GetTileByLocation(IPoint point);
    }
}
