using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class Spawner : MonoBehaviour
{
	public PrefabFactory factory;

    [Tooltip("The path of the factory object to find if 'factory' is null")]
    public string sceneFactoryObjectName;

	public Color gizmoColor = Color.yellow;

    private GameObject _spawn;

    public void OnEnable()
	{
        if (factory == null && sceneFactoryObjectName.Length > 0)
            factory = GameObject.Find(sceneFactoryObjectName).GetComponent<PrefabFactory>();

        _spawn = factory.CreateRandom();
		if(_spawn != null)
		{
			var conditions = _spawn.GetComponents<ISpawnCondition>();
			var conditionsPassed = conditions.All (c => c.ShouldSpawn ());
			if (!conditionsPassed) {
				Despawn ();
				return;
			}

			_spawn.transform.position = transform.position;
			_spawn.SetActive(true);
		}
	}

	public void OnDisable ()
	{
		Despawn ();
	}

	private void Despawn()
	{
		factory.Despawn(_spawn);
		_spawn = null;
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = gizmoColor;
		Gizmos.DrawSphere (transform.position, 0.2f);
	}
}
