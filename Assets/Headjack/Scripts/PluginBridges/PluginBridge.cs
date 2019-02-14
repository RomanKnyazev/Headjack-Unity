#if HEADJACK_GOT_PLUGINS
using UnityEngine;
using UnityEngine.Events;
using HTC.UnityPlugin.Multimedia;
namespace Headjack.Plugins
{
#if HEADJACK_EMT
    public class MediaPlayerCtrlBridge : IMediaPlayerCtrl {
        private MediaPlayerCtrl emt;
        private GameObject _gameObject;
        public MediaPlayerCtrlBridge(GameObject G){
            _gameObject = G;
            emt = _gameObject.AddComponent<MediaPlayerCtrl>();
        }
        public GameObject gameobject
        {
            get{
                return _gameObject;
            }
        }

        public GameObject[] m_TargetMaterial
        { 
            get
            {
                return emt.m_TargetMaterial;
            }
            set 
            {
                emt.m_TargetMaterial = value;
            }
        }
		public bool m_bLoop
		{ 
			get
			{
				return emt.m_bLoop;
			}
			set 
			{
				emt.m_bLoop = value;
			}
		}
        public GameObject gameObject
        { 
            get
            {
                return _gameObject;
            }
        }

        VideoReady _onReady;
        public VideoReady OnReady { 
            get
            {
                return _onReady;
            }
            set
            {
                _onReady = value;
                emt.OnReady = delegate()
                {
                        _onReady();
                };
            }
        }
        VideoFirstFrameReady _onVideoFirstFrameReady;
        public VideoFirstFrameReady OnVideoFirstFrameReady{
            get
            {
                return _onVideoFirstFrameReady;
            }
            set
            {
                _onVideoFirstFrameReady = value;
                emt.OnVideoFirstFrameReady = delegate ()
                {
                        _onVideoFirstFrameReady();
                };
            }
        }

        VideoEnd _onEnd;
        public VideoEnd OnEnd{ 
            get
            {
                return _onEnd;
            }
            set
            {
                _onEnd = value;
                emt.OnEnd = delegate()
                {
                        _onEnd();
                };
            }
        } 
        public MEDIAPLAYER_STATE GetCurrentState()
        {
            return (MEDIAPLAYER_STATE)emt.GetCurrentState();
        }
        public void SetVolume(float fVolume)
        {
            emt.SetVolume(fVolume);
        }
        public void Load(string strFileName)
        {
            emt.Load(strFileName);
        }
        public void UnLoad()
        {
            emt.UnLoad();
        }
        public void Play()
        {
            emt.Play();
        }
        public int GetSeekPosition()
        {
            return emt.GetSeekPosition();
        }
        public void SeekTo(int iSeek)
        {
            emt.SeekTo(iSeek);
        }
        public int GetDuration()
        {
            return emt.GetDuration();
        }
        public void Pause()
        {
            emt.Pause();
        }
        public void Stop()
        {
            emt.Stop();
        }
    }
#endif

    public class MediaDecoderBridge : IMediaDecoder {
        private MediaDecoder vmd;
        private GameObject _gameObject;
        public MediaDecoderBridge(GameObject G) {
            _gameObject = G;
            vmd=_gameObject.AddComponent<MediaDecoder>();
        }
 
        public GameObject gameObject
        { 
            get
            {
                return _gameObject;
            }
        }

        //IMediaDecoder getInstance();
        public void initDecoder(string path){
            vmd.initDecoder(path);
        }
        public void startDecoding(){
            vmd.startDecoding();
        }
        public void stopDecoding(){
            vmd.stopDecoding();
        }
        public void setSeekTime(float seekTime){
            vmd.setSeekTime(seekTime);
        }
        public void setVolume(float vol){
            vmd.setVolume(vol);
        }
        public void mute()
        {
            vmd.mute();
        }
        public void unmute(){
            vmd.unmute();
        }
		public void replay(){
			vmd.replay ();
		}
        public void setPause(){
            vmd.setPause();
        }
        public void setResume()
        {
            vmd.setResume();
        }
        public bool playOnAwake{ 
            get
            {
                return vmd.playOnAwake;
            }
            set
            {
                vmd.playOnAwake = value;
            }
        }
        public float getVideoCurrentTime(){
            return vmd.getVideoCurrentTime();
        }
        public float videoTotalTime{ 
            get
            { 
                return vmd.videoTotalTime;
            }
        }
        public string mediaPath
        { 
            get
            {
                return vmd.mediaPath;
            }
            set
            {
                vmd.mediaPath = value;
            }
        }
        public UnityEvent onInitComplete
        {
            get
            {
                return vmd.onInitComplete;
            }
            set
            {
                vmd.onInitComplete = value;
            }
        }
        public UnityEvent onVideoEnd
        {
            get
            {
                return vmd.onVideoEnd;
            }
            set
            {
                vmd.onVideoEnd = value;
            }
        }
        public DecoderState getDecoderState()
        {
            return (DecoderState)vmd.getDecoderState();
        }
    }
}
#endif