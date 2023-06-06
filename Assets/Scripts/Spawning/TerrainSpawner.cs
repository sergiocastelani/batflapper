using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flapper.Utils;
using UnityEditor;

public class TerrainSpawner : MonoBehaviour {

	[Tooltip("The path of the factory object to find if 'factory' is null")]
    public string sceneFactoryObjectName;

    private Transform _camTransform;
	private float _startTriggerPos;
	private float _endTriggerPos;
    
	private PrefabFactory _factory;
    private GameObject _parentTerrain;

    private System.Action _onUpdateAction;

    void OnEnable()
	{
        if (_factory == null && sceneFactoryObjectName.Length > 0)
            _factory = GameObject.Find(sceneFactoryObjectName).GetComponent<PrefabFactory>();

		if (_parentTerrain == null)
			_parentTerrain = transform.parent.parent.gameObject;
        
		Camera cam = Camera.main;
		_camTransform = cam.transform;
		_startTriggerPos = transform.position.x - (cam.orthographicSize * cam.aspect) - 1;
		_endTriggerPos = transform.position.x + (cam.orthographicSize * cam.aspect) + 1;

        _onUpdateAction = SpawnNextIfCameraIsNear;
    }

    void Update()
	{
		if(_onUpdateAction != null) 
			_onUpdateAction();
	}

	private void SpawnNextIfCameraIsNear()
	{
		if (_camTransform.position.x < _startTriggerPos)
			return;

		var nextTerrain = _factory.CreateRandom();
		if(nextTerrain == null)
		{
			Debug.LogError("Need more terrains in the PrefabFactory");
            _onUpdateAction = null;
		}
		else
		{
			var entrance = nextTerrain.transform.Find("Grid/entrance");
			var deltaPosition = entrance.transform.position - this.transform.position;
			nextTerrain.transform.position -= deltaPosition;
			nextTerrain.SetActive(true);

			_onUpdateAction = DespawnSelfIfCameraIsFar;
		}
	}

	private void DespawnSelfIfCameraIsFar()
	{
		if (_camTransform.position.x < _endTriggerPos)
			return;

		_factory.Despawn(_parentTerrain);
        _onUpdateAction = null;
	}
}
