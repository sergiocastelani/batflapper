using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Flapper.Utils.Transitions {

public class Director
{
	private Queue<List<IEnumerator>> _batches;
	private List<IEnumerator> _currentBatch;

	private MonoBehaviour _coOwner;

	public Director(){
		_batches = new Queue<List<IEnumerator>>();
		AddSyncPoint();
	}

	//Adds an iterator function to run in parallel
	public void Add(IEnumerator iterator){
		if(_coOwner != null){
			throw new System.InvalidOperationException("The director has been played already.");
		}

		_currentBatch.Add(iterator);
	}

	//waits all previous iterator functions terminate before starting the next ones
	public void AddSyncPoint(){
		if(_coOwner != null){
			throw new System.InvalidOperationException("The director has been played already.");
		}

		_currentBatch = new List<IEnumerator>();
		_batches.Enqueue(_currentBatch);
	}

	public void AddSleep(float seconds){
		if(_coOwner != null){
			throw new System.InvalidOperationException("The director has been played already.");
		}

		_currentBatch.Add(_sleep(seconds));
	}

	public void Play(MonoBehaviour coroutineOwner, Action finishCallback){
		if(_coOwner != null){
			throw new System.InvalidOperationException("The director has been played already.");
		}

		_coOwner = coroutineOwner;
		coroutineOwner.StartCoroutine(_Playback(finishCallback));
	}

	public void Stop(){
		if(_coOwner != null){
			_coOwner.StopCoroutine("_Playback");
		}
	}

	private IEnumerator _Playback(Action finishCallback){
		while(_batches.Count > 0){
			_currentBatch = _batches.Dequeue();
			while(_currentBatch.Count > 0){

				for(var i=0; i < _currentBatch.Count; ){
					if(_currentBatch[i].MoveNext()){
						++i;
					} else {
						_currentBatch.RemoveAt(i);
					}
				}

				yield return null;
			}
		}

		if(finishCallback != null) finishCallback();
	}

	private IEnumerator _sleep(float seconds){
		while(seconds > 0){
			yield return null;
			seconds -= Time.deltaTime;
		}
	}
}

} //namespace
