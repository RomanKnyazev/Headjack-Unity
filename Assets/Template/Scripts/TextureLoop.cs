using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Assets
{
    [RequireComponent(typeof(Renderer))]
	public sealed class TextureLoop : MonoBehaviour 
	{
        public Texture2D[] _textures;
        public float _interval = 0.1f;

        [Range(0, 1f)]
        public float _random = 0.1f;
        public string _textureName = "_MainTex";
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            int id = 0;
            if (_textures.Length == 0)
                yield break;
            while(true)
            {
                id = (id + 1) % _textures.Length;
                _renderer.material.SetTexture(_textureName, _textures[id]);
                yield return new WaitForSeconds(_interval * Random.Range(1 - _random, 1 + _random));
            }
        }
    }
}