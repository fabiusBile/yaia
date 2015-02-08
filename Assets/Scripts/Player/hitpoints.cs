using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class hitpoints : MonoBehaviour
{
		public float hp = 100;
		[RPC]
		void TakeDamage (object[] serialized)
		{
				float dmg = (float)serialized[0];
				Vector3 pos = (Vector3)serialized[1];
				if (hp > 0){
						hp -= dmg;
						SpawnFloatingText sft = new SpawnFloatingText(pos,"-"+dmg+"hp");
						sft.Spawn();
				}
				if (hp <= 0) {
						if (transform.tag != "Player") {
								if (PhotonNetwork.isMasterClient)
										PhotonNetwork.Destroy (gameObject);
						} else {
								transform.GetComponent<PhotonView> ().RPC ("Stun", PhotonTargets.AllBuffered, null);
						}
				}
		}
}
