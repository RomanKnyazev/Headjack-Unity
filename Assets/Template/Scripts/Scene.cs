using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
	public sealed class Scene : MonoBehaviour 
	{
        public Scene()
        {
            Instance = this;
        }

        public static Scene Instance { get; private set; }
    }
}