using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sonar : MonoBehaviour {
	public Material animatedMaterialTwoSteps;
	public Material animatedMaterialOneStep;
	public float totalFadeTime;
	public float secondsPerPulse;
	public float pulseSpeed;

	float _currentAmbience;
	Vector3 _currentSonarPosition;
	float _currentSonarRadius;
	float _lastPulseTime;

	public void Stop(){
			animatedMaterialTwoSteps.SetFloat("_Ambience", 1.0f);
			animatedMaterialTwoSteps.SetVector("_SonarPosition", this._currentSonarPosition);
			animatedMaterialTwoSteps.SetFloat("_SonarRadius", 0.0f);
			animatedMaterialOneStep.SetFloat("_Ambience", 1.0f);
			animatedMaterialOneStep.SetVector("_SonarPosition", this._currentSonarPosition);
			animatedMaterialOneStep.SetFloat("_SonarRadius", 0.0f);
			enabled = false;
	}

	void Awake () {
		this._currentAmbience = 0.0f;
		this._currentSonarPosition = Camera.main.WorldToScreenPoint(this.transform.position);
		this._currentSonarRadius = 1.1f;
		this._lastPulseTime = 0.0f;
	}

	void Update () {
#if UNITY_EDITOR
		if(!Application.isPlaying) {
			animatedMaterialTwoSteps.SetFloat("_Ambience", 1.0f);
			animatedMaterialTwoSteps.SetVector("_SonarPosition", this._currentSonarPosition);
			animatedMaterialTwoSteps.SetFloat("_SonarRadius", 0.0f);
			animatedMaterialOneStep.SetFloat("_Ambience", 1.0f);
			animatedMaterialOneStep.SetVector("_SonarPosition", this._currentSonarPosition);
			animatedMaterialOneStep.SetFloat("_SonarRadius", 0.0f);
			return;
		}
#endif

		//ambience light decay
		var ambienceDecayPerSecond = 1.0f/this.totalFadeTime;
		_currentAmbience -= ambienceDecayPerSecond * Time.deltaTime;
		_currentAmbience = Mathf.Max(0.0f, _currentAmbience);

		//sonar pulse
		var maxPulseRadius = Mathf.Max(Screen.width, Screen.height) * 1.4f;
		var pulsePixelsPerSecond = maxPulseRadius * this.pulseSpeed;

		if(_currentSonarRadius < 1.0f){
			var pulseTimeEllapsed = Time.timeSinceLevelLoad - _lastPulseTime;
			if(pulseTimeEllapsed > this.secondsPerPulse){
				_lastPulseTime = Time.timeSinceLevelLoad;
				_currentSonarPosition = Camera.main.WorldToScreenPoint(this.transform.position);
				_currentSonarRadius = pulsePixelsPerSecond * (pulseTimeEllapsed % this.secondsPerPulse);
				_currentSonarRadius = Mathf.Max(1.0001f, _currentSonarRadius);
			}
		}else{
			_currentSonarRadius += pulsePixelsPerSecond * Time.deltaTime;
			if(_currentSonarRadius > maxPulseRadius){
				_currentSonarRadius = 0.1f;
				_currentAmbience = 1.0f;
			}
		}

		animatedMaterialTwoSteps.SetFloat("_Ambience", this._currentAmbience);
		animatedMaterialTwoSteps.SetVector("_SonarPosition", this._currentSonarPosition);
		animatedMaterialTwoSteps.SetFloat("_SonarRadius", this._currentSonarRadius);
		animatedMaterialOneStep.SetFloat("_Ambience", this._currentAmbience);
		animatedMaterialOneStep.SetVector("_SonarPosition", this._currentSonarPosition);
		animatedMaterialOneStep.SetFloat("_SonarRadius", this._currentSonarRadius);
	}
}

