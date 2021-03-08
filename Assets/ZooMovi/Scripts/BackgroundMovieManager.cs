using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BackgroundMovieManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private VideoPlayer _videoPlayer;
    
    [SerializeField] private Text MoviePlaySpeedText;

    [SerializeField] private float MoviePlaySpeed = 1.0f;

    void Start()
    {
        //_videoPlayer.skipOnDrop = false;
        // VideoPlayerコンポーネント取得.
        //_videoPlayer = obj.GetComponent<VideoPlayer>();
        // 即再生されるのを防ぐ.
        _videoPlayer.playOnAwake = false;
        // パス or VideoClip を設定.
        //_videoPlayer.url = "Assets/Resources/testfile.mp4";
        // videoPlayer.clip = videoClip;
        // 読込完了時のコールバックを設定.
        _videoPlayer.prepareCompleted += OnCompletePrepare;

        // 読込開始.
        _videoPlayer.Prepare();
    }

    // 読込完了時のコールバック.
    void OnCompletePrepare(VideoPlayer source)
    {
        _videoPlayer.skipOnDrop = false;
        Debug.Log("skipOnDrop = " + source.skipOnDrop);
        Debug.Log("canSetSkipOnDrop = " + source.canSetSkipOnDrop);
        Debug.Log("isPrepared = " + source.isPrepared);
        // 読込が完了したら再生.
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Updateで毎フレーム値を書き込むことは良くないのですが、処理はそこまで重くならないことと、動画読み込み完了時に値を設定してもうまく設定されていなかったために、このように設定しています。
        _videoPlayer.skipOnDrop = false;
        
        if (_videoPlayer.canSetSkipOnDrop == true)
        {
            Debug.Log("a");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _videoPlayer.Prepare();

            _videoPlayer.skipOnDrop = false;
            Debug.Log("skipOnDrop = " + _videoPlayer.skipOnDrop);
            Debug.Log("canSetSkipOnDrop = " + _videoPlayer.canSetSkipOnDrop);
            Debug.Log("isPrepared = " + _videoPlayer.isPrepared);
            Debug.Log("playbackSpeed = " + _videoPlayer.playbackSpeed);
        }
    }

    public void ChangedDropdownMovie()
    {
        
        switch (_dropdown.value)
        {
            case 0:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/river.mp4";
                _videoPlayer.Play();
                break;
            case 1:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/ice.mp4";
                _videoPlayer.Play();
                break;
            case 2:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/lake.mp4";
                _videoPlayer.Play();
                break;
            case 3:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Electronic1.mp4";
                _videoPlayer.Play();
                break;            
            case 4:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Electronic2.mp4";
                _videoPlayer.Play();
                break;            
            case 5:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Electronic3.mp4";
                _videoPlayer.Play();
                break;            
            case 6:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Electronic4.mp4";
                _videoPlayer.Play();
                break;            
            case 7:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Electronic5.mp4";
                _videoPlayer.Play();
                break;            
            case 8:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Forest1.mp4";
                _videoPlayer.Play();
                break;            
            case 9:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Forest2.mp4";
                _videoPlayer.Play();
                break;            
            case 10:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Forest3.mp4";
                _videoPlayer.Play();
                break;            
            case 11:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean1.mp4";
                _videoPlayer.Play();
                break;            
            case 12:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean2.mp4";
                _videoPlayer.Play();
                break;            
            case 13:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean3.mp4";
                _videoPlayer.Play();
                break;            
            case 14:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean4.mp4";
                _videoPlayer.Play();
                break;            
            case 15:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean5.mp4";
                _videoPlayer.Play();
                break;            
            case 16:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean6.mp4";
                _videoPlayer.Play();
                break;            
            case 17:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean7.mp4";
                _videoPlayer.Play();
                break;            
            case 18:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean8.mp4";
                _videoPlayer.Play();
                break;            
            case 19:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Ocean9.mp4";
                _videoPlayer.Play();
                break;            
            case 20:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Sky1.mp4";
                _videoPlayer.Play();
                break;            
            case 21:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Snow1.mp4";
                _videoPlayer.Play();
                break;            
            case 22:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Snow2.mp4";
                _videoPlayer.Play();
                break;            
            case 23:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Snow3.mp4";
                _videoPlayer.Play();
                break;            
            case 24:
                _videoPlayer.url = "https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/Movies/Space1.mp4";
                _videoPlayer.Play();
                break;            
            default:
                break;
        }
    }

    public void ChangedMoviePlaySpeed()
    {
        string MoviePlaySpeedString = MoviePlaySpeedText.text;
        //tmpAnimationSpeedRateStringの終端文字にゴミが入っているので取り除いています(不要となったようです)
        // MoviePlaySpeedString =
        //     MoviePlaySpeedString.Substring(0, MoviePlaySpeedString.Length - 1);
        if (float.TryParse(MoviePlaySpeedString, out MoviePlaySpeed))
        {
            MoviePlaySpeed = Mathf.Clamp(MoviePlaySpeed, 1.0f, 10f)*0.1f;
        }
        else
        {
            MoviePlaySpeed = 0.1f;
        }
        _videoPlayer.playbackSpeed = MoviePlaySpeed;
    }
}