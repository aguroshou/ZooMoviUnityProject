using System.Collections;
using System.Collections.Generic;
using OpenCVForUnitySample;
using UnityEngine;
using ZooMovi.Scripts;

public class CallFfmpeg : MonoBehaviour
{
   [SerializeField] private ZooMoviVideoWriterExample zooMoviVideoWriterExample;
   
    public void CallJavaScriptFfmpegFunction()
    {
        zooMoviVideoWriterExample.SaveMovieToIndexedDB();
        var executer = new NativeExecuter();
        var callbackParameter = new CallbackParameter
        {
            callbackGameObjectName = gameObject.name,
            callbackFunctionName = "Callback",
            callbackMoviePathName = zooMoviVideoWriterExample.indexedDBDataPath
        };
        var parameterJson = JsonUtility.ToJson(callbackParameter);
        Debug.Log("CallJavaScriptFfmpegFunction: "+zooMoviVideoWriterExample.indexedDBDataPath);
        executer.Execute("FfmpegMethod", parameterJson);
    }
    
    public void Callback()
    {
        Debug.Log("callback from js");
    }
}
