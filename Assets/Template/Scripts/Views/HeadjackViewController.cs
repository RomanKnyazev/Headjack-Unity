using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Headjack;

namespace Assets
{
	public sealed class HeadjackViewController : ViewController
    {
        public static HeadjackViewController Instance { get; private set; }

        public HeadjackViewController()
        {
            Instance = this;
        }
        
        public MessageBoxView _mbViewPrefab;
        public CategoriesView _catViewPrefab;
        public VideosView _videosViewPrefab;
        public DetailsView _detailsViewPrefab;
        public PlaybackView _playbackViewPrefab;

        public void OnHeadjackInitialized(HeadjackInitializer.InitializationEventArgs e)
        {
            if (e.Status != HeadjackInitializer.InitializationEventArgs.StatusCode.Success)
                ShowMessage(e.Message);
            else
                ShowCategories();
        }

        public void ShowMessage(string message, string buttonText = "OK", Action onResult = null)
        {
            var mb = Instantiate(_mbViewPrefab, transform, false);
            mb.InitWithModel(new MessageBoxView.MessageBoxModel { _body = message, _buttonText = buttonText, _onResult = onResult });
            SetActiveView(mb);
        }
        
        public void ShowCategories()
        {
            LogUtil.Log(LogUtil.Severity.Info, this, "Showing cats");
            var cats = Instantiate(_catViewPrefab, transform, false);
            cats.InitWithModel(new CategoriesView.CategoriesModel { _categories = App.GetCategories().Select(s => App.GetCategoryMetadata(s)) });
			if (App.GetCategories () == null)
				ShowVideos (null);
			else
            	SetActiveView(cats);
        }

        public void ShowVideos(string category)
        {
            LogUtil.Log(LogUtil.Severity.Info, this, "Showing vids");
            var vids = Instantiate(_videosViewPrefab, transform, false);
            vids.InitWithModel(new VideosView.VideosModel { _category = App.GetCategoryMetadata(category) });
            SetActiveView(vids);
        }

        public void ShowVideoDetails(string project)
        {
            LogUtil.Log(LogUtil.Severity.Info, this, "Showing dets");
            var vid = Instantiate(_detailsViewPrefab, transform, false);
            vid.InitWithModel(new DetailsView.DetailsModel { _project = App.GetProjectMetadata(project) });
            SetActiveView(vid);
        }

        public void ShowPlayback(App.ProjectMetadata project, bool stream)
        {
            Fader.Instance.FadeOutIn(() =>
            {
                LogUtil.Log(LogUtil.Severity.Info, this, "Showing play");
                var playback = Instantiate(_playbackViewPrefab, transform, false);
                playback.InitWithModel(new PlaybackView.PlaybackModel
                {
                    _currentCategory = App.GetCategoryMetadata(project.Category),
                    _currentProject = project,
                    _stream = stream
                });
                SetActiveView(playback);
            }, null);
        }
	}
}