using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Headjack;
using UnityEngine.UI;

namespace Assets
{
    public sealed class DetailsView : View
    {
        public class DetailsModel : InitPack
        {
            public App.ProjectMetadata _project;
        }

        public Text _title, _description, _fileSize;
        public Slider _downloadProgress;
        public Button _playButton, _streamButton, _downloadButton, _cancelButton, _deleteButton;
        public Image _databaseIcon;
        //public GameObject _localFileBlock, _remoteFileBlock;
        public RawImage _texture;

        private DetailsModel _model;

        public void PlayClicked()
        {
            HeadjackViewController.Instance.ShowPlayback(_model._project, false);
        }

        public void StreamClicked()
        {
            HeadjackViewController.Instance.ShowPlayback(_model._project, true);
        }

        public void DownloadClicked()
        {
            _databaseIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            App.Download(_model._project.Id);
        }

        public void CancelClicked()
        {
            App.Cancel(_model._project.Id);
        }

        public void DeleteClicked()
        {
            _databaseIcon.GetComponent<Image>().color = new Color32(75, 75, 75, 75);
            _downloadProgress.value = 0;
            App.Delete(_model._project.Id);
        }

        public void BackClicked()
        {
            HeadjackViewController.Instance.ShowVideos(_model._project.Category);
        }

        private void Awake()
        {
            _downloadProgress.value = App.GotFiles(_model._project.Id) ? 1 : 0;
            
            if (!App.GotFiles(_model._project.Id))
            {
                _databaseIcon.GetComponent<Image>().color = new Color32(75, 75, 75, 75);
            } else
            {
                _databaseIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }

        }
        private void Update()
        {
            var downloaded = App.GotFiles(_model._project.Id);
            //_localFileBlock.gameObject.SetActive(downloaded);
            //_remoteFileBlock.gameObject.SetActive(!downloaded);

            var downloadInProgress = App.ProjectIsDownloading(_model._project.Id);

            if (downloadInProgress)
            {
                _downloadProgress.value = App.GetProjectProgress(_model._project.Id) / 100;
            }
            _streamButton.gameObject.SetActive(!downloaded);
            _playButton.gameObject.SetActive(downloaded);
            _downloadButton.gameObject.SetActive(!downloadInProgress && !downloaded);
            _cancelButton.gameObject.SetActive(downloadInProgress);
            _deleteButton.gameObject.SetActive(downloaded);

        }


        public override void InitWithModel(InitPack model)
        {
            if (model == null)
                throw new ArgumentNullException("model cannot be null");
            var m = model as DetailsModel;
            if (m == null)
                throw new ArgumentException(GetType().ToString() + " cannot be initted with model of type " + model.GetType().ToString());

            _model = m;

            _title.text = _model._project.Title;
            float projectSize = Headjack.App.GetProjectMetadata(_model._project.Id, Headjack.App.ByteConversionType.Megabytes).TotalSize;
			_fileSize.text = System.Math.Round(projectSize, 2) + " mb";
            _texture.texture = App.GetImage(_model._project.ThumbnailId);
        }


    }
}