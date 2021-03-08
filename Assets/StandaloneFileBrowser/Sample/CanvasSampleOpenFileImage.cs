using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileImage : MonoBehaviour, IPointerDownHandler {
    //public RawImage output;
    //public SpriteRenderer outputSpriteRenderer;
    [SerializeField] private GameObject loadedSpriteObject;
    [SerializeField] private float loadedSpriteScaleRate = 1.0f;
    

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".png, .jpg", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutine(url));
    }
#else
    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    void Start() {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "", false);
        if (paths.Length > 0) {
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }
#endif

    private IEnumerator OutputRoutine(string url) {
        var loader = new WWW(url);
        yield return loader;
        Texture2D tex = loader.texture;
        //Texture2DをSpriteに変換
        SpriteRenderer loadedSpriteRenderer = loadedSpriteObject.GetComponent<SpriteRenderer>();
        loadedSpriteRenderer.sprite = Sprite.Create(tex,
            new Rect(0.0f, 0.0f, tex.width, tex.height),
            new Vector2(0.5f, 0.5f));
        ChangeSpriteScale();
        //_loadBackgroundImage.ChangeBackgroundImageScale();
    }
    
    private void ChangeSpriteScale()
    {
        SpriteRenderer sr = loadedSpriteObject.GetComponent<SpriteRenderer>();

        // カメラの外枠のスケールをワールド座標系で取得
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // スプライトのスケールもワールド座標系で取得
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        //  両者の比率を出してスプライトのローカル座標系に反映
        //背景の縦横比を固定する
        //横長の背景の場合
        //不等号を反対にすることで枠からはみ出ないようにしています。
        if (worldScreenWidth / worldScreenHeight < width / height)
        {
            loadedSpriteObject.transform.localScale =
                new Vector3(worldScreenWidth / width * loadedSpriteScaleRate, worldScreenWidth / width * loadedSpriteScaleRate);
        }
        //縦長の背景の場合
        else
        {
            loadedSpriteObject.transform.localScale =
                new Vector3(worldScreenHeight / height * loadedSpriteScaleRate, worldScreenHeight / height * loadedSpriteScaleRate);
        }

        //背景の縦横比を固定しない
        // {
        //     backgroundScreenObject.transform.localScale =
        //         new Vector3(worldScreenWidth / width, worldScreenHeight / height);
        // }
    }
}