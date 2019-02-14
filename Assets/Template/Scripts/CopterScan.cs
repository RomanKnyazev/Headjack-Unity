using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Assets
{
    [RequireComponent(typeof(Animator))]
    public sealed class CopterScan : MonoBehaviour
    {
        public UnityEvent _onSequenceEnd;
        private Animator _animator;
        private bool _ready;

        public void Ready()
        {
            _animator.SetBool("Ready", _ready = true);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animator.SetBool("ReadySecondTime", _ready);
        }

        public void SequenceEnd()
        {
            _onSequenceEnd.Invoke();
        }
    }
}