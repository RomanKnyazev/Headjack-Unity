using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace Assets
{
    public sealed class Fader : MonoBehaviour
    {
        private enum State
        {
            FadedIn,
            FadedOut,

            FadingIn,
            FadingOut,
            FadingOutIn
        }

        public Material _skybox;
        public string _skyboxColorVar = "_Tint";

        [Range(0, 0.1f)]
        public float _maxFog, _initialVisiblity = 0;
        public float _duration = 1;

        private State CurrentState
        {
            get { return _state; }
            set
            {
                _state = value;
                LogUtil.Log(LogUtil.Severity.Info, this, "new state - " + value);
            }
        }
        private Color _skyColor;
        private Color _fogColor;
        private float _fogDensity;
        private Action _onFadeOut, _onFadeIn;

        private float _fadeTime;
        private bool _fadeoutReached;
        private State _state;

        public static Fader Instance { get; private set; }

        public Fader()
        {
            Instance = this;
        }

        public bool FadeOutIn(Action onFadedOut, Action onFadedIn)
        {
            if (CurrentState != State.FadedIn)
                return false;
            CurrentState = State.FadingOutIn;
            _fadeTime = _duration;
            _fadeoutReached = false;
            _onFadeOut = onFadedOut;
            _onFadeIn = onFadedIn;
            return true;
        }

        public void FadeOutIn()
        {
            FadeOutIn(null, null);
        }

        public bool FadeOut(Action onComplete)
        {
            if (CurrentState != State.FadedIn)
                return false;
            CurrentState = State.FadingOut;
            _fadeTime = _duration;
            _onFadeOut = onComplete;
            return true;
        }

        public void FadeOut()
        {
            FadeOut(null);
        }

        public bool FadeIn(Action onComplete)
        {
            if (CurrentState != State.FadedOut)
                return false;
            CurrentState = State.FadingIn;
            _onFadeIn = onComplete;
            _fadeTime = 0;
            return true;
        }

        public void FadeIn()
        {
            FadeIn(null);
        }

        private void Start()
        {
            _skyColor = _skybox.GetColor(_skyboxColorVar);
            _fogColor = RenderSettings.fogColor;
            _fogDensity = RenderSettings.fogDensity;

            if (_initialVisiblity < 1)
                CurrentState = State.FadedOut;
            UpdateParams(_initialVisiblity);
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case State.FadedIn:
                case State.FadedOut:
                    break;

                case State.FadingIn:
                    if (_fadeTime >= _duration)
                    {
                        UpdateParams(1);
                        CurrentState = State.FadedIn;
                        if (_onFadeOut != null)
                            _onFadeOut();
                    }
                    else
                    {
                        _fadeTime += Time.deltaTime;
                        UpdateParams(_fadeTime / _duration);
                    }
                    break;

                case State.FadingOut:
                    if (_fadeTime <= 0)
                    {
                        UpdateParams(0);
                        CurrentState = State.FadedOut;
                        if (_onFadeOut != null)
                            _onFadeOut();
                    }
                    else
                    {
                        _fadeTime -= Time.deltaTime;
                        UpdateParams(_fadeTime / _duration);
                    }
                    break;

                case State.FadingOutIn:
                    if (_fadeoutReached)
                    {
                        if (_fadeTime >= _duration)
                        {
                            UpdateParams(1);
                            CurrentState = State.FadedIn;
                            if (_onFadeIn != null)
                                _onFadeIn();
                            break;
                        }
                        else
                            _fadeTime += Time.deltaTime;
                    }
                    else
                    {
                        if (_fadeTime <= 0)
                        {
                            UpdateParams(0);
                            _fadeoutReached = true;
                            if (_onFadeOut != null)
                                _onFadeOut();
                            break;
                        }
                        else
                            _fadeTime -= Time.deltaTime;
                    }
                    UpdateParams(_fadeTime / _duration);
                    break;
            }
        }

        private void UpdateParams(float x)
        {
            _skybox.SetColor(_skyboxColorVar, _skyColor * x);
            RenderSettings.fogDensity = _fogDensity + (1 - x) * _maxFog;
            RenderSettings.fogColor = _fogColor * x;
        }
    }
}