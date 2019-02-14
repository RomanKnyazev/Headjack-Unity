using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

namespace Assets
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class CurvedGraphic : BaseMeshEffect
    {
        private bool _canvasSpace;
        private float _r, _tesselationThreshold;
        private int _tesselation = 0;
        private Graphic _graphic;

        protected override void Awake()
        {
            base.Awake();
            _graphic = GetComponent<Graphic>();
            var settings = _graphic.canvas.GetComponent<CurvedCanvasSettings>();
            if (settings == null)
            {
                Debug.LogWarning("No curved ui settings were found");
                _r = (_graphic.canvas.transform as RectTransform).rect.width;
                _tesselation = 0;
                _tesselationThreshold = 0;
            }
            else
            {
                _r = settings._radius;
                _tesselation = settings._tesselation;
                _tesselationThreshold = settings._tesselationThreshold;
                _canvasSpace = settings._canvasSpace;
                if(!settings._enabled)
                {
                    enabled = false;
                    _graphic.SetVerticesDirty();
                }
            }
        }

        public override void ModifyMesh(Mesh m)
        {
            using (var vh = new VertexHelper(m))
            {
                ModifyMesh(vh);
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            var l = new List<UIVertex>(vh.currentVertCount);
            vh.GetUIVertexStream(l);
            if (l.Count != 0 && (l[0].position - l[1].position).sqrMagnitude > _tesselationThreshold * _tesselationThreshold)
            {
                TesselateVerts(l);
            }
            var transformedPos = _canvasSpace ? _graphic.canvas.transform.InverseTransformPoint(transform.position) : Vector3.zero;
            for (int i = 0; i < l.Count; i++)
            {
                var v = l[i];
                var p = v.position;
                var x = p.x + transformedPos.x;
                p.z += Mathf.Sqrt(Mathf.Abs(_r * _r - x * x)) - _r;
                v.position = p;
                l[i] = v;
            }
            vh.AddUIVertexTriangleStream(l);
        }

        private void TesselateVerts(List<UIVertex> l, int level = 0)
        {
            if (level == _tesselation)
            {
                return;
            }
            var newL = new List<UIVertex>(l.Count * 2);
            for (int i = 0; i < l.Count / 6; i++)
            {
                var v1 = l[i * 6];
                var v2 = l[i * 6 + 1];
                var v3 = l[i * 6 + 2];
                var v4 = l[i * 6 + 4];
                var v1d = LerpUIVertex(v2, v3, 0.5f);
                var v2d = LerpUIVertex(v4, v1, 0.5f);
                UIVertex[] newTris =
                {
                    v1, v2, v1d, v1d, v2d, v1,
                    v2d, v1d, v3, v3, v4, v2d
                };
                newL.AddRange(newTris);
            }
            l.Clear();
            l.AddRange(newL);
            TesselateVerts(l, level + 1);
        }

        private static UIVertex LerpUIVertex(UIVertex v1, UIVertex v2, float f)
        {
            var result = new UIVertex();
            result.color = Color.Lerp(v1.color, v2.color, f);
            result.position = Vector3.Lerp(v1.position, v2.position, f);
            result.normal = Vector3.Lerp(v1.normal, v2.normal, f).normalized;
            result.tangent = Vector3.Lerp(v1.tangent, v2.tangent, f).normalized;
            result.uv0 = Vector2.Lerp(v1.uv0, v2.uv0, f);
            result.uv1 = Vector2.Lerp(v1.uv1, v2.uv1, f);
            return result;
        }
    }
}