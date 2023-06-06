using UnityEngine;
using UnityEngine.UI;

public class StageScore : MonoBehaviour {
	public static StageScore instance;

	public Text distanceIntegralDisplay;
	public Text distanceDecimalDisplay;
	public Text scoreDisplay;

    public Transform trackedObject;


    private int _distance = 0;
    private int _coins = 0;

    public int Distance {
		get { return _distance; }
		set {
            _distance = value;
            var integral = value / 100;
            var decim = (value % 100) / 10;
            distanceIntegralDisplay.text = integral.ToString() + ".";
            distanceDecimalDisplay.text = decim.ToString() + " m";
        }
	}

	public int Coins {
		get { return _coins; }
		set { _coins = value; }
	}

	public int FinalScore
	{
		get { 
			return _distance + _coins * Score.MONEY_SCORE_VALUE; 
		}
	}

	public void UpdateScoreDisplay()
	{
        scoreDisplay.text = FinalScore.ToString();
    }

    void OnEnable () {
		instance = this;
	}

	private float _scoreUpdateAccumulatedDeltaTime = 0.0f;

    void Update() {
        Distance = Score.WorldToDistance(trackedObject.position.x);

		//update score HUD only a few times per second
		_scoreUpdateAccumulatedDeltaTime += Time.deltaTime;
		if (_scoreUpdateAccumulatedDeltaTime > 0.3f)
		{
            scoreDisplay.text = FinalScore.ToString();
			_scoreUpdateAccumulatedDeltaTime = 0.0f;
        }
    }

}
