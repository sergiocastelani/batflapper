using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public float xDistance;

	Vector2 _currentVelocity;
	bool _shaking = false;

	public void Shake(){
		StartCoroutine(_shakeCoroutine());
	}

	IEnumerator _shakeCoroutine(){
		_shaking = true;
		var originalPosition = transform.position;
		var startTime = Time.time;

		do{
			transform.position = originalPosition + Random.insideUnitSphere * 0.3f;
			yield return new WaitForSeconds(0.05f);
		}while(Time.time - startTime < 0.5f);

		_shaking = false;
	}

	void Start () {
		_currentVelocity = Vector2.zero;
	}
	
	void Update () {
		if(!_shaking){
			Vector2 newCamPosition = target.position + new Vector3(xDistance, 0.0f, 0.0f);
			newCamPosition = Vector2.SmoothDamp(transform.position, newCamPosition, ref _currentVelocity, 0.5f, Mathf.Infinity, Time.deltaTime);
			transform.position = new Vector3(newCamPosition.x, newCamPosition.y, transform.position.z);
		}
	}
}

