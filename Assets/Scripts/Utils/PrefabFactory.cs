using UnityEngine;
using System.Collections.Generic;

public class PrefabFactory : MonoBehaviour
{
	private List<GameObject> _originals = new List<GameObject> ();
	private List<GameObject> _used = new List<GameObject> ();

	void Start ()
	{
		foreach (Transform child in transform) {
			var go = child.gameObject;
			_originals.Add (go);
			go.SetActive (false);
		}
	}

	public void Despawn(GameObject go)
	{
		if(go != null)
		{
			go.SetActive(false);

			if ( _used.Remove(go) )
				_originals.Add(go);
		}
	}

	public GameObject CreateRandom ()
	{
		if(_originals.Count > 0){
			var index = Random.Range (0, _originals.Count);
			var obj = _originals [index];
			_originals.RemoveAt(index);
			_used.Add(obj);
			return obj;
		}else{
			return null;
		}
	}
}
