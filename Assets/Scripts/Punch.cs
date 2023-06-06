using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flapper.Utils;
using Flapper.Utils.Transitions;

public class Punch : MonoBehaviour {

	void OnEnable(){
		GetComponent<AudioSource>().pitch = 1;
	}

	public void Show(Vector3 position, Action finishCallback){
		transform.position = position;

		var director = new Director();
		director.Add(Tweening.Scale(transform, Vector3.zero, Vector3.one, 1.5f, Easing.Elastic.Out));
		director.Add(Tweening.Rotation2D(transform, 720f, 0f, 0.3f, Easing.Linear));
		director.Play(this, finishCallback);

		GetComponent<AudioSource>().Play();
	}

	public void slowSound(){
		GetComponent<AudioSource>().pitch = 0.7f;
	}

}
