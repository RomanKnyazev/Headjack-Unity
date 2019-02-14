using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Headjack;
using System;

namespace Assets
{
	public sealed class VideoOverview : View 
	{
        public class VideoModel : InitPack
        {
            public App.ProjectMetadata _project;
        }

        public VideoModel _model;

        public Text _duration;

        public void GotoDetailsClicked()
        {
            HeadjackViewController.Instance.ShowVideoDetails(_model._project.Id);
        }

        public override void InitWithModel(InitPack model)
        {
            if (model == null)
                throw new ArgumentNullException("model cannot be null");
            var m = model as VideoModel;
            if (m == null)
                throw new ArgumentException(GetType().ToString() + " cannot be initted with model of type " + model.GetType().ToString());
            _model = m;
            var videoData = App.GetVideoMetadata(m._project.Id);
            if (videoData.Duration / (3600 * 1000) == 0)
            {
                _duration.text = videoData.DurationMMSS;
            }
            else
            {
                _duration.text = videoData.DurationHHMMSS;
            }
        }
    }
}