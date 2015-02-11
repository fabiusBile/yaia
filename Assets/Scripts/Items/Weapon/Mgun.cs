using UnityEngine;
using System.Collections;

public class Mgun : MonoBehaviour
{
		public Rigidbody2D bullet;
		public float BulletSpeed = 10;
		public Transform foresight;
		public float fireDelay = 0.5f;
		private float nextFire = 0.0f;
		public Transform handsR;
		public Transform handsL;
		public Transform lHandle;
		PlayerControl pc;
		bool facingRight;
		weapon_controller weaponController;
		// Update is called once per frame
		void Start ()
		{
				Transform body = transform.root.GetChild (1);
				handsL = body.FindChild ("HandL");
				handsR = body.FindChild ("HandR");
				lHandle = transform.FindChild ("LHandle");
				pc = transform.root.GetComponent<PlayerControl> ();
				facingRight = true;
				transform.root.GetComponent<Animator> ().SetBool ("Mgun", true);
				weaponController = transform.parent.GetComponent<weapon_controller> ();
		}
		void FixedUpdate ()
		{
			if (weaponController.enabled) {
						bool fire = weaponController.fire;
						Vector3 dir = pc.dir;
						Vector2 direction = Vector2.ClampMagnitude ((foresight.position - transform.position), 1f);
						if (fire == true && Time.time > nextFire) {
								Rigidbody2D shootedBullet = Instantiate (bullet, foresight.position, foresight.rotation) as Rigidbody2D;
								shootedBullet.velocity = direction * BulletSpeed;
								nextFire = Time.time + fireDelay;
						}
						Vector3 dirL = new Vector3 (-(lHandle.position.x - handsL.position.x), lHandle.position.y - handsL.position.y, lHandle.position.z - handsL.position.z); 
						//С помощью арктангенса находим угол для поворота и переводим его в радианы
						float angle = Mathf.Atan2 (-dir.x, -dir.y) * Mathf.Rad2Deg;
						float angleL = Mathf.Atan2 (-dirL.x, -dirL.y) * Mathf.Rad2Deg;
						if (facingRight) {
								if (-angle + 90f > 95 && (angle > -170 && angle < 170)) {
										facingRight = !facingRight;
								}
								handsR.rotation = Quaternion.AngleAxis (-angle + 90f - 2f + foresight.rotation.z, Vector3.forward);
								handsL.rotation = Quaternion.AngleAxis (-angleL + 45f, Vector3.forward);
						} else {
								if (angle + 90f > 95 && (angle > -170 && angle < 170)) {
										facingRight = !facingRight;
								}
								handsR.rotation = Quaternion.AngleAxis (angle + 90f - 3f + foresight.rotation.z, Vector3.forward);
								handsL.rotation = Quaternion.AngleAxis (angleL + 45f, Vector3.forward);
						}
				}
		}
}