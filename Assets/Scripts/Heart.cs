using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flapper.Utils;
using Flapper.Utils.Transitions;

public class Heart : MonoBehaviour {
	private Collider2D _collider;

	private Vector3 _originalScale;

	void Awake()
	{
		_originalScale = transform.localScale;
		_collider = GetComponent<Collider2D> ();
		_beatingAnimation();
	}

	void OnEnable()
	{
		transform.localScale = _originalScale;
		_collider.enabled = true;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		_collider.enabled = false;
		var player = GameObject.FindWithTag("Player");
		player.GetComponent<BatLife> ().RecoverHp ();
		_collectAnimation();
	}

	private void _beatingAnimation(){
		var smallScale = _originalScale - new Vector3(0.5f, 0.5f, 0f);

		var director = new Director();
		director.Add(Tweening.Scale(transform, _originalScale, smallScale, 0.1f, Easing.Linear));
		director.AddSyncPoint();
		director.Add(Tweening.Scale(transform, smallScale, _originalScale , 0.5f, Easing.Elastic.Out));
		director.AddSyncPoint();
		director.AddSleep(0.5f);
		director.Play(this, _beatingAnimation);
	}

	private void _collectAnimation(){
		var director = new Director();
		director.Add(Tweening.Position(transform, transform.localPosition, transform.localPosition + Vector3.up, 0.8f, Easing.Bounce.Out));
		director.Add(Tweening.Blink(GetComponent<SpriteRenderer>(), false, 1f));
		director.Play(this, null);
	}
}
