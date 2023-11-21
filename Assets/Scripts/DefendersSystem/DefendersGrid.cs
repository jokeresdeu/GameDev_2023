using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefendersSystem
{
    public class DefendersGrid : MonoBehaviour
    {
        [Header("GridSettings")]
        [SerializeField] private Vector2 _gridSize;
        [SerializeField] private int _horizontalCellsCount;
        [SerializeField] private int _verticalCellsCount;
        
        [SerializeField] private SpriteRenderer _slotSprite;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _nearSelectedColor;
        
        private Vector2 _cellSize;
        private Vector2 _startPos;
        
        private List<DefenderSlot> _slots;

        public void Initialize()
        {
            CalculateGrid();
            CreateSlots();
        }
        
        private void CalculateGrid()
        {
            _startPos = transform.position;
            _cellSize.x = _gridSize.x / _horizontalCellsCount;
            _cellSize.y = _gridSize.y / _verticalCellsCount;
        }
        
        private void CreateSlots()
        {
            _slots = new List<DefenderSlot>();
            int cellsCount = (_verticalCellsCount + 1) * (_horizontalCellsCount + 1);
            for (var y = 0; y < _verticalCellsCount; y++)
            {
                for (var x = 0; x < _horizontalCellsCount; x++)
                {
                    var startPos = new Vector2(_startPos.x + _cellSize.x * x, _startPos.y + _cellSize.y * y);
                    var sprite = Instantiate(_slotSprite, transform);
                    sprite.transform.localScale = _cellSize;
                    sprite.transform.position = startPos + _cellSize / 2;
                    _slots.Add(new DefenderSlot(startPos, sprite, cellsCount - (x+1) * (y+1)));
                }
            }
        }
        
        public bool TryGetDefenderSlot(Vector2 worldPos, out DefenderSlot defenderSlot)
        {
            defenderSlot = null;
            foreach (var slot in _slots)
            {
                if (!PosInsideSlot(slot, worldPos)) 
                    continue;
                defenderSlot = slot;
                return true;
            }

            return false;
        }
        
        public void ShowSelection(DefenderSlot defenderSlot)
        {
            foreach (var slot in _slots)
            {
                if (slot == defenderSlot)
                {
                    slot.ShowCell(_selectedColor);
                    continue;
                }
                
                if (Math.Abs(slot.StartPosition.x - defenderSlot.StartPosition.x) < 0.1 
                    || Math.Abs(slot.StartPosition.y - defenderSlot.StartPosition.y) < 0.1)
                {
                    slot.ShowCell(_nearSelectedColor);
                    continue;
                }
                
                slot.HideCell();
            }
        }

        public void HideSelection()
        {
            foreach(DefenderSlot slot in _slots)
                slot.HideCell();
        }
        
        private bool PosInsideSlot(DefenderSlot defenderSlot, Vector2 worldPos)
        {
            var slotEndPos = defenderSlot.StartPosition + _cellSize;
            return worldPos.x >= defenderSlot.StartPosition.x && worldPos.x <= slotEndPos.x &&
                   worldPos.y >= defenderSlot.StartPosition.y && worldPos.y <= slotEndPos.y;
        }
        
        #region DrawGizmos

        [SerializeField] private bool _drawGizmos;
        private Vector2[,] _cellsPositions;
        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
            {
                return;
            }
            
            CalculateGrid();
            _cellsPositions = new Vector2[_horizontalCellsCount, _verticalCellsCount];
            for (var y = 0; y < _verticalCellsCount; y++)
            {
                for (var x = 0; x < _horizontalCellsCount; x++)
                {
                    var startPos = new Vector2(_startPos.x + _cellSize.x * x, _startPos.y + _cellSize.y * y);
                    _cellsPositions[x,y] = startPos;
                }
            }
           
            Gizmos.color = Color.red;
            var endPos = _startPos + _gridSize;
            for (var y = 0; y < _verticalCellsCount; y++)
            {
                var cellPos = _cellsPositions[0, y];
                Gizmos.DrawLine(cellPos, new Vector2(endPos.x, cellPos.y));
            }

            for (var x = 0; x < _horizontalCellsCount; x++)
            {
                var cellPos = _cellsPositions[x, 0];
                Gizmos.DrawLine(cellPos, new Vector2(cellPos.x, endPos.y));
            }
            
            Gizmos.DrawWireCube( _startPos + _gridSize/2, _gridSize);
        }
        
        #endregion
    }
}