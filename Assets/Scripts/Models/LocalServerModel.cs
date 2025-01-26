using System;
using System.Collections.Generic;
using Data;
using Infastructure;
using Newtonsoft.Json;
using Zenject;
using Photon.Pun;
using Services;
using UnityEngine;

namespace Models
{
    public class LocalServerModel : MonoBehaviourPunCallbacks, IServerModel
    {
        private EPlayerType _serverInit = EPlayerType.None;
        private ETurnPlayers _turnPlayer = ETurnPlayers.Player1;

        public event Action<ETurnPlayers> OnTurnPlayer;
        public event Action<CellData> OnUpdatePoints;

        [Inject] private StaticDataService _dataService;
        [Inject] private GridModel _gridModel;
        [Inject] private WindowService _windowService;

        private void Start()
        {
            gameObject.GetComponent<PhotonView>().ViewID = PhotonNetwork.AllocateViewID(800);
            _turnPlayer = ETurnPlayers.Player1;
        }

        public void InitializeStartGame(EPlayerType serverLocate, CellData[,] mainMatrix)
        {
            _serverInit = serverLocate;
        }

        public void UpdateDataCell(CellData data, ETurnPlayers player)
        {
            string jsonData = JsonConvert.SerializeObject(data);

            photonView.RPC("UpdateDataCellPun", RpcTarget.AllBuffered, jsonData);
            photonView.RPC("UpdatePlayerPoints", RpcTarget.AllBuffered, jsonData);
        }

        public void PlayerTurn(ETurnPlayers turnPlayer)
        {
            _turnPlayer = turnPlayer;
            photonView.RPC("SetPlayerTurn", RpcTarget.AllBuffered, turnPlayer);
        }

        public void WinnerGame(CellType cellType)
        {
            if(_dataService.PlayerType != EPlayerType.Host) return;
            
            ETurnPlayers winner = cellType == CellType.O ? ETurnPlayers.Player2 : ETurnPlayers.Player1;
            Debug.Log("WinnerGame" + cellType );
            photonView.RPC("EndGame", RpcTarget.AllBuffered, winner);
        }

        [PunRPC]
        private void EndGame(ETurnPlayers winPlayer)
        {
            _dataService.PlayerPoints[winPlayer] += Constants.WinPoints;
            _windowService.CloseWindow(WindowType.Gameplay);
            _windowService.OpenWindow(WindowType.EndGame);
        }

        [PunRPC]
        private void SetPlayerTurn(ETurnPlayers turnPlayer)
        {
            if (turnPlayer == _dataService.CurrentPlayer)
            {
                _dataService.IsCanClicked = true;
            }
            else
            {
                _dataService.IsCanClicked = false;
            }
            
            OnTurnPlayer?.Invoke(turnPlayer);
        }

        [PunRPC]
        private void UpdateDataCellPun(string jsonData)
        {
            CellData data = JsonConvert.DeserializeObject<CellData>(jsonData);
            _dataService.PlayerPoints[_turnPlayer] += data.Points;
            data.Points = 0;
            _gridModel.Matrix[data.Position.I, data.Position.J].Points = 0;
            _gridModel.SetDataCell(data);

            _turnPlayer = _turnPlayer switch
            {
                ETurnPlayers.Player1 => ETurnPlayers.Player2,
                ETurnPlayers.Player2 => ETurnPlayers.Player1,
                _ => _turnPlayer
            };

            PlayerTurn(_turnPlayer);
        }
        
        [PunRPC]
        public void UpdatePlayerPoints(string dataJson)
        {
            var cellData = JsonConvert.DeserializeObject<CellData>(dataJson);
            OnUpdatePoints?.Invoke(cellData);
        }
    }
}