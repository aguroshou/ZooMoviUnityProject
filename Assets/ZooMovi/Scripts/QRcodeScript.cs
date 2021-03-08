//引用: https://sirohood.exp.jp/20191002-2908/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing;        //QRコード作成に必要
using ZXing.QrCode; //QRコード作成に必要

public class QRcodeScript : MonoBehaviour
{

    public SpriteRenderer QRcodeSprite;//最終的に表示するSpriteRendererオブジェクト

    private Texture2D EncodedQRTextire;//エンコードして出来たQRコードのTxture2Dが入る

    private int QrTxtureW = 256;//作成するテクスチャサイズ
    private int QrTxtureH = 256;//作成するテクスチャサイズ

    [SerializeField] private string QrCodeUrl;//QRコード化したいURL
    [SerializeField] private InputField InputQrCodeUrl;
    

    public void SetQrCodeUrl()
    {

        //オブジェクトがアタッチされていなければ取得する
        if (QRcodeSprite == null){
            QRcodeSprite = GameObject.Find("QRcodeSprite").GetComponent<SpriteRenderer>();    
        }
        
        //新規の空のテクスチャを作成
        EncodedQRTextire = new Texture2D(QrTxtureW, QrTxtureH);
        
        //QRコードで表示するリンクを指定
        //QrCodeUrl = "https://sirohood.exp.jp/wp-content/uploads/2018/03/IMG_2065.jpg";
        if (InputQrCodeUrl.text == "")
        {  
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
            QrCodeUrl = InputQrCodeUrl.text;
        }
        
    
        //エンコード処理
        var color32 = Encode(QrCodeUrl, EncodedQRTextire.width, EncodedQRTextire.height);

        //https://docs.unity3d.com/2018.4/Documentation/ScriptReference/Texture2D.SetPixels32.html
        //ピクセルカラーのブロックを設定
        EncodedQRTextire.SetPixels32(color32);
        
        //https://docs.unity3d.com/ja/2017.4/ScriptReference/Texture2D.Apply.html
        //エンコードで取得した情報で変更を適用する
        EncodedQRTextire.Apply();

        //スプライトを作成してオブジェクトに張り付け
        QRcodeSprite.sprite = Sprite.Create(EncodedQRTextire, new Rect(0,0,QrTxtureW,QrTxtureH),Vector2.zero);
         
    }

   


    //32 ビット形式での RGBA の色の表現
    //https://docs.unity3d.com/ja/2018.4/ScriptReference/Color32.html

    //エンコード処理（ここはサンプル通り）
    private static Color32[] Encode(string textForEncoding, int width, int height){
        var writer = new BarcodeWriter{
            Format = BarcodeFormat.QR_CODE,
            
            Options = new QrCodeEncodingOptions{
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }


}