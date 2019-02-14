using UnityEngine;
using Headjack;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets
{
    public sealed class VRInputModule : BaseInputModule
    {        
        private readonly float _actionDelayTime = 1f;
        private float _timer = 0f;
        private bool _clicked = false, _over = false;
        private Collider _lastCollider;

        public override void Process()
        {
            var currentCollider = App.CrosshairHit.collider;
            if (currentCollider != _lastCollider)
            {
                if (currentCollider != null)
                {
                    ExecuteEvents.Execute(currentCollider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
                }
                else if (_lastCollider != null)
                {
                    ExecuteEvents.Execute(_lastCollider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
                    ResetState();
                }
            }
            else if(currentCollider != null)
                _timer += Time.deltaTime;

            if (currentCollider != null)
            {
                if (Input.GetMouseButtonUp(0))
                    ExecuteEvents.Execute(currentCollider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);

#if GVR
                if (_timer >= _actionDelayTime && !_clicked)
                {
                    _clicked = true;
                    ExecuteEvents.Execute(currentCollider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                }
#endif
            }            
            _lastCollider = currentCollider;
        }

        private void ResetState()
        {
            _over = false;
            _timer = 0f;
            _clicked = false;
        }
    }
}
