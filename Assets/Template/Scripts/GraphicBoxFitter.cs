using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    [RequireComponent(typeof(RectTransform))]
	public sealed class GraphicBoxFitter : MonoBehaviour 
	{
        private readonly float _colliderDepth = 0.01f;
        private BoxCollider _collider;

        private void OnEnable()
        {
            var rectTranform = this.GetComponent<RectTransform>();
            _collider = this.GetComponent<BoxCollider>();

            if (!_collider)
            {
                _collider = this.gameObject.AddComponent<BoxCollider>();
                _collider.size = new Vector3(rectTranform.rect.width,
                                             rectTranform.rect.height,
                                             _colliderDepth);
            }
        }
    }
}