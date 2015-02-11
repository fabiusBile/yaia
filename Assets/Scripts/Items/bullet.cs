using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	public float damage = 10;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 3);
	
	}

	void OnCollisionEnter2D (Collision2D coll){
	if (coll.gameObject!=null)
	if(coll.gameObject != gameObject)
		
		if (coll.gameObject.GetComponent<hitpoints>()){
			object[] data = new object[2];
			data[0]=damage;
			data[1]=transform.position;
			coll.gameObject.GetComponent<PhotonView>().RPC("TakeDamage",PhotonTargets.All,data);
		}
		Destroy (gameObject);
	}
}
