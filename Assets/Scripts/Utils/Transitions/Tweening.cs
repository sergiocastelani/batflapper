using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Flapper.Utils.Transitions {

public class Tweening {

	protected class WaitTime{
		private float _time;
		private float _originalTime;

		public WaitTime(float time){ _time = _originalTime = time; }

		public bool Done() {
			_time -= Time.deltaTime;
			return _time <= 0;
		}

		public void Reset(){ _time = _originalTime; }
	}

    public static IEnumerator Position(Transform target, Vector3 start, Vector3 end, float duration, Func<float, float> easing)
	{
		var steps = _EasingSteps(start, end, duration, easing);
		foreach (var stepValue in steps)
		{
			target.localPosition = stepValue;
			yield return null;
		}
    }

    public static IEnumerator Scale(Transform target, Vector3 start, Vector3 end, float duration, Func<float, float> easing)
	{
		var steps = _EasingSteps(start, end, duration, easing);
		foreach (var stepValue in steps)
		{
			target.localScale = stepValue;
			yield return null;
		}
    }
	
    public static IEnumerator Rotation2D(Transform target, float startDegree, float endDegree, float duration, Func<float, float> easing)
	{
		var steps = _EasingSteps(startDegree, endDegree, duration, easing);
		foreach (var stepValue in steps)
		{
			target.localRotation = Quaternion.AngleAxis(stepValue, Vector3.forward);
			yield return null;
		}
    }
	
    public static IEnumerator Color(SpriteRenderer target, Vector4 start, Vector4 end, float duration, Func<float, float> easing)
	{
		var steps = _EasingSteps(start, end, duration, easing);
		foreach (var stepValue in steps)
		{
			target.color = stepValue;
			yield return null;
		}
    }

    public static IEnumerator Color(Image target, Vector4 start, Vector4 end, float duration, Func<float, float> easing)
	{
		var steps = _EasingSteps(start, end, duration, easing);
		foreach (var stepValue in steps)
		{
			target.color = stepValue;
			yield return null;
		}
    }

    public static IEnumerator Blink(SpriteRenderer target, bool visibleAtEnd, float duration)
	{
		var alpha = 1f - target.color.a;
		var time = 0f;
		var wait = new WaitTime(0.1f);

		while(time < duration){
			yield return null;
			if(wait.Done()){
				wait.Reset();
				
				alpha = 1 - alpha;
				target.color = new Vector4(target.color.r, target.color.g, target.color.b, alpha);

				time += 0.1f;
				time = Mathf.Min(time, duration);
			}
		}

		target.color = new Vector4(target.color.r, target.color.g, target.color.b, visibleAtEnd ? 1f : 0f);
    }

    public static IEnumerator Blink(Image target, bool visibleAtEnd, float duration)
	{
		var alpha = 1f - target.color.a;
		var time = 0f;
		var wait = new WaitTime(0.1f);

		while(time < duration){
			yield return null;
			if(wait.Done()){
				wait.Reset();
				
				alpha = 1 - alpha;
				target.color = new Vector4(target.color.r, target.color.g, target.color.b, alpha);

				time += 0.1f;
				time = Mathf.Min(time, duration);
			}
		}

		target.color = new Vector4(target.color.r, target.color.g, target.color.b, visibleAtEnd ? 1f : 0f);
    }

	// ----------- Easing -------------

	private static IEnumerable<Vector4> _EasingSteps(Vector4 start, Vector4 end, float duration, Func<float, float> easing) 
	{
		yield return start;
		
		var delta = end - start;
		var time = 0f;
		while (time < duration){
			time += Time.deltaTime;
			time = Mathf.Min(time, duration);
			yield return start + (delta * easing(time/duration));
		}
	}
	
	private static IEnumerable<Vector3> _EasingSteps(Vector3 start, Vector3 end, float duration, Func<float, float> easing) 
	{
		yield return start;
		
		var delta = end - start;
		var time = 0f;
		while (time < duration){
			time += Time.deltaTime;
			time = Mathf.Min(time, duration);
			yield return start + (delta * easing(time/duration));
		}
	}
	
	private static IEnumerable<float> _EasingSteps(float start, float end, float duration, Func<float, float> easing) 
	{
		yield return start;
		
		var delta = end - start;
		var time = 0f;
		while (time < duration){
			time += Time.deltaTime;
			time = Mathf.Min(time, duration);
			yield return start + (delta * easing(time/duration));
		}
	}
	
}
    
} //namespace