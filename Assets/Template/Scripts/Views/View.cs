using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Assets
{
    public abstract class View : MonoBehaviour
    {
        public abstract class InitPack
        {
        }

        public Animator _animator;
        private bool _hiding;
        private bool _showing;
        private Action _callback;

        public bool InTransition { get { return _showing || _hiding; } }

        protected event Action<bool> _onVisibilityChanged;

        protected virtual void OnEnable()
        {
            _showing = true;
            _hiding = false;
        }
        
        public virtual void BeginTransition(bool show, Action onEnd)
        {
            if (_showing && show || _hiding && !show)
            {
                Debug.LogWarning("will not interrupt the transition - end goals match (show=" + show + ")");
                return;
            }
            if (_showing || _hiding)
            {
                Debug.LogWarning("interrupting transition");
            }

            if(show)
                gameObject.SetActive(true);
            
            _showing = show;
            _hiding = !show;

            _callback = onEnd;

            _animator.SetBool("Show", show); 
        }

        public void OnTransitionEnd()
        {
            if(!_showing && !_hiding)
            {
                Debug.LogWarning("wait a sec, what was the transition again?");
                return;
            }
            if (_callback != null)
            {
                _callback();
            }
            _callback = null;

            var e = _onVisibilityChanged;
            if (e != null)
                e.Invoke(_showing);

            if(_hiding)
                gameObject.SetActive(false);

            _showing = _hiding = false;
        }

        public abstract void InitWithModel(InitPack model);
    }
}