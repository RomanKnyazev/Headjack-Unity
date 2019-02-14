using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
	public sealed class Spin : MonoBehaviour 
	{
        public Vector3 _axis = Vector3.up;
        public float _rpm = 1;
        private float _phi;
        private Quaternion _rot;

        private void Start()
        {
            _rot = transform.localRotation;
        }

		private void FixedUpdate()
		{
            _phi += _rpm * Mathf.PI * 2 * Time.fixedDeltaTime;
            if (Mathf.Abs(_phi) > Mathf.PI * 2)
                _phi %= Mathf.PI * 2;
            transform.localRotation = _rot * Quaternion.AngleAxis(_phi * Mathf.Rad2Deg, _axis);
		}
	}
}