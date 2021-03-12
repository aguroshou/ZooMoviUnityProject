# ZooMoviUnityProject
## リポジトリの概要
Zoomの動画背景を簡単に作成するブラウザアプリ「ずーむび」のUnityプロジェクトです。

UnityのWebGLビルドだけでは.mp4形式の動画を作成することはできなかったのですが、JavaScriptのFFMPEG ウェブアセンブリを使用することにより.mp4形式の動画を作成することができています。

## ずーむび アプリ紹介
こちらがZooMoviUnityProjectをWebGLでビルドしたアプリになります。

https://aguroshou.github.io/ZoomVirtualBackgroundMovieMaker/

## 使用方法
#### OpenCV for Unityのアセット
このUnityプロジェクトを利用するためには、95ドルのOpenCV for Unityのアセットをインポートする必要があります。

UnityのWebGL上で.avi形式の動画を作成する際に使用しています。

https://assetstore.unity.com/packages/tools/integration/opencv-for-unity-21088?locale=ja-JP

#### Unityのバージョン
Unity2020.2.1fでのみ動作確認をしています。

※Unity2019などのバージョンでは、「Mac Retina Display」の設定により、Macのディスプレイ上でアプリの解像度が2倍に増加する現象を確認しています。

そのため、動画の解像度が2倍の状態で保存される不具合を確認しています。

#### ずーむびをWebGLビルドするまでの手順
1. Unity2020.2.1fでZooMoviUnityProjectを開きます。
1. OpenCV for Unityのアセットをインポートします。
1. ビルドの設定画面で「WebGL」ビルドに設定し、ZooMoviMainSceneを追加してビルドしてください。
1. このリポジトリにある「index.html」をビルドしたアプリの中にある「index.html」に差し替えます。
1. 「index.html」の83, 85, 86, 87行目の「*****」の部分をビルド時に設定した名前に変更してください。
1. 「Web Server for Chrome」や「GitHub Pages」上でアプリを開くことにより、ずーむびを使用することができます。

## ZooMoviUnityProjectを改善する手順
OSSとしてZooMoviUnityProjectに機能を追加・整理などしてくださる方を歓迎しています。

ZooMoviUnityProjectを編集する際の手順は以下の通りになります。
1. このリポジトリを「fork」します。
2. main以外のブランチを作成し、そこで作業をしてコミット・プッシュします。
3. mainブランチへプルリクエストを作成してください。

## 連絡先
不具合や質問などがある場合には、以下のTwitterアカウントへご連絡していただければ幸いです。

https://twitter.com/aguroshou
