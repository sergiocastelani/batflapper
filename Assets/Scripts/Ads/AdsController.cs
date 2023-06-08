using System;
using UnityEngine;

//Attach this at only one game object
class AdsController : MonoBehaviour
{
    static private IAdsImplementation _implementation = null;

    static public void PrepareAd()
    {
        Initialize();
        _implementation?.PrepareAd();
    }

    static public void ShowAd(Action<bool> adCompletedCallback)
    {
        _implementation?.ShowAd(adCompletedCallback);
    }

    static private void Initialize()
    {
        if (_implementation == null)
        {
#if UNITY_WEBGL
            _implementation = new AdsJavascript();
#else
            _implementation = new AdsUnity();
#endif
        }
    }

    private void Awake()
    {
        Initialize();
    }

}
