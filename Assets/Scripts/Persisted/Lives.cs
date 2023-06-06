using System;
using UnityEngine;

public class Lives {
	public const int INITIAL_MAX = 10;
	public const int SECONDS_PER_INCREMENT = 5*60;

	private static int _max = -1;
	private static int _available = -1;
	private static Nullable<DateTime> _nextIncrementTime;

	public static int Max {
		get {
			if(_max < 0){
				_max = PlayerPrefs.GetInt("Lives.Max", INITIAL_MAX);
				PlayerPrefs.SetInt("Lives.Max", _max);
			}
			return _max;
		}
		set {
			_max = value;
			PlayerPrefs.SetInt("Lives.Max", _max);
		}
	}

	public static int Available {
		get {
			if(_available < 0){
				_available = PlayerPrefs.GetInt("Lives.Available", Max);
				PlayerPrefs.SetInt("Lives.Available", _available);
			}
			return _available;
		}
		set {
            if (value > _available)
                NextIncrementTime = DateTime.UtcNow + TimeSpan.FromSeconds(SECONDS_PER_INCREMENT);

            _available = (int) Mathf.Min((float) value, (float) Max);
            _available = (int) Mathf.Max((float) _available, 0.0f);
            PlayerPrefs.SetInt("Lives.Available", _available);
		}
	}

	public static DateTime NextIncrementTime {
		get {
			if(_nextIncrementTime == null){
				var timeString = PlayerPrefs.GetString("Lives.NextIncrementTime", (DateTime.UtcNow + TimeSpan.FromSeconds(SECONDS_PER_INCREMENT)).ToString());
				PlayerPrefs.SetString("Lives.NextIncrementTime", timeString);
				_nextIncrementTime = DateTime.Parse(timeString);
			}
			return _nextIncrementTime.Value;
		}
		set {
			_nextIncrementTime = value;
			PlayerPrefs.SetString("Lives.NextIncrementTime", _nextIncrementTime.Value.ToString());
		}
	}

	public static TimeSpan TimeForNextIncrement {
		get {
			return NextIncrementTime - DateTime.UtcNow;
		}
	}

	public static void Update() {
		var now = DateTime.UtcNow;
		if (now >= NextIncrementTime){
			var timeElapsed = (now - NextIncrementTime).Seconds;
			var increments = 1 + (timeElapsed / SECONDS_PER_INCREMENT);
			Available += increments;
		}
	}

}
