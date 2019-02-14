using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Headjack;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets
{
    public abstract class ImageView : View
    {
        public ImageViewItem _prefab;
        public Transform _content;

        private int _selectedItem;
        [Range(0, 1f)]
        public float _lerpPower = 0.1f;
        public float _r = 1000f;
        private List<ImageViewItem> _items = new List<ImageViewItem>();

        [Range(1, 2f)]
        public float _scaleFactor = 1.1f;
        [Range(0, 90f)]
        public float _rotationMaxAngle = 30f;

        public event Action<ImageViewItem> OnItemAdded;

        public int SelectedItemIndex
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                var v = Mathf.Clamp(value, 0, _content.childCount - 1);
                if (_selectedItem != v)
                    foreach (var i in _items)
                    {
                        i.ApplyStyle(ImageViewItem.Style.Default);
                    }
                _selectedItem = v;
            }
        }

        public int Count { get { return _items.Count; } }

        public List<ImageViewItem> Items { get { return _items; } }

        public void Clear()
        {
            _items.Clear();
            foreach (Transform t in _content)
            {
                Destroy(t.gameObject);
            }
        }

        public void FocusOn(ImageViewItem child)
        {
            SelectedItemIndex = _items.IndexOf(child);
        }

        protected virtual void Update()
        {
            if (_items.Count == 0)
            {
                return;
            }
            UpdatePositioning();
            UpdateBend();
        }

        private void UpdatePositioning()
        {
            var t = _content as RectTransform;
            var i = _items[SelectedItemIndex].transform as RectTransform;
            var targetX = -i.anchoredPosition.x;
            t.anchoredPosition = new Vector2(Mathf.Lerp(t.anchoredPosition.x, targetX, _lerpPower), t.anchoredPosition.y);
            if (Mathf.Abs(t.anchoredPosition.x - targetX) < i.rect.width / 2)
            {
                _items[SelectedItemIndex].ApplyStyle(ImageViewItem.Style.Hightlighted);
            }
        }

        private void UpdateBend()
        {
            foreach (Transform i in _content)
            {
                var p = transform.InverseTransformPoint(i.position);
                var loc = i.localPosition;
                loc.z = 0;
                i.localPosition = loc;
                //i.localRotation = Quaternion.AngleAxis(p.x / _r * _rotationMaxAngle, Vector3.up);
            }
        }

        public ImageViewItem AddItem(Texture2D tex, string text, UnityAction onClick)
        {
            var o = Instantiate(_prefab, _content, false);
            _items.Add(o);
            o.Parent = this;
            o._image.texture = tex;
            o._text.text = text;
            o._onSelected.AddListener(onClick);
            var e = OnItemAdded;
            if (e != null)
            {
                e(o);
            }
            return o;
        }

        public void ClickCurrent()
        {
            if (_items.Count != 0)
            {
                _items[SelectedItemIndex].Clicked();
            }
        }

        public void AddToIndex(int v)
        {
            SelectedItemIndex += v;
        }

    }
}