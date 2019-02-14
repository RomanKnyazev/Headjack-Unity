using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
	public sealed class CurvedCanvasSettings : MonoBehaviour 
	{
        public bool _enabled = true;
        public int _tesselation;
        public float _radius;
        public float _tesselationThreshold;
        public bool _canvasSpace;
    }
}