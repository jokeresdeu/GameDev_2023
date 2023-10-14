using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples
{
    public class PortalSequence : MonoBehaviour
    {
        [SerializeField] private float _sequenceFloatTime;
        
        [Header("OutOfHouse")]
        [SerializeField] private Rigidbody2D _fox;
        [SerializeField] private Transform _outOfHousePoint;
        [SerializeField] private float _outOfHouseTime;
        [SerializeField] private Transform _behindPortalPoint;
        [SerializeField] private float _foxUpTime;
        [SerializeField] private float _rotationAngle;
        [SerializeField] private Ease _upEase;
        
        [Header("PortalSettings")]
        [SerializeField] private Transform _portal;
        [SerializeField] private float _portalOpenTime;
        [SerializeField] private Ease _portalOpenSequence;
        [SerializeField] private Ease _portalCloseSequence;

        [Header("Ray")]
        [SerializeField] private Transform _ray;
        [SerializeField] private float _showRayTime;
        [SerializeField] private Ease _rayShowSequence;
        [SerializeField] private Ease _rayHideSequence;
        
        private void Start()
        {
            GetPortalSequence();
        }

        private Sequence GetPortalSequence()
        {
            var portalOpenSize = _portal.transform.localScale;
            _portal.transform.localScale = new Vector3(0, portalOpenSize.y, 0);
            var rayOpenSize = _ray.transform.localScale;
            _ray.transform.localScale = new Vector3(rayOpenSize.x, 0, 0);
            var sequence = DOTween.Sequence();
            sequence.Append(_fox.DOMove(_outOfHousePoint.position, _outOfHouseTime));
            sequence.Append(_portal.DOScale(portalOpenSize, _portalOpenTime).SetEase(_portalOpenSequence));
            sequence.Append(_ray.DOScale(rayOpenSize, _showRayTime).SetEase(_rayShowSequence));
            var moveUpSequence = DOTween.Sequence();
            moveUpSequence.Join(_fox.DOMove(_behindPortalPoint.position, _foxUpTime).SetEase(_upEase));
            moveUpSequence.Join(_fox.DORotate(_rotationAngle, _foxUpTime).SetEase(_upEase));
            sequence.Append(moveUpSequence);

            sequence.Append(_ray.DOScale(new Vector3(rayOpenSize.x, 0, 0), _showRayTime).SetEase(_rayHideSequence));
            sequence.Append(_portal.DOScale(new Vector3(0, portalOpenSize.y, 0), _portalOpenTime).SetEase(_portalCloseSequence));
            sequence.SetLoops(100);
            sequence.timeScale = _sequenceFloatTime;
            return sequence;
        }
    }
}