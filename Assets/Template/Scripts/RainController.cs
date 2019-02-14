using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public sealed class RainController : MonoBehaviour
    {
        /// <summary>
        /// Grain is a triangle fan.
        /// </summary>
        private const int GRAINS_SIDES = 5;

        [System.Serializable]
        private struct MeshInstance
        {
            public Mesh _mesh;
            public Vector3 _position;
            public float _speed;
            public float _lifetime;
        }

        private List<MeshInstance> _activeMeshes = new List<MeshInstance>();

        private Vector4[] _grainVerts;

        [Range(0, 1f)]
        public float _grainSizeVariation = 0.1f;
        public Vector3 _grainSize = new Vector3(0.05f, 0.15f, 0.05f), _meshBounds = Vector3.one * 10;

        [Range(1, 65536 / (GRAINS_SIDES + 1))]
        public int _grains = 1000;

        public Quaternion _rotation = Quaternion.identity;

        [Range(1, 10)]
        public int _meshesCount = 3;
        public Vector3 _bounds = Vector3.one * 100;

        public float _speed = 10, _speedVariation = 0.1f, _meshLifetime = 5;
        public int _meshesPerUpdate = 1;

        public Material _rainMaterial;

        public List<Mesh> _meshes;

        private void OnDrawGizmos()
        {
            var m = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _bounds);
            Gizmos.matrix = m;
        }

        private void Start()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            CreateReferenceGrain();
            for (int i = 0; i < _meshesCount; i++)
                CreateMesh();

            sw.Stop();
            LogUtil.Log(LogUtil.Severity.Info, this, "Initialized in {0} msec", sw.ElapsedMilliseconds);
        }

        private void CreateReferenceGrain()
        {
            _grainVerts = new Vector4[GRAINS_SIDES + 1];
            _grainVerts[0] = new Vector4(0, 0, 0, 1);
            for (int i = 0; i < GRAINS_SIDES; i++)
            {
                var phi = (float)i / GRAINS_SIDES * Mathf.PI * 2 - Mathf.PI / 2;
                _grainVerts[i + 1] = new Vector4(Mathf.Cos(phi), Mathf.Sin(phi), 0, 1);
            }
        }

        private void CreateMesh()
        {
            var mesh = new Mesh() { name = "RainDrops" };
            var verts = new Vector3[(GRAINS_SIDES + 1) * _grains];
            var indices = new int[GRAINS_SIDES * 3 * _grains];

            for (int i = 0; i < _grains; i++)
                AddGrain(i, verts, indices);

            mesh.vertices = verts;
            mesh.triangles = indices;
            _meshes.Add(mesh);
            mesh.RecalculateBounds();
        }

        private void AddGrain(int id, Vector3[] verts, int[] indices)
        {
            var vertOffset = id * (GRAINS_SIDES + 1);
            var indOffset = id * (GRAINS_SIDES * 3);
            var pos = RandomGrainPos();
            var t = Matrix4x4.TRS(pos, Quaternion.identity, _grainSize * Random.Range(1 - _grainSizeVariation, 1 + _grainSizeVariation));
            for (int i = 0; i < GRAINS_SIDES + 1; i++)
                verts[vertOffset + i] = t * _grainVerts[i];

            for (int i = 0; i < GRAINS_SIDES; i++)
            {
                indices[indOffset + i * 3 + 0] = vertOffset;
                indices[indOffset + i * 3 + 1] = vertOffset + 1 + i;
                indices[indOffset + i * 3 + 2] = vertOffset + ((1 + i) % GRAINS_SIDES) + 1;
            }
        }

        private Vector4 RandomGrainPos()
        {
            return new Vector4(
                Random.Range(-_meshBounds.x / 2, _meshBounds.x / 2),
                Random.Range(-_meshBounds.y / 2, _meshBounds.y / 2),
                Random.Range(-_meshBounds.z / 2, _meshBounds.z / 2),
                1
            );
        }

        private Vector3 RandomMeshPos()
        {
            return new Vector3(
                Random.Range(-_bounds.x / 2, _bounds.x / 2),
                _bounds.y / 2,
                Random.Range(-_bounds.z / 2, _bounds.z / 2)
            );
        }

        private void Update()
        {
            _activeMeshes.RemoveAll(s => s._lifetime > _meshLifetime);
            AddMeshes();
            UpdateMeshes();
        }

        private void UpdateMeshes()
        {
            var orientation = _rotation * -Vector3.up;
            for (int i = 0; i < _activeMeshes.Count; i++)
            {
                var m = _activeMeshes[i];
                m._position += orientation * _speed * Time.deltaTime;
                m._lifetime += Time.deltaTime;
                _activeMeshes[i] = m;
            }
        }


        private void AddMeshes()
        {
            for (int i = 0; i < _meshesPerUpdate; i++)
            {
                _activeMeshes.Add(new MeshInstance
                {
                    _mesh = _meshes[Random.Range(0, _meshes.Count)],
                    _position = RandomMeshPos(),
                    _speed = Random.Range(1 - _speedVariation, 1 + _speedVariation) * _speed
                });
            }
        }

        private void OnRenderObject()
        {
            _rainMaterial.SetPass(0);
            for (int i = 0; i < _activeMeshes.Count; i++)
            {
                Debug.DrawLine(transform.position, transform.position + _activeMeshes[i]._position);
                var rotationAroundCenter = LookAtTransformCenter(_activeMeshes[i]._position);
                var matrix = transform.localToWorldMatrix * Matrix4x4.TRS(_activeMeshes[i]._position, _rotation * rotationAroundCenter, Vector3.one);
                Graphics.DrawMeshNow(_activeMeshes[i]._mesh, matrix);
            }
        }

        private Quaternion LookAtTransformCenter(Vector3 p)
        {
            p.y = 0;
            return Quaternion.LookRotation(-p.normalized, Vector3.up);
        }
    }
}