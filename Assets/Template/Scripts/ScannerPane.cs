using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace Assets
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class ScannerPane : MonoBehaviour
    {
        [Range(1, 100)]
        public int _segments = 10;
        [Range(0, Mathf.PI)]
        public float _angle = Mathf.PI / 2;

        public float _length = 1;

        public string _textureName = "_MainTex";

        public bool _enableTextureScroll, _textureTile;
        [Range(0, 10f)]
        public float _textureVScrollSpeed = 1, _textureUpdateInterval = 0.1f;

        public LineRenderer _line1, _line2;

        private Mesh _mesh;
        private Material _material;
        private float _offset;

        private Vector3[] _verts;
        private Vector2[] _uvs;
        private int[] _indices;

        private void Start()
        {
            _mesh = new Mesh();
            _mesh.vertices = _verts = new Vector3[_segments * 3];
            _indices = new int[_segments * 3];
            _mesh.uv = _uvs = new Vector2[_segments * 3];

            for (int i = 0; i < _segments * 3; i++)
                _indices[i] = i;
            _mesh.triangles = _indices;

            GetComponent<MeshFilter>().mesh = _mesh;
            _material = GetComponent<Renderer>().material;

            if (_enableTextureScroll)
                StartCoroutine(UpdateTextureOffset());
        }

        private IEnumerator UpdateTextureOffset()
        {
            while (true)
            {
                _offset += _textureVScrollSpeed * Time.deltaTime;
                _offset %= 1;
                _material.SetTextureOffset(_textureName, new Vector2(0, _offset));
                yield return new WaitForSeconds(_textureUpdateInterval);
            }
        }

        private void Update()
        {
            RegenerateMesh();
            UpdateLines();
        }

        private void UpdateLines()
        {
            var xShift = Mathf.Sin(_angle / 2) * _length;
            _line1.SetPosition(1, new Vector3(xShift, 0, _length));
            _line2.SetPosition(1, new Vector3(-xShift, 0, _length));
        }

        private void RegenerateMesh()
        {
            var xShift = Mathf.Sin(_angle / 2) * _length;
            var zShift = _length;

            var positionSegmentWidth = 2 * xShift / _segments;
            for (int i = 0; i < _segments; i++)
            {
                var xOffset = 2 * xShift * (i - _segments / 2) / _segments;
                if (_segments % 2 == 0)
                    xOffset += positionSegmentWidth / 2;
                _verts[i * 3 + 1] = new Vector3(xOffset + positionSegmentWidth / 2, 0, zShift);
                _verts[i * 3 + 2] = new Vector3(xOffset - positionSegmentWidth / 2, 0, zShift);
            }

            var uvSegmentWidth = 1.0f / _segments;
            var len = _textureTile ? _length : 1;
            for (int i = 0; i < _segments; i++)
            {
                var xOffset = uvSegmentWidth * i;
                _uvs[i * 3] = new Vector2(xOffset + uvSegmentWidth / 2, 0);
                _uvs[i * 3 + 1] = new Vector2(xOffset, len);
                _uvs[i * 3 + 2] = new Vector2(xOffset + uvSegmentWidth, len);
            }

            _mesh.uv = _uvs;
            _mesh.vertices = _verts;
            _mesh.RecalculateBounds();
        }
    }
}