//OpenCV for UnityのVideoWriterExample.csをZooMoviの作成のために編集したプログラムです。

using System.IO;
using System.Runtime.InteropServices;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.VideoioModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZooMovi.Scripts
{
    /// <summary>
    /// VideoWriter Example
    /// An example of saving a video file using the VideoWriter class.
    /// http://docs.opencv.org/3.2.0/dd/d43/tutorial_py_video_display.html
    /// </summary>
    public class ZooMoviVideoWriterExample : MonoBehaviour
    {
        /// <summary>
        /// The preview panel.
        /// </summary>
        public RawImage previewPanel;

        /// <summary>
        /// The rec button.
        /// </summary>
        public Button recButton;

        /// <summary>
        /// The play button.
        /// </summary>
        public Button playButton;

        /// <summary>
        /// Unityで作成したAVI形式の動画をダウンロードするためのボタンです。
        /// 今回は未使用の状態となっています。
        /// </summary>
        public Button saveAviMovieButton;
        
        /// <summary>
        /// Unityで作成したAVI形式の動画をFFMPEGでMP4形式に変換してダウンロードするためのボタンです。
        /// </summary>
        public Button saveMp4MovieButton;

        /// <summary>
        /// The max frame count.
        /// </summary>
        [Header("保存する動画のフレーム数です。フレーム数が多いとWebGLのアプリが落ちてしまいます。MacBook Pro 2019では1280*720の動画は80秒(80*30FPS=2400フレーム)保存できることを確認しました。2560*1440の動画は20秒(20*30FPS=600フレーム)保存できることを確認しました。")]
        [SerializeField] private int maxFrameCount = 120;

        [Header("保存する動画のフレーム数です。フレーム数が多いとWebGLのアプリが落ちてしまいます。MacBook Pro 2019では1280*720の動画は80秒(80*30FPS=2400フレーム)保存できることを確認しました。2560*1440の動画は20秒(20*30FPS=600フレーム)保存できることを確認しました。")]
        [SerializeField] private int movieFps = 120;

        /// <summary>
        /// The frame count.
        /// </summary>
        int frameCount;

        /// <summary>
        /// The videowriter.
        /// </summary>
        VideoWriter writer;

        /// <summary>
        /// The videocapture.
        /// </summary>
        VideoCapture capture;

        /// <summary>
        /// The screen capture.
        /// </summary>
        Texture2D screenCapture;

        /// <summary>
        /// The recording frame rgb mat.
        /// </summary>
        Mat recordingFrameRgbMat;

        /// <summary>
        /// The preview rgb mat.
        /// </summary>
        Mat previewRgbMat;

        /// <summary>
        /// The preview texture.
        /// </summary>
        Texture2D previrwTexture;

        /// <summary>
        /// Indicates whether videowriter is recording.
        /// </summary>
        bool isRecording;

        /// <summary>
        /// Indicates whether videocapture is playing.
        /// </summary>
        bool isPlaying;

        /// <summary>
        /// The save path.
        /// </summary>
        string savePath;

        private int frameCountUpdate = 0;

        private int currentScreenWidth;
        private int currentScreenHeight;

        [SerializeField] private CallFfmpeg callFfmpeg;
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void FileDownLoad(byte[] bytes, int size, string filename);
#endif

        // Use this for initialization
        void Start()
        {
            playButton.interactable = false;
            saveAviMovieButton.interactable = false;
            saveMp4MovieButton.interactable = false;
            previewPanel.gameObject.SetActive(false);
            Initialize();
        }

        private void Initialize()
        {
            Texture2D imgTexture = Resources.Load("face") as Texture2D;

            Mat imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);

            Utils.texture2DToMat(imgTexture, imgMat);

            Texture2D texture = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);

            Utils.matToTexture2D(imgMat, texture);
        }

        void Update()
        {
            frameCountUpdate++;
            if (isPlaying)
            {
                //Loop play
                if (capture.get(Videoio.CAP_PROP_POS_FRAMES) >= capture.get(Videoio.CAP_PROP_FRAME_COUNT))
                    capture.set(Videoio.CAP_PROP_POS_FRAMES, 0);

                if (capture.grab())
                {
                    capture.retrieve(previewRgbMat, 0);

                    Imgproc.rectangle(previewRgbMat, new Point(0, 0),
                        new Point(previewRgbMat.cols(), previewRgbMat.rows()), new Scalar(0, 0, 255), 3);

                    Imgproc.cvtColor(previewRgbMat, previewRgbMat, Imgproc.COLOR_BGR2RGB);
                    Utils.fastMatToTexture2D(previewRgbMat, previrwTexture);
                }
            }
        }

        public string indexedDBDataPath;

        public void SaveMovieToIndexedDB()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(savePath);
            if (bytes != null)
            {
                indexedDBDataPath = Application.persistentDataPath + "/testMovie.avi";
                File.WriteAllBytes(indexedDBDataPath, bytes);
                string tmpString = "saved";
                Debug.Log(indexedDBDataPath + ":" + tmpString);

                //なぜかPlayerPrefsでセーブを行うとindexedDBが更新される
                PlayerPrefs.SetString("PlayerPrefsTest", tmpString);
            }
        }

        public void DownloadMovieData()
        {
            string filename = "movie.avi";
            byte[] bytes = System.IO.File.ReadAllBytes(savePath);
            if (bytes != null)
            {
#if UNITY_EDITOR
                string path = Path.Combine(Application.persistentDataPath, filename);
                File.WriteAllBytes(path, bytes);
                Debug.Log(filename + " has been saved into " + path);
#elif UNITY_WEBGL
                FileDownLoad(bytes, bytes.Length, filename);
#endif
            }
        }

        void OnPostRender()
        {
            if (isRecording && frameCount != frameCountUpdate)
            {
                frameCount = frameCountUpdate;
                if (frameCount >= maxFrameCount ||
                    recordingFrameRgbMat.width() != Screen.width || recordingFrameRgbMat.height() != Screen.height)
                {
                    OnRecButtonClick();
                    return;
                }

                // Take screen shot.
                screenCapture.ReadPixels(new UnityEngine.Rect(0, 0, Screen.width, Screen.height), 0, 0);
                screenCapture.Apply();

                Utils.texture2DToMat(screenCapture, recordingFrameRgbMat);
                Imgproc.cvtColor(recordingFrameRgbMat, recordingFrameRgbMat, Imgproc.COLOR_RGB2BGR);
                
                //動画上に文字を表示するプログラムです。
                // Imgproc.putText(recordingFrameRgbMat, frameCountUpdate.ToString(),
                //     new Point(recordingFrameRgbMat.cols() - 70, 30), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0,
                //     new Scalar(255, 255, 255), 2, Imgproc.LINE_AA, false);
                // Imgproc.putText(recordingFrameRgbMat, frameCount.ToString(),
                //     new Point(recordingFrameRgbMat.cols() - 70, 50), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0,
                //     new Scalar(255, 255, 255), 2, Imgproc.LINE_AA, false);
                // Imgproc.putText(recordingFrameRgbMat, "SavePath:", new Point(5, recordingFrameRgbMat.rows() - 30), Imgproc.FONT_HERSHEY_SIMPLEX, 0.8, new Scalar(0, 0, 255), 2, Imgproc.LINE_AA, false);
                // Imgproc.putText(recordingFrameRgbMat, savePath, new Point(5, recordingFrameRgbMat.rows() - 8), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, new Scalar(255, 255, 255), 0, Imgproc.LINE_AA, false);

                writer.write(recordingFrameRgbMat);
            }
        }

        private void StartRecording(string savePath)
        {
            if (isRecording || isPlaying)
                return;

            this.savePath = savePath;

            writer = new VideoWriter();
#if !UNITY_IOS
            writer.open(savePath, VideoWriter.fourcc('M', 'J', 'P', 'G'), movieFps, new Size(Screen.width, Screen.height));
#else
            writer.open(savePath, VideoWriter.fourcc('D', 'V', 'I', 'X'), 30, new Size(Screen.width, Screen.height));
#endif

            if (!writer.isOpened())
            {
                Debug.LogError("writer.isOpened() false");
                writer.release();
                return;
            }

            screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            recordingFrameRgbMat = new Mat(Screen.height, Screen.width, CvType.CV_8UC3);
            frameCount = 0;
            frameCountUpdate = 0;

            isRecording = true;
        }

        private void StopRecording()
        {
            if (!isRecording || isPlaying)
                return;

            if (writer != null && !writer.IsDisposed)
                writer.release();

            if (recordingFrameRgbMat != null && !recordingFrameRgbMat.IsDisposed)
                recordingFrameRgbMat.Dispose();

            Screen.SetResolution(currentScreenWidth, currentScreenHeight, false);
            isRecording = false;
        }

        private void PlayVideo(string filePath)
        {
            if (isPlaying || isRecording)
                return;

            capture = new VideoCapture();
            capture.open(filePath);

            if (!capture.isOpened())
            {
                Debug.LogError("capture.isOpened() is false. ");
                capture.release();
                return;
            }

            Debug.Log("CAP_PROP_FORMAT: " + capture.get(Videoio.CAP_PROP_FORMAT));
            Debug.Log("CAP_PROP_POS_MSEC: " + capture.get(Videoio.CAP_PROP_POS_MSEC));
            Debug.Log("CAP_PROP_POS_FRAMES: " + capture.get(Videoio.CAP_PROP_POS_FRAMES));
            Debug.Log("CAP_PROP_POS_AVI_RATIO: " + capture.get(Videoio.CAP_PROP_POS_AVI_RATIO));
            Debug.Log("CAP_PROP_FRAME_COUNT: " + capture.get(Videoio.CAP_PROP_FRAME_COUNT));
            Debug.Log("CAP_PROP_FPS: " + capture.get(Videoio.CAP_PROP_FPS));
            Debug.Log("CAP_PROP_FRAME_WIDTH: " + capture.get(Videoio.CAP_PROP_FRAME_WIDTH));
            Debug.Log("CAP_PROP_FRAME_HEIGHT: " + capture.get(Videoio.CAP_PROP_FRAME_HEIGHT));
            double ext = capture.get(Videoio.CAP_PROP_FOURCC);
            Debug.Log("CAP_PROP_FOURCC: " + (char) ((int) ext & 0XFF) + (char) (((int) ext & 0XFF00) >> 8) +
                      (char) (((int) ext & 0XFF0000) >> 16) + (char) (((int) ext & 0XFF000000) >> 24));


            previewRgbMat = new Mat();
            capture.grab();

            capture.retrieve(previewRgbMat, 0);

            int frameWidth = previewRgbMat.cols();
            int frameHeight = previewRgbMat.rows();
            previrwTexture = new Texture2D(frameWidth, frameHeight, TextureFormat.RGB24, false);

            capture.set(Videoio.CAP_PROP_POS_FRAMES, 0);

            previewPanel.texture = previrwTexture;

            isPlaying = true;
        }

        private void StopVideo()
        {
            if (!isPlaying || isRecording)
                return;

            if (capture != null && !capture.IsDisposed)
                capture.release();

            if (previewRgbMat != null && !previewRgbMat.IsDisposed)
                previewRgbMat.Dispose();

            isPlaying = false;
        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        void OnDestroy()
        {
            StopRecording();
            StopVideo();
        }

        /// <summary>
        /// Raises the back button click event.
        /// </summary>
        public void OnBackButtonClick()
        {
            SceneManager.LoadScene("OpenCVForUnityExample");
        }

        /// <summary>
        /// Raises the rec button click event.
        /// </summary>
        public void OnRecButtonClick()
        {
            if (isRecording)
            {
                recButton.GetComponentInChildren<UnityEngine.UI.Text>().color = Color.black;
                StopRecording();
                recButton.interactable = false;
                playButton.interactable = true;
                saveAviMovieButton.interactable = true;
                saveMp4MovieButton.interactable = true;

                previewPanel.gameObject.SetActive(false);

                callFfmpeg.CallJavaScriptFfmpegFunction();
            }
            else
            {
#if !UNITY_IOS
                currentScreenWidth = Screen.width;
                currentScreenHeight = Screen.height;
                StartRecording(Application.persistentDataPath + "/VideoWriterExample_output.avi");
#else
                StartRecording(Application.persistentDataPath + "/VideoWriterExample_output.m4v");
#endif

                if (isRecording)
                {
                    recButton.GetComponentInChildren<UnityEngine.UI.Text>().color = Color.red;
                    recButton.interactable = false;
                    playButton.interactable = false;
                    saveAviMovieButton.interactable = false;
                    saveMp4MovieButton.interactable = false;
                }
            }
        }

        /// <summary>
        /// Raises the play button click event.
        /// </summary>
        public void OnPlayButtonClick()
        {
            if (isPlaying)
            {
                StopVideo();
                playButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "録画した動画を再生する";
                recButton.interactable = false;
                previewPanel.gameObject.SetActive(false);
            }
            else
            {
                if (string.IsNullOrEmpty(savePath))
                    return;

                PlayVideo(savePath);
                playButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "動画の再生を止める";
                recButton.interactable = false;
                previewPanel.gameObject.SetActive(true);
            }
        }
    }
}