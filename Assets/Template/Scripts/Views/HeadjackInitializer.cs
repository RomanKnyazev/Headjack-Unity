using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;
using Headjack;

namespace Assets
{
    public sealed class HeadjackInitializer : MonoBehaviour
    {
        public Transform _cameraAnchor;
        public Material _skyboxMaterial;


        [Serializable]
        public class InitializationEventArgs : EventArgs
        {
            public enum StatusCode
            {
                Success, Empty, Error
            }

            public StatusCode Status { get; private set; }
            public string Message { get; private set; }

            public InitializationEventArgs(StatusCode status, string message)
            {
                Status = status;
                Message = message;
            }
        }

        [Serializable]
        public class InitializationEvent : UnityEvent<InitializationEventArgs> { }

        public InitializationEvent _onInitFinished;

        private void Awake()
        {
            App.Initialize(OnInitialize, true, true);
            App.CameraParent.transform.position = _cameraAnchor.transform.position;
            App.CameraParent.transform.rotation = _cameraAnchor.transform.rotation;

            _onInitFinished.AddListener((s) =>
                LogUtil.Log(
                    s.Status == InitializationEventArgs.StatusCode.Success ? LogUtil.Severity.Info : LogUtil.Severity.Error, this, s.Message
                )
            );
        }

        private void OnInitialize(bool success, string error)
        {
            App.SetCamera();
            _skyboxMaterial = Instantiate(_skyboxMaterial);
            _skyboxMaterial.SetColor("_Tint", App.GetColor("Primary Tint", Color.white / 2));
            App.SetCameraBackground(_skyboxMaterial);
            App.ShowCrosshair = true;

            if (success)
                HandleSuccess(error);
            else
                HandleFailure(error);
        }

        private void HandleFailure(string error)
        {
            var invokationArgs = new InitializationEventArgs(InitializationEventArgs.StatusCode.Error, error);
            _onInitFinished.Invoke(invokationArgs);
        }

        private void HandleSuccess(string error)
        {
            if (App.GetProjects().Length == 0)
            {
                var invokationArgs = new InitializationEventArgs(InitializationEventArgs.StatusCode.Empty, "THERE ARE NO PUBLISHED PROJECTS");
                _onInitFinished.Invoke(invokationArgs);
            }
            else if (!string.IsNullOrEmpty(error))
            {
                if (error.Equals("Offline", StringComparison.InvariantCultureIgnoreCase))
                {
                    var invokationArgs = new InitializationEventArgs(InitializationEventArgs.StatusCode.Success, "LAUNCHING IN OFFLINE MODE");
                    _onInitFinished.Invoke(invokationArgs);
                }
                else
                    throw new InvalidOperationException("Success with an unknown error message?");           
            }
            else
                App.DownloadAllTextures(OnTexturesLoaded);            
        }


        private void OnTexturesLoaded(bool success, string error)
        {
            InitializationEventArgs invokationArgs;
            if (success)
                invokationArgs = new InitializationEventArgs(InitializationEventArgs.StatusCode.Success, string.Empty);
            else
                invokationArgs = new InitializationEventArgs(InitializationEventArgs.StatusCode.Error, error);
            _onInitFinished.Invoke(invokationArgs);
        }
    }
}