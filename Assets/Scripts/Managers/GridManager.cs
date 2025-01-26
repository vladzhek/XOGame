using System;
using Data;
using Infastructure;
using Models;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        [Inject] private IServerModel _serverModel;
        [Inject] private GridModel _gridModel;

        public void OnEnable()
        {
            _gridModel.OnUpdateCell += CheckForWinner;
        }

        public void OnDisable()
        {
            _gridModel.OnUpdateCell -= CheckForWinner;
        }

        private void CheckForWinner(CellData updatedCell)
        {
            if (updatedCell.Type == CellType.None)
                return;

            int i = updatedCell.Position.I;
            int j = updatedCell.Position.J;

            if (CheckRow(i, updatedCell.Type) || 
                CheckColumn(j, updatedCell.Type) || 
                CheckMainDiagonal(updatedCell.Type) || 
                CheckSecondaryDiagonal(updatedCell.Type))
            {
                _serverModel.WinnerGame(updatedCell.Type);
            }
        }

        private bool CheckRow(int row, CellType targetType)
        {
            for (int j = 0; j < Constants.matrixSize; j++)
            {
                if (_gridModel.Matrix[row, j].Type != targetType)
                    return false;
            }
            return true;
        }

        private bool CheckColumn(int col, CellType targetType)
        {
            for (int i = 0; i < Constants.matrixSize; i++)
            {
                if (_gridModel.Matrix[i, col].Type != targetType)
                    return false;
            }
            return true;
        }

        private bool CheckMainDiagonal(CellType targetType)
        {
            for (int i = 0; i < Constants.matrixSize; i++)
            {
                if (_gridModel.Matrix[i, i].Type != targetType)
                    return false;
            }
            return true;
        }

        private bool CheckSecondaryDiagonal(CellType targetType)
        {
            for (int i = 0; i < Constants.matrixSize; i++)
            {
                if (_gridModel.Matrix[i, Constants.matrixSize - 1 - i].Type != targetType)
                    return false;
            }
            return true;
        }
    }
}