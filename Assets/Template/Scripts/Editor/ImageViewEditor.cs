using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Assets
{
    [CustomEditor(typeof(ImageView))]
    public sealed class ImageViewEditor : Editor
    {
        private int _index;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        /*    var t = target as ImageView;
            if (t.Count == 0)
                return;
            var i = EditorGUILayout.IntSlider(_index, 0, t.Count - 1);
            if (i != _index)
            {
                t.SelectedItemIndex = _index = i;
            }*/
        }
    }
}