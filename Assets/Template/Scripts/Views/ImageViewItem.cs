using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace Assets
{
	public sealed class ImageViewItem : MonoBehaviour, IPointerClickHandler
	{
        public enum Style
        {
            Default,
            Hightlighted,
        }

        private const float SCALE_FACTOR = 1.0f;

        public RawImage _image;
        public Text _text;

        public ImageView Parent { get; set; }
        public bool Focused { get { return _style != Style.Default; } }

        public UnityEvent _onSelected, _onHighlighted, _onDefault;
        public Vector3 _backgroundScaleHighlighted = Vector3.one;
        public float _zHighlighted = 0;
        public Font _defaultFont, _highlightedFont;
        private Style _style;

        public void Clicked()
        {
            _onSelected.Invoke();
        }

        public void ApplyStyle(Style style)
        {
            _style = style;
            switch (style)
            {
                case Style.Default:
                    {
                        transform.localScale = Vector3.one;
                        _text.font = _defaultFont;
                        _text.color = Color.white;
                        var t = transform.localPosition;
                        t.z = 0;
                        transform.localPosition = t;
                        _onDefault.Invoke();
                        break;
                    }
                case Style.Hightlighted:
                    {
                        transform.localScale = Vector3.one * SCALE_FACTOR;
                        _text.font = _highlightedFont;
                        _text.color = Color.white;
                        var t = transform.localPosition;
                        t.z = _zHighlighted;
                        transform.localPosition = t;
                        _onHighlighted.Invoke();
                        break;
                    }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Parent.FocusOn(this);
        }

    }
}