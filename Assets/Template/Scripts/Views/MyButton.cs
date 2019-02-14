using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using System;

namespace Assets
{
    public sealed class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public const float LERP_POWER = 0.1f;

        private bool _pressed;
        private bool _over;
        private Vector3 _targetScale = Vector3.one;

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, LERP_POWER);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
            UpdateStyle();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressed = true;
            UpdateStyle();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _over = false;
            UpdateStyle();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _over = true;
            UpdateStyle();
        }

        private void UpdateStyle()
        {
            if (_pressed)
            {
                _targetScale = Vector3.one * 0.9f;
            }
            else
            {
                if (_over)
                    _targetScale = Vector3.one * 1.1f;
                else
                    _targetScale = Vector3.one;
            }
        }

    }
}