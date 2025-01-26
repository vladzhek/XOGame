using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AllWindowData", menuName = "Data/AllWindowData")]
    public class AllWindowsData : ScriptableObject
    {
        public List<WindowData> Windows;
    }
}