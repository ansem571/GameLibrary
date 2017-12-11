using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Interfaces
{
    public interface IPlayerStats : IStats
    {
        int Deaths { get; set; }
        int CurrentExp { get; set; }
        int MaxExp { get; set; }

        void LevelUp();
    }
}
