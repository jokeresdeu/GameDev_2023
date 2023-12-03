using Entities;
using ObjectPool;
using UnityEngine;

namespace DefendersSystem
{
    public class DefenceBuilder : MonoBehaviour
    {
        [SerializeField] private DefendersGrid _defendersGrid;
        [SerializeField] private ShooterEntity _baseEntity;
        [SerializeField] private SpriteRenderer _pointer;
        [SerializeField] private Transform _endReachPoint;

        private InteractionType _interactionType;
        private Camera _camera;
        
        private void Start()
        {
            _defendersGrid.Initialize();
            
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _interactionType = InteractionType.Build;

            if (Input.GetKeyDown(KeyCode.Alpha2))
                _interactionType = InteractionType.Remove;

            if (_interactionType == InteractionType.None)
                return;

            Vector2 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            if (!_defendersGrid.TryGetDefenderSlot(pos, out DefenderSlot defenderSlot))
            {
                _pointer.gameObject.SetActive(false);
                _defendersGrid.HideSelection();
                return;
            }
            
            _pointer.gameObject.SetActive(true);
            if (Input.GetButtonUp("Fire1"))
            {
                OnClickReleased(defenderSlot);
                return;
            }

            OnDragged(pos, defenderSlot);
        }

        private void OnDragged(Vector3 pos, DefenderSlot defenderSlot)
        {
            if(_interactionType == InteractionType.None)
                return;
            
            _pointer.transform.position = pos;
            if (_interactionType == InteractionType.Remove && !defenderSlot.IsFree)
            {
                _defendersGrid.ShowSelection(defenderSlot);
                return;
            }

            if (defenderSlot.IsFree && _interactionType == InteractionType.Build)
            {
                _defendersGrid.ShowSelection(defenderSlot);
                return;
            }
            
            _defendersGrid.HideSelection();
        }

        private void OnClickReleased(DefenderSlot defenderSlot)
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