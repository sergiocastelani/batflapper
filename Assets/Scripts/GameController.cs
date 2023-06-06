using UnityEngine;
using Flapper.Utils.Transitions;

public class GameController : MonoBehaviour {

    public GameObject bat;
    public StartMenuGUI gameOverGUI;
    public Transform newDistanceRecordHUD;
    public Transform newScoreRecordHUD;

    private int? _currentBestDistance;
    private int? _currentBestScore;

    void OnEnable() {
        gameOverGUI.gameObject.SetActive(false);
        _currentBestDistance = Score.HighestDistance;
        _currentBestScore = Score.HighestScore;
    }

    void Update() {
        if (_currentBestDistance != null && _currentBestDistance > Score.MIN_VALID_DISTANCE && StageScore.instance.Distance > _currentBestDistance)
        {
            _currentBestDistance = null;
            ShowRecordIcon(newDistanceRecordHUD);
        }

        if (_currentBestScore != null && _currentBestScore > Score.MIN_VALID_SCORE && StageScore.instance.FinalScore > _currentBestScore)
        {
            _currentBestScore = null;
            ShowRecordIcon(newScoreRecordHUD);
        }
    }

    public void GameOver () {
        //stop updating scores
        StageScore.instance.UpdateScoreDisplay();
        StageScore.instance.gameObject.SetActive(false);


        //keep high scores
        Score.HighestDistance = Mathf.Max(Score.HighestDistance, StageScore.instance.Distance);
        Score.HighestScore = Mathf.Max(Score.HighestScore, StageScore.instance.FinalScore);

        //slow motion
        Time.timeScale = 0.5f;
        bat.GetComponent<AudioSource>().pitch = 0.7f;

        Invoke("_gameOverStep2", 1.5f);
	}

    protected void _gameOverStep2(){
        //back to normal time
		Time.timeScale = 1f;
        bat.GetComponent<AudioSource>().Stop();

        gameOverGUI.gameObject.SetActive(true);
        gameOverGUI.Show();
    }

    private void ShowRecordIcon(Transform iconGroup)
    {
        if (! iconGroup.gameObject.activeInHierarchy )
        {
            iconGroup.gameObject.SetActive(true);
            var d = new Director();
            d.Add(Tweening.Scale(iconGroup, new Vector3(0, 0, 1), Vector3.one, 1f, Easing.Elastic.Out));
            d.Play(this, null);

            iconGroup.Find("sound").GetComponent<AudioSource>().Play();
        }
    }

}
