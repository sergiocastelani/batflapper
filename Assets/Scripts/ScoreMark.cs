using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScoreMark : MonoBehaviour 
{
	private Transform _camTransform;
	private float _camHalfHeight;

	private Transform _icon;

    void Start () {
        if (Score.HighestDistance >= Score.MIN_VALID_DISTANCE)
		{
            var mainCam = Camera.main;
            _camHalfHeight = mainCam.orthographicSize;
            _camTransform = mainCam.transform;

            _icon = transform.Find("icon").transform;

            var pos = transform.position;
			pos.x = Score.DistanceToWorld(Score.HighestDistance);
			transform.position = pos;
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	void Update() {
		var pos = _icon.position;
		pos.y = _camTransform.position.y - _camHalfHeight + 0.46f;
		_icon.position = pos;
	}

}
