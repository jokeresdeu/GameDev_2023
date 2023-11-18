using Entities;
using ObjectPool;
using UnityEngine;

namespace DefendersSystem
{
    public class DefenderSlot
    {
        private readonly SpriteRenderer _slotImage;
        private readonly int _sortingOrder;
        
        public Vector2 StartPosition { get; }
       
        public bool IsFree { get; private set; }

        private BaseEntity _defender;
        
        public DefenderSlot(Vector2 startPosition, SpriteRenderer slotImage, int order)
        {
            _sortingOrder = order;
            StartPosition = startPosition;
            _slotImage = slotImage;
            IsFree = true;
        }

        public void ShowCell(Color color)
        {
            _slotImage.enabled = true;
            _slotImage.color = color;
        }

        public void PlaceDefender(BaseEntity defender)
        {
            _defender = defender;
            _defender.SetSortingOrder(_sortingOrder);
            _defender.transform.position = StartPosition;
            _defender.transform.SetParent(_slotImage.transform);
            _defender.ReturnRequested += DefenderRemoved;
            IsFree = false;
        }

        public void RemoveDefender()
        {
            if(IsFree)
                return;
            
            _defender.ReturnRequested -= DefenderRemoved;
            _defender.Remove();
            _defender = null;
            IsFree = true;
        }

        private void DefenderRemoved(IPoolable defender)
        {
            _defender.ReturnRequested -= DefenderRemoved;
            _defender = null;
            IsFree = true;
        }

        public void HideCell() => _slotImage.enabled = false;
    }
}