using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Assets
{
    public sealed class MyButtonFilling : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public const float LERP_POWER = 0.1f;
        public float _timer = 1.0f;
        public Image _filling;
        private bool _pressed;
        private bool _hovered = false;
        private bool _over;
        private Vector3 _targetScale = Vector3.one;

        private void Update()
        {
            if (_timer < 1 && _hovered)
            {
                _timer += Time.deltaTime;
                _filling.fillAmount = _timer;
            }
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, LERP_POWER);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
            _timer = 0.0f;
            _hovered = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressed = true;
            _timer = 0.0f;
            _hovered = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _over = false;
            _timer = 0.0f;
            _filling.fillAmount = _timer;
            _hovered = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _over = true;
            _timer = 0.0f;
            _hovered = true;
        }
    }
}