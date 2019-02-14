using UnityEngine;
using System.Collections;
using System;

[Serializable]
public enum SoundTypes
{
	OnClick,
	OnDownloadComplete,
	OnHover,
	OnNextPage
}

public class MusicManager : MonoBehaviour
{
	[Header("Sound Clips:")]
	public AudioClip OnClickClip;
	public AudioClip OnDownloadCompleteClip;
	public AudioClip OnHoverClip;
	public AudioClip OnNextPageClip;

	[Space(10)]
	[Header("MusicSettings:")]
	public bool UseMusic = true;

	AudioSource _musicSource;
	AudioSource _audioSource;

	private static MusicManager _instance;
	public static MusicManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<MusicManager>();
				if (_instance == null)
				{
					Debug.LogError("MusicManager Instance is not available");
				}

			}
			return _instance;
		}
	}

    
	void Awake ()
	{
		_instance = this;
		if (UseMusic)
		{
			_audioSource = GetComponent<AudioSource>();
			if (_musicSource == null)
			{
                _musicSource = gameObject.AddComponent<AudioSource>();
			}
		}
		else
		{
		    _audioSource = GetComponent<AudioSource>();
		}
	}



	public void PlaySceneSound(SoundTypes soundType)
	{
		switch (soundType)
		{
			case SoundTypes.OnClick:
		        _audioSource.clip = OnClickClip;
				break;
			case SoundTypes.OnDownloadComplete:
                _audioSource.clip = OnDownloadCompleteClip;
                break;
			case SoundTypes.OnHover:
                _audioSource.clip = OnHoverClip;
                break;
			case SoundTypes.OnNextPage:
                _audioSource.clip = OnNextPageClip;
                break;
			default:
				break;
		}
        _audioSource.Play();
    }
}
