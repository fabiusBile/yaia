using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public Transform mob;
	public bool autoSpawn;

	// Use this for initialization
	void Start ()
	{
		if (autoSpawn){
			Spawn();
			GameObject.Destroy(gameObject);
		}

	}
	public void Spawn(){
		if (PhotonNetwork.isMasterClient){
			PhotonNetwork.Instantiate (mob.name.ToString (), transform.position, transform.rotation, 0);
		}
	}
}
