using UnityEngine;
using System;
using System.Collections.Generic;

/* Score is tracked in centimeters */
public class Score {

    public const int MONEY_SCORE_VALUE = 150;
    public const int MIN_VALID_DISTANCE = 10;
    public const int MIN_VALID_SCORE = 10;

    //2.5 game units = 100 centimeters
    private const float WORLD_TO_DISTANCE_RATIO = 100f/2.5f;

    public static int WorldToDistance(float worldPosition) {
		return (int)(worldPosition * WORLD_TO_DISTANCE_RATIO);
	}

	public static float DistanceToWorld(int distance) {
		return distance / WORLD_TO_DISTANCE_RATIO;
	}

	private static int _highestDistance = -1;
    private static int _highestScore = -1;

    public static int HighestDistance {
		get {
			if(_highestDistance < 0)
                _highestDistance = PlayerPrefs.GetInt("Score.HighestDistance", 0);

			return _highestDistance;
        }

		set {
            if (value >= MIN_VALID_DISTANCE)
            {
                _highestDistance = value;
                PlayerPrefs.SetInt("Score.HighestDistance", value);
            }
        }
	}

    public static int HighestScore
    {
        get
        {
            if (_highestScore < 0)
                _highestScore = PlayerPrefs.GetInt("Score.HighestScore", 0);

            return _highestScore;
        }

        set
        {
            if (value >= MIN_VALID_SCORE)
            {
                _highestScore = value;
                PlayerPrefs.SetInt("Score.HighestScore", value);
            }
        }
    }
}
