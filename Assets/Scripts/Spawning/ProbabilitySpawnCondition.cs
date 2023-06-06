using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySpawnCondition : MonoBehaviour, ISpawnCondition
{
	public int Probability;

	public bool ShouldSpawn()
	{
		return Random.Range (0, 100) < Probability;
	}
}
