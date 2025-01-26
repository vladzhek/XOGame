using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Services
{
    public class StaticDataService
    {
        public EPlayerType PlayerType;
        public ETurnPlayers CurrentPlayer;
        public Dictionary<WindowType, WindowData> Windows = new();
        public Dictionary<ETurnPlayers, int> PlayerPoints = new();
        public bool IsCanClicked;
        
        public void Load()
        {
            LoadData();
            LoadWindows();
        }

        private void LoadData()
        {
            PlayerPoints.Add(ETurnPlayers.Player1, 0);
            PlayerPoints.Add(ETurnPlayers.Player2, 0);
        }
        
        private void LoadWindows()
        {
            var data = Resources.Load<AllWindowsData>("Configs/AllWindowData");
            foreach (var window in data.Windows)
            {
                Windows.Add(window.Type, window);
            }
        }

        public void RebutData()
        {
            PlayerPoints[ETurnPlayers.Player1] = 0;
            PlayerPoints[ETurnPlayers.Player2] = 0;
        }
    }
}