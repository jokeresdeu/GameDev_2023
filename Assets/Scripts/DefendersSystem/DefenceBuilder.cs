using Entities;
using Extensions.UI;
using PlayerInput.InputController;
using UnityEngine;
using ObjectPool;

namespace DefendersSystem
{
    public class DefenceBuilder : MonoBehaviour
    {
        [SerializeField] private EditorInput _input;
        [SerializeField] private DefendersGrid _defendersGrid;
        [SerializeField] private Clicker _defenderSelector;
        [SerializeField] private Clicker _removeDefenderSelector;
        [SerializeField] private ShooterEntity _baseEntity;
        [SerializeField] private SpriteRenderer _pointer;
        [SerializeField] private Transform _endReachPoint;
        
        private InteractionType _interactionType;

        private void Start()
        {
            _defendersGrid.Initalize();
            _input.Initialize();
            _input.Enabled = true;
            _input.ClickReleased += OnClickReleased;
            _input.Dragged += OnDragged;
            _defenderSelector.Clicked += OnDefenderSelected;
            _defenderSelector.DragStarted += OnDefenderSelected;
            _removeDefenderSelector.Clicked += OnRemoveDefenderSelected;
            _removeDefenderSelector.DragStarted += OnRemoveDefenderSelected;
        }
        
        private void OnDefenderSelected()
        {
            _interactionType = InteractionType.Build;
            _defenderSelector.Interactable = false;
            _removeDefenderSelector.Interactable = false;
        }

        private void OnRemoveDefenderSelected()
        {
            _interactionType = InteractionType.Remove;
            _defenderSelector.Interactable = false;
            _removeDefenderSelector.Interactable = false;
        }

        private void OnDragged(Vector2 pos)
        {
            if(_interactionType == InteractionType.None)
                return;
            
            _pointer.gameObject.SetActive(!_input.IsPointerOverUI());
            _pointer.transform.position = pos;
            
            if(!_defendersGrid.TryGetDefenderSlot(pos, out DefenderSlot defenderSlot))
                return;
            
            if (_interactionType == InteractionType.Remove && !defenderSlot.IsFree)
            {
                _defendersGrid.ShowSelection(defenderSlot.StartPosition);
                return;
            }

            if (defenderSlot.IsFree)
            {
                _defendersGrid.ShowSelection(defenderSlot.StartPosition);
                return;
            }
            
            _defendersGrid.HideSelection();
        }
        
        private void OnClickReleased(Vector2 pos)
        {
            if (_defendersGrid.TryGetDefenderSlot(pos, out DefenderSlot defenderSlot))
            {
                switch (_interactionType)
                {
                    case InteractionType.Build:
                        BuildDefender(defenderSlot);
                        break;
                    case InteractionType.Remove:
                        defenderSlot.RemoveDefender();
                        break;
                }
            }
            
            Reset();
        }

        private void BuildDefender(DefenderSlot defenderSlot)
        {
            if(!defenderSlot.IsFree)
                return;
            
            var defender = ObjectsPool.Instance.GetObject(_baseEntity);
            defender.Initialize(_endReachPoint.position);
            defenderSlot.PlaceDefender(defender);
        }
        
        private void Reset()
        {
            _defendersGrid.HideSelection();
            _pointer.gameObject.SetActive(false);
            _defenderSelector.Interactable = true;
            _removeDefenderSelector.Interactable = true;
            _interactionType = InteractionType.None;
        }
        
        private enum InteractionType
        {
            None = 0,
            Build = 1,
            Remove = 2
        }
    }
}