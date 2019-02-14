using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Headjack;
using System;
using UnityEngine.UI;

namespace Assets
{
    public sealed class PlaybackView : View
    {
        public class PlaybackModel : InitPack
        {
            public App.ProjectMetadata _currentProject;
            public App.CategoryMetadata _currentCategory;
            public bool _stream;
        }

        public PlaybackModel _model;

        public CanvasGroup _contentCanvasGroup;
        public Button _nextButton, _prevButton;
        public Slider _progressSlider;
        public Text _currentTime, _totalTime;

        public float _playerScale = 2;
        private bool _backing;
        private float _maxDisplayTime = 3, _displayTime;
        private bool _sliderReady;

        protected override void OnEnable()
        {
            base.OnEnable();
            App.EnableOVRPlatformMenu = false;
            Scene.Instance.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            App.EnableOVRPlatformMenu = true;
        }

        public void Start()
        {
            App.Play(_model._currentProject.Id, _model._stream, false, (s, t) => App.DestroyVideoPlayer());
            App.Player.transform.localScale = Vector3.one * _playerScale;

            var projects = App.GetProjects(_model._currentCategory.Id);
            var id = Array.IndexOf(projects, _model._currentProject.Id);
            UpdateNavButtonsStatus(id, projects.Length);
        }

        public void PlayPause()
        {
            App.Player.PauseResume();
        }

        public void AddIndex(int i)
        {
            var projects = App.GetProjects(_model._currentCategory.Id);
            var id = Array.IndexOf(projects, _model._currentProject.Id);
            if (id == -1)
            {
                Debug.LogWarning("cannot find this project in category");
                return;
            }
            else
            {
                var newId = Mathf.Clamp(id + i, 0, projects.Length);
                UpdateNavButtonsStatus(newId, projects.Length);

                if (App.GotFiles(projects[newId]))
                {
                    MusicManager.Instance.PlaySceneSound(SoundTypes.OnNextPage);
                    if (App.Player != null)
                        App.Player.Stop();
                    HeadjackViewController.Instance.ShowPlayback(App.GetProjectMetadata(projects[newId]), false);
                }
                else
                {
                    if (App.Player != null)
                        App.DestroyVideoPlayer();
                    HeadjackViewController.Instance.ShowVideoDetails(projects[newId]);
                }
            }
        }

        private void UpdateNavButtonsStatus(int id, int length)
        {
            if (id == 0 && length == 1)
            {
                _nextButton.interactable = false;
                _prevButton.interactable = false;
            }
            else if (id == 0)
            {
                _nextButton.interactable = true;
                _prevButton.interactable = false;
            }
            else if (id == length - 1)
            {
                _nextButton.interactable = false;
                _prevButton.interactable = true;
            }
            else
            {
                _nextButton.interactable = true;
                _prevButton.interactable = true;
            }
        }

        public void MuteUnmute()
        {
            App.Player.Mute = !App.Player.Mute;
        }

        private void Update()
        {
            if (VRInput.Back.Pressed && !_backing)
                Back();
            if (_backing)
                return;

            UpdateVisibility();

            if (App.Player)
            {
                if (!float.IsNaN(App.Player.Seek))
                    _progressSlider.value = App.Player.Seek;
                var span = TimeSpan.FromMilliseconds(App.Player.SeekMs);
                _currentTime.text = string.Format("{0}:{1}:{2}", span.Hours.ToString("00"), span.Minutes.ToString("00"), span.Seconds.ToString("00"));
            }
            else
            {
                Back();
            }
        }

        private void UpdateVisibility()
        {
            if (Input.GetMouseButton(0) || (App.CrosshairHit.collider != null && _contentCanvasGroup.interactable))
                _displayTime = _maxDisplayTime;

            _displayTime -= Time.unscaledDeltaTime;
            _displayTime = Mathf.Max(_displayTime, 0);
            if (_displayTime < _maxDisplayTime / 2)
                _contentCanvasGroup.alpha = _displayTime / _maxDisplayTime * 2;
            else
                _contentCanvasGroup.alpha = 1;

            _contentCanvasGroup.interactable = _contentCanvasGroup.blocksRaycasts = _displayTime > 0;
        }

        public void Back()
        {
            if (App.Player != null)
            {
                App.DestroyVideoPlayer();
            }
            _backing = true;
            Scene.Instance.gameObject.SetActive(true);
            HeadjackViewController.Instance.ShowVideos(_model._currentCategory.Id);
        }

        public override void InitWithModel(InitPack model)
        {
            if (model == null)
                throw new ArgumentNullException("model cannot be null");
            var m = model as PlaybackModel;
            if (m == null)
                throw new ArgumentException(GetType().ToString() + " cannot be initted with model of type " + model.GetType().ToString());
            _model = m;

            _totalTime.text = App.GetVideoMetadata(_model._currentProject.VideoId).DurationHHMMSS;
        }
    }

}
