using System;
using System.Runtime.InteropServices;

class AdsJavascript : IAdsImplementation
{

    [DllImport("__Internal")]
    private static extern void JS_PrepareAd();

    [DllImport("__Internal")]
    private static extern void JS_ShowAd();

    public void PrepareAd()
    {
        JS_PrepareAd();
    }

    public void ShowAd(Action<bool> adCompletedCallback)
    {
        JS_ShowAd();
        adCompletedCallback(true);
    }
}
