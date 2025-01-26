using System;
using System.Collections.Generic;
using Data;
using Infastructure;
using Models;
using Photon.Pun;
using UnityEngine;
using View.UI;
using Zenject;

namespace View
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private CellUI _cellUI;
        
        [Inject] private GridModel _gridModel;

        private Dictionary<PosData, CellUI> _cells = new();

        private void OnEnable()
        {
            _gridModel.OnUpdateCell += UpdateCellUI;
            _gridModel.OnUpdateAll += UpdateAllUI;
            _gridModel.OnCellClicked += UpdateCellUI;
        }

        private void OnDisable()
        {
            _gridModel.OnUpdateCell -= UpdateCellUI;
            _gridModel.OnUpdateAll -= UpdateAllUI;
            _gridModel.OnCellClicked -= UpdateCellUI;
        }

        private void Start()
        {
            InitializeCells();
        }

        private void InitializeCells()
        {
            UpdateAllUI();
        }

        private void UpdateCellUI(CellData data)
        {
            _cells[data.Position].SetData(data);
        }
        
        private void UpdateAllUI()
        {
            foreach (var value in _cells)
            {
                Destroy(value.Value.gameObject);
            }
            _cells.Clear();
            
            for (int i = 0; i < Constants.matrixSize; i++)
            {
                for (int j = 0; j < Constants.matrixSize; j++)
                {
                    var cellPrefab = Instantiate(_cellUI, transform);
                    cellPrefab.SetData(_gridModel.Matrix[i, j]);
                    _cells.Add(_gridModel.Matrix[i, j].Position, cellPrefab);
                }
            }
        }
    }
}