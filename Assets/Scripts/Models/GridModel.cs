using System;
using Data;
using Infastructure;
using Random = UnityEngine.Random;

namespace Models
{
    public class GridModel
    {
        public CellData[,] Matrix => matrix;
        public event Action<CellData> OnUpdateCell;
        public event Action OnUpdateAll;
        public event Action<CellData> OnCellClicked;
        
        private CellData[,] matrix = new CellData[Constants.matrixSize, Constants.matrixSize];

        public void InitializeGridCells()
        {
            for (int i = 0; i < Constants.matrixSize; i++)
            {
                for (int j = 0; j < Constants.matrixSize; j++)
                {
                    matrix[i, j] = new CellData()
                    {
                        Points = GetRandomPoints(),
                        Position = new PosData(){I = i,J = j},
                        Type = CellType.None
                    };
                }
            }
        }

        public void SetDataCell(CellData data)
        {
            matrix[data.Position.I, data.Position.J] = data;
            OnUpdateCell?.Invoke(data);
        }
        
        public void SetDataMatrix(CellData[,] data)
        {
            matrix = data;
            OnUpdateAll?.Invoke();
        }

        private int GetRandomPoints()
        {
            var randomPoints = Random.Range(2, 8) * 5;
            return randomPoints;
        }
        
        public CellData[] GetRowData(int rowIndex)
        {
            int size = Constants.matrixSize;
            var rowData = new CellData[size];

            for (int i = 0; i < size; i++)
            {
                rowData[i] = matrix[rowIndex, i];
            }

            return rowData;
        }
    }
}