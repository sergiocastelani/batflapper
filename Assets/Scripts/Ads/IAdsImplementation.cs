using System;
using UnityEngine.Advertisements;

interface IAdsImplementation
{
    public void PrepareAd();

    public void ShowAd(Action<bool> adCompletedCallback);
}
