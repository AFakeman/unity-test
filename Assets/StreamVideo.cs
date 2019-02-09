using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Video;


public class StreamVideo : MonoBehaviour {


    public RawImage image; //объект для изображения
    public GameObject playIcon; //кнопка паузы-продолжения

    //public GameObject sliderObject; //Для слайдера
    //private Slider slider;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;
    private AudioSource audioSource;

    private bool isPaused = false;
    private bool firstRun = true;

    //// Use this for initialization(если расскоментировать то тогда видео будет играться сразу при переходе на новую сцену)
    //void Start ()
    //{
    //    Application.runInBackground = true;
    //    StartCoroutine(playVideo());
    //}

    IEnumerator playVideo()
    {
        playIcon.SetActive(false);
        firstRun = false;

        //Добавляем VideoPlayer к gameobject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add audiosource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both video and audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        //непонятная хрень для загрузки видео(чуть позже)
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = MainMenu.path; //bind with MainMenu

        //Set audio output to audiosource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from video to AudioSourceto be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video to play then prepare Audio to prevent buffering
        //videoPlayer.clip = videoToPlay; //можно загружать(для одного предрасположенного видео из assets)
        videoPlayer.Prepare();

        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare wait for 5 seconds only 
            yield return waitTime;
            //Break out after 5 seconds wait    
            break;
        }

        Debug.Log("Done preparing video");

        //Ассоциируем вывод изображения в image
        image.texture = videoPlayer.texture;
        Debug.Log("Ok");

        //slider.maxValue = (float)videoPlayer.clip.length; можно получить максимальную длинну видео но не работает
        
        //Play video
        videoPlayer.Play();

        //play sound
        audioSource.Play();

        Debug.Log("Playing video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time:" + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;

        }
        Debug.Log("Done playing video");
    }

    public void PlayPause()
    {
        if ((!firstRun) && (!isPaused))
        {
            videoPlayer.Pause();
            audioSource.Pause();
            playIcon.SetActive(true);
            isPaused = true;
            Debug.Log("Done playing video");
           
            
        }
        else if (!firstRun && isPaused)
        {
            videoPlayer.Play();
            audioSource.Play();
            playIcon.SetActive(false);
            isPaused = false;
            Debug.Log("Playing video");
            Debug.LogWarning("Video Time:" + Mathf.FloorToInt((float)videoPlayer.time));
        }
        else {
            StartCoroutine(playVideo());
        }
    }

    // Update is called once per frame
    void Update () {


	}
}
