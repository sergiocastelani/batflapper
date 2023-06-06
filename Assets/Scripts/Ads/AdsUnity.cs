using System;
using UnityEngine;
using UnityEngine.Advertisements;

class AdsUnity : 
    IAdsImplementation, 
    IUnityAdsInitializationListener, IUnityAdsLoadListener, 
    IUnityAdsShowListener
{
    [SerializeField] private const string _androidGameId = "5304566";
    [SerializeField] private const string _iOSGameId = "5304567";
    [SerializeField] private bool _testMode = false;
    private string _gameId;

    [SerializeField] private const string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] private const string _iOSAdUnitId = "Interstitial_iOS";
    private string _adUnitId;

    private Action<bool> _adCompletedCallback;

    public AdsUnity() 
    {
#if UNITY_IOS
        _gameId = _iOSGameId;
        _adUnitId = _iOSAdUnitId;
        _testMode = false;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
        _adUnitId = _androidAdUnitId;
        _testMode = false;
#elif UNITY_EDITOR
        _gameId = _androidGameId;
        _adUnitId = _androidAdUnitId;
        _testMode = true;
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        else
        {
            Debug.Log("Can't initialize Unity Ads");
        }
    }

    public void PrepareAd()
    {
        if (Advertisement.isInitialized)
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }
    }

    public void ShowAd(Action<bool> adCompletedCallback)
    {
        _adCompletedCallback = adCompletedCallback;

        if (Advertisement.isInitialized)
        {
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }
        else
        {
            _adCompletedCallback(true);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error} - {message}");
        _adCompletedCallback(true);
    }

    public void OnUnityAdsShowStart(string _adUnitId)
    {
    }

    public void OnUnityAdsShowClick(string _adUnitId)
    {
        _adCompletedCallback(true);
    }

    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        _adCompletedCallback(true);
    }
}
