using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class XONoneUI : MonoBehaviour
    {
        [SerializeField] private Sprite _XSprite;
        [SerializeField] private Sprite _OSprite;
        [SerializeField] private Sprite _NoneSprite;
        
        private Image _currentImage;
        
        private void Awake()
        {
            _currentImage = GetComponent<Image>();
        }

        public void SetType(CellType type)
        {
            _currentImage.sprite = type switch
            {
                CellType.None => _NoneSprite,
                CellType.X => _XSprite,
                CellType.O => _OSprite,
                _ => null
            };
        }
    }
}