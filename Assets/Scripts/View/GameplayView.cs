using System;
using Data;
using Infastructure;
using Models;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace View
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _turnText;
        [SerializeField] private TMP_Text _p1Text;
        [SerializeField] private TMP_Text _p2Text;
        
        [Inject] private IServerModel _serverModel;
        [Inject] private StaticDataService _dataService;
        
        private void Awake()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            _serverModel.OnTurnPlayer += UpdateTurnPlayer;
            _serverModel.OnUpdatePoints += UpdatePoints;
        }

        private void OnDisable()
        {
            _serverModel.OnTurnPlayer -= UpdateTurnPlayer;
            _serverModel.OnUpdatePoints -= UpdatePoints;
        }

        private void UpdatePoints(CellData cellData)
        {
            _p1Text.text = "Player1\n" + _dataService.PlayerPoints[ETurnPlayers.Player1];
            _p2Text.text = "Player2\n" + _dataService.PlayerPoints[ETurnPlayers.Player2];
        }

        private void UpdateTurnPlayer(ETurnPlayers turn)
        {
            _turnText.text = "TURN " + turn;
        }
    }
}