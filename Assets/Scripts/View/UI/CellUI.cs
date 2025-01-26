using System;
using Data;
using Infastructure;
using Models;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View.UI
{
    public class CellUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cellPoints;
        [SerializeField] private XONoneUI _xoNoneImage;
        [SerializeField] private Button _button;
        
        private CellData _currentData = new();
        [Inject] private GridModel _gridModel;
        [Inject] private StaticDataService _dataService;
        [Inject] private IServerModel _serverModel;

        private void Awake()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnCellClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnCellClicked);
        }

        public void SetData(CellData data)
        {
            _currentData.Type = data.Type;
            _currentData.Position = data.Position;
            _currentData.Points = data.Points;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _xoNoneImage.SetType(_currentData.Type);
            _cellPoints.text = _currentData.Points.ToString();
            if(_currentData.Points == 0) _cellPoints.gameObject.SetActive(false);
            else _cellPoints.gameObject.SetActive(true);
        }

        private void OnCellClicked()
        {
            if(!_dataService.IsCanClicked) return;

            if (_dataService.CurrentPlayer == ETurnPlayers.Player1)
            {
                _xoNoneImage.SetType(CellType.X);
                _currentData.Type = CellType.X;
            }
            else
            {
                _xoNoneImage.SetType(CellType.O);
                _currentData.Type = CellType.O;
            }
            _serverModel.UpdateDataCell(_currentData, _dataService.CurrentPlayer);
        }
    }
}