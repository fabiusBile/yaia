using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
		weapon_controller weaponController;
		public float fireDelay = 0.5f;
		Animator anm;
		PlayerControl pc;
		bool prevFire;
		float swordLength;
		Transform swordEnd;
		Transform swordHandle;
		public float swingDamage = 100f;
		public float stubDamage = 100f;
		public float minMultiplayer = 0.7f;
		public float maxMultiplayer = 1.5f;
		float damage = 0;
		float multiplayer;
		GameObject target;
		// Use this for initialization
		void Start ()
		{
				transform.root.GetComponent<Animator> ().SetBool ("Sword", true);
				pc = transform.root.GetComponent<PlayerControl> ();
				weaponController = transform.parent.GetComponent<weapon_controller> ();
				anm = transform.root.GetComponent<Animator> ();
				swordEnd = transform.GetChild (0);
				swordHandle = transform.parent;
				swordLength = Vector2.Distance (swordHandle.position, swordEnd.position);
		}

		// Update is called once per frame
		void Update ()
		{
				bool fire = weaponController.fire;
				bool fire2 = weaponController.fire2;
		if (fire && !pc.AttackCharged && !pc.AttackCharging && !pc.AttackAnimPlayed && !fire2) {
						anm.SetTrigger ("FireStart");
						anm.ResetTrigger ("FireStop");
						multiplayer = minMultiplayer;
						damage = swingDamage;
						target = null;
				}
				if (fire2 && !pc.AttackCharged && !pc.AttackCharging && !pc.AttackAnimPlayed && !fire) {
						anm.SetTrigger ("Fire2Start");
						anm.ResetTrigger ("Fire2Stop");
						multiplayer = minMultiplayer;
						damage = stubDamage;
						target = null;
				}
				if (pc.AttackCharged) {
						if (fire || fire2) {
								if (multiplayer < maxMultiplayer)
										multiplayer += 0.05f;
						} else {
								if (!fire) {
										anm.SetTrigger ("FireStop");
										pc.AttackCharged = false;
										pc.AttackCharging = false;
								}
								if (!fire2) {
										anm.SetTrigger ("Fire2Stop");
										pc.AttackCharged = false;
										pc.AttackCharging = false;
								}
						}
				}
				if (pc.AttackAnimPlayed) {
						RaycastHit2D[] hits;
						hits = Physics2D.RaycastAll (swordHandle.position, Vector2.ClampMagnitude (new Vector2 (swordEnd.position.x - swordHandle.position.x, swordEnd.position.y - swordHandle.position.y), 1f), swordLength);
						foreach (RaycastHit2D hit in hits) {
								if (hit.transform.tag != "Player" && !hit.collider.isTrigger) {
										if (hit.transform.GetComponent<hitpoints> ()) {
												if (hit.transform.root.gameObject != target) {
														target = hit.transform.root.gameObject;
														object[] data = new object[2];
														data [0] = damage * multiplayer;
														data [1] = hit.transform.position;
														hit.transform.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.All, data);
												}
										}
								}
						}
				}
		}
}
