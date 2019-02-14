using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Headjack;
using UnityEngine.UI;

namespace Assets
{
    public abstract class ViewController : MonoBehaviour
    {
        private View _activeView;

        private bool _busy;

        private Animator _loader;

        public void SetActiveView(View view)
        {
            var currentView = _activeView;
            if (currentView != null)            
               currentView.BeginTransition(false, () => Destroy(currentView.gameObject));
            
            (_activeView = view).BeginTransition(true, null);
        }

        private void BringToFront(View view)
        {
            if (_activeView != null)
            {
                Destroy(_activeView.gameObject);
                if (_activeView.InTransition)
                    LogUtil.Log(LogUtil.Severity.Info, this, "destroying view {0} prematurely", _activeView.gameObject.name);
            }
            (_activeView = view).BeginTransition(true, null);
        }

    }
}