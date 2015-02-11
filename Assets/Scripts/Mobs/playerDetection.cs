using UnityEngine;
using System.Collections;

public class playerDetection : MonoBehaviour {
	void OnTriggerStay2D(Collider2D  collider){
		if (collider.gameObject.tag == "Player"&&!collider.GetComponent<PlayerControl>().stunned) {
			transform.parent.GetComponent<enemy_simple> ().PlayerDetected (collider.transform);
		}
	}
}
