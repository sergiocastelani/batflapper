using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Flapper.Utils;
using Flapper.Utils.Transitions;

public class FloatingText : MonoBehaviour {
	Text _guiText;

	Director _currentDirector;

	void Start () {
		_guiText = GetComponent<Text>();
	}
	
	public void Show(string text, Color color, Vector3 startPosition) {
		if(_currentDirector != null)
			_currentDirector.Stop();

		transform.position = startPosition;	
		_guiText.color = color;
		_guiText.text = text;

		_currentDirector = new Director();
		_currentDirector.Add(Tweening.Position(transform, startPosition, startPosition + Vector3.up, 0.8f, Easing.Cubic.Out));
		_currentDirector.Play(this, null);
	}
}
