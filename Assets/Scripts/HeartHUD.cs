using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flapper.Utils;
using Flapper.Utils.Transitions;

public class HeartHUD : MonoBehaviour {

	Vector3 _originalScale;

	public void Awake(){
		_originalScale = transform.localScale;
	}

	public void OnEnable(){
		transform.localScale = _originalScale;
	}

	public void hide () {
		var director = new Director();
		director.Add(Tweening.Scale(transform, _originalScale, Vector3.zero, 1, Easing.Bounce.Out));
		director.Play(this, null);
	}

	public void show () {
		var director = new Director();
		director.Add(Tweening.Scale(transform, Vector3.zero, _originalScale, 1, Easing.Bounce.Out));
		director.Play(this, null);
	}

}
