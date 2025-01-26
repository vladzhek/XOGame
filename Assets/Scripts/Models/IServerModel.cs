using System;
using Data;

namespace Models
{
    public interface IServerModel
    {
        public event Action<ETurnPlayers> OnTurnPlayer;
        public event Action<CellData> OnUpdatePoints;
        
        public void InitializeStartGame(EPlayerType serverLocate, CellData[,] mainMatrix);
        public void UpdateDataCell(CellData data, ETurnPlayers player);
        public void PlayerTurn(ETurnPlayers turnPlayer);
        public void WinnerGame(CellType cellType);
    }
}