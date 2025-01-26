using System;
using Unity.VisualScripting;

namespace Data
{
    [Serializable]
    public class CellData
    {
        public int Points;
        public CellType Type;
        public PosData Position;

        public int ID
        {
            get
            {
                return Position.I + Position.J;
            }
        }
    }
}