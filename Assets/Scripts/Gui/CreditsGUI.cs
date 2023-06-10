using UnityEngine;
using UnityEngine.UI;

public class CreditsGUI : MonoBehaviour 
{
    public StartMenuGUI startMenuGUI;

    public void Show()
	{
		gameObject.SetActive(true);
	}

	public void CloseButtonClick()
	{
		gameObject.SetActive(false);
		startMenuGUI.ShowMainPanel();
    }

}
