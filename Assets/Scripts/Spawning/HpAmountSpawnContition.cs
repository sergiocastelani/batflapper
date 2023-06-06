using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpAmountSpawnContition : MonoBehaviour, ISpawnCondition 
{
	public int MinimumHp;

	public bool ShouldSpawn ()
	{
		var player = GameObject.FindWithTag ("Player");
		var playerHp = player.GetComponent<BatLife> ().Hp;
		return playerHp <= MinimumHp;
	}
}
