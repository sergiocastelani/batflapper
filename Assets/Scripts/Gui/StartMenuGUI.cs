using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Flapper.Utils.Transitions;

public class StartMenuGUI : MonoBehaviour {
	private Text _availableLivesText;
	private Slider _lifeProgressSlider;
	private Text _lifeProgressText;
	private AudioSource _invalidSound;

    void OnEnable()
	{
		try
		{
            AdsController.PrepareAd();
        } 
		catch(Exception ex) 
		{
			Debug.LogException(ex);
		}

        _availableLivesText = transform.Find("panel/lives/availableText").GetComponent<Text>();
		_lifeProgressSlider = transform.Find("panel/lives/progressBar").GetComponent<Slider>();
		_lifeProgressText = transform.Find("panel/lives/progressText").GetComponent<Text>();
        _invalidSound = transform.Find("invalidSound").GetComponent<AudioSource>();

        var maxLivesText = transform.Find("panel/lives/maxText").GetComponent<Text>();
		maxLivesText.text = "(max. " + Lives.Max + ")";

        var bestDistanceText = transform.Find("panel/scores/bestDistanceText").GetComponent<Text>();
        var integral = Score.HighestDistance / 100;
        var decim = (Score.HighestDistance % 100) / 10;
        bestDistanceText.text = integral.ToString() + ".";
        bestDistanceText.text += decim.ToString() + " m";

        var bestScoreText = transform.Find("panel/scores/bestScoreText").GetComponent<Text>();
        bestScoreText.text = Score.HighestScore.ToString();

        StartCoroutine(updateLifeProgress());
		PlayerPrefs.Save();
	}

	void OnDisable(){
		PlayerPrefs.Save();
	}

	private IEnumerator updateLifeProgress(){
		while(true){
			Lives.Update();
			_availableLivesText.text = Lives.Available.ToString();

			var countdown = Lives.TimeForNextIncrement;
			var progress = 1.0f - (Mathf.Floor((float)countdown.TotalSeconds) / (float) Lives.SECONDS_PER_INCREMENT);
			progress = Mathf.Max(0.08f, progress); //ProgressBar is buggy when too low

			_lifeProgressSlider.value = progress;
			_lifeProgressText.text = String.Format ("{0:00}:{1:00}", countdown.Minutes, countdown.Seconds+1);

			yield return new WaitForSeconds(1);
		}
	}

	public void Show(){
		var	mainPanel = transform.Find("panel").gameObject;
		var director = new Director();
		director.Add(Tweening.Scale(mainPanel.transform, Vector3.zero, Vector3.one, 1f, Easing.Elastic.Out));
		director.Add(Tweening.Rotation2D(mainPanel.transform, 720f, 0f, 0.5f, Easing.Linear));
		director.Play( this, null );

		transform.Find("openSound").GetComponent<AudioSource>().Play();
	}

	public void PlayButtonClick(){
		if(Lives.Available > 0){
			Lives.Available -= 1;
			SceneManager.LoadScene ("mainscene");
		}
		else
		{
			_invalidSound.Play();

            var playButton = transform.Find("panel/playButton");
            var director1 = new Director();
            director1.Add(Tweening.Color(playButton.GetComponent<Image>(), new Vector4(1, 0, 0, 1), Vector4.one, 1.0f, Easing.Cubic.Out));
            director1.Play(this, null);

            var heartIcon = transform.Find("panel/lives/heart").transform;
            var director2 = new Director();
            director2.Add(Tweening.Scale(heartIcon, Vector3.zero, heartIcon.localScale, 1f, Easing.Elastic.Out));
            director2.Play(this, null);
        }
    }

	public void VideoButtonClick()
	{
		try 
		{
            AdsController.ShowAd((ok) =>
            {
                AdsController.PrepareAd();

                if (ok)
                    Lives.Available = Lives.Max;
            });
        }
        catch (Exception e)
		{
            Debug.LogException(e);
            Lives.Available = Lives.Max;
        }
    }
}
