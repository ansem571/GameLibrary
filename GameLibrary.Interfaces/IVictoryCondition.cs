using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Interfaces
{
    public interface IVictoryCondition
    {
        bool VictoryAchieved { get; }
        void VictoryConditionAchieved(params object[] args);

        void DisplayVictoryMessage(params object[] args);
    }
}
