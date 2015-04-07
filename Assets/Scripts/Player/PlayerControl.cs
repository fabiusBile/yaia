using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
		[HideInInspector]
		public bool
				facingRight = true;			// For determining which way the player is currently facing.
		[HideInInspector]
		public bool
				jump = false;				// Condition for whether the player should jump.
	 

		public float moveForce = 365f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
		public float jumpForce = 1000f;			// Amount of force added when the player jumps.

	
		private Transform groundCheck;			// A position marking where to check if the player is grounded.
		private bool grounded = false;			// Whether or not the player is grounded.
		private Animator anim;					// Reference to the player's animator component.
		public Camera cam;
		public weapon_controller wc;
		public Transform foresight;
		//public Transform lHandle;
		public bool stunned = false;
		public float useRadius = 3f;
		public float unstAm = 0;
		public float unstSpeed = 0.5f;
		float nextTime = 0f;
		float nextCdTime = 0f;
		public Collider2D itemHit;
		bool unstunning = false;
		float cdToResp = 60f;
		public Transform checkpoint;
		Text MainMessage;
		hitpoints Hp;
		Text healthText;
		Image healthBar;
		public Image UseBar;
		public Image UseBarBg;
		float h;
		public Vector3 dir;
		public bool AttackAnimPlayed;
		public bool AttackCharging;
		public bool AttackCharged;
		Ability test;
		void Start(){
		test = new Ability ("test",gameObject);
		}
		void Awake ()
		{
				// Setting up references.
				groundCheck = transform.Find ("groundCheck");
				anim = GetComponent<Animator> ();
				anim.SetBool ("Jump", false);
				anim.SetBool ("Stunned", false);
				Hp = transform.GetComponent<hitpoints> ();
				healthText = GameObject.Find ("healthText").GetComponent<Text> ();
				healthBar = GameObject.Find ("healthBar").GetComponent<Image> ();
				MainMessage = GameObject.Find ("MainMessage").GetComponent<Text> ();
				UseBarBg = GameObject.Find ("UseBar_bg").GetComponent<Image> ();
				UseBar = GameObject.Find ("UseBar").GetComponent<Image> ();
		}

		void OnTriggerEnter2D (Collider2D col)
		{
				if (col.transform.name == "checkpoint") {
						checkpoint = col.transform;
				}
		}

		void Update ()
		{
				healthText.text = "HP:" + Hp.hp.ToString ();
				healthBar.fillAmount = Hp.hp / 100f;
				if (!stunned) {
						// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
						grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));  
						
						// If the jump button is pressed and the player is grounded then the player should jump.
						if (Input.GetButtonDown ("Jump") && grounded) {
								anim.SetBool ("Jump", true);
								jump = true;
						}
						if (Input.GetButtonDown ("Fire1"))
								GetComponent<PhotonView> ().RPC ("Fire", PhotonTargets.All, true);
						if (Input.GetButtonUp ("Fire1"))
								GetComponent<PhotonView> ().RPC ("Fire", PhotonTargets.All, false);
						if (Input.GetButtonDown ("Fire2"))
								GetComponent<PhotonView> ().RPC ("Fire2", PhotonTargets.All, true);
						if (Input.GetButtonUp ("Fire2"))
								GetComponent<PhotonView> ().RPC ("Fire2", PhotonTargets.All, false);
						if (Input.GetButtonDown ("Use")) {
								int bitmask = 1 << LayerMask.NameToLayer ("Items");
								//Кастуем круг радиусом useRadius
								itemHit = Physics2D.OverlapCircle (transform.position, useRadius, bitmask);
						}
						if (Input.GetButton ("Use")) {
								//Если круг чего-то коснулся
								test.execute();
								if (itemHit != null) {
										if (!UseBar.enabled) {
												UseBar.enabled = true;
												UseBarBg.enabled = true;
										}
										if (itemHit.tag == "Player" && itemHit.GetComponent<PlayerControl> ().stunned) {
												if (Vector2.Distance (transform.position, itemHit.transform.position) > useRadius) {
														PhotonNetwork.RPC (itemHit.gameObject.GetPhotonView (), "Unstunning", PhotonTargets.AllBuffered, false);
												}
												if (Time.time >= nextTime) {
														PhotonNetwork.RPC (itemHit.gameObject.GetPhotonView (), "Unstunning", PhotonTargets.AllBuffered, true);
														nextTime = Time.time + unstSpeed;
												}
												UseBar.fillAmount = itemHit.GetComponent<PlayerControl> ().unstAm;
										} else {
												UseBar.enabled = false;
												UseBarBg.enabled = false;
										}
								}
						}
						if (Input.GetButtonUp ("Use")) {
								UseBar.enabled = false;
								UseBarBg.enabled = false;
								if (itemHit != null) {
										if (itemHit.tag == "Player" && itemHit.GetComponent<PlayerControl> ().stunned) {
												PhotonNetwork.RPC (itemHit.gameObject.GetPhotonView (), "Unstunning", PhotonTargets.AllBuffered, false);
										}
								}
						}
				} else {
						UseBar.enabled = false;
						UseBarBg.enabled = false;
						if (itemHit != null) {
								if (itemHit.tag == "Player" && itemHit.GetComponent<PlayerControl> ().stunned) {
										PhotonNetwork.RPC (itemHit.gameObject.GetPhotonView (), "Unstunning", PhotonTargets.AllBuffered, false);
								}
						}
						if (!unstunning) {
								if (!MainMessage.enabled)
										MainMessage.enabled = true;
								if (cdToResp > 0) {
										if (Time.time >= nextCdTime) {
												cdToResp -= 0.1f;
												nextCdTime = Time.time + 0.1f;
												MainMessage.text = "To respawn:\n" + cdToResp.ToString ("#0.00");
										}
								} else
										transform.GetComponent<PhotonView> ().RPC ("Respawn", PhotonTargets.All, null);
						}
				}
		}

		void FixedUpdate ()
		{
				if (!stunned) {
						if (grounded) {
								h = Input.GetAxis ("Horizontal");
						}
						
						anim.SetFloat ("Speed", Mathf.Abs (rigidbody2D.velocity.x));
						if (h * rigidbody2D.velocity.x < maxSpeed)
								rigidbody2D.AddForce (Vector2.right * h * moveForce);
						// If the player's horizontal velocity is greater than the maxSpeed...
						if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
						// ... set the player's velocity to the maxSpeed in the x axis.
								rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
						
						// If the player should jump...
						if (jump) {
								// Set the Jump animator trigger parameter.


								// Play a random jump audio clip.
								// Add a vertical force to the player.
								rigidbody2D.AddForce (new Vector2 (0f, jumpForce));

								// Make sure the player can't jump again until the jump conditions from Update are satisfied.
								jump = false;
								anim.SetBool ("Jump", false);

						}
						//Поворот
						if (cam != null) {
								Ray ray = cam.camera.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
										//Разница между координатами игрока и курсора
										dir = new Vector3 (-(hit.point.x - foresight.position.x), hit.point.y - foresight.position.y, hit.point.z - foresight.position.z); 
										//С помощью арктангенса находим угол для поворота и переводим его в радианы
										float angle = Mathf.Atan2 (-dir.x, -dir.y) * Mathf.Rad2Deg;
										if (Vector3.Distance (transform.position, hit.point) > 0.5) {
												dir = new Vector3 (-(hit.point.x - foresight.position.x), hit.point.y - foresight.position.y, hit.point.z - foresight.position.z); 
												if (facingRight) {
														if (-angle + 90f > 95 && (angle > -170 && angle < 170)) {
																Flip ();
														}
												} else {
														if (angle + 90f > 95 && (angle > -170 && angle < 170)) {
																Flip ();
														}
												}
										}
								}
						}
				}
		}
	
		void Flip ()
		{
				// Switch the way the player is labelled as facing.
				facingRight = !facingRight;

				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;
				cam.GetComponent<Cam> ().Flip ();
		}

		[RPC]
		void Respawn ()
		{
				rigidbody2D.MovePosition(checkpoint.position);
				transform.GetComponent<hitpoints> ().hp = 10f;
				unstunning = false;
				stunned = false;
				cdToResp = 100f;
				anim.SetBool ("Stunned", false);
				MainMessage.enabled = false;
		}

		[RPC]
		void Fire (bool f)
		{
				wc.fire = f;
		}

		[RPC]
		void Fire2 (bool f)
		{
				wc.fire2 = f;
		}

		public bool facing ()
		{
				return facingRight;
		}
			
		[RPC]
		void Unstunning (bool am)
		{
				if (am) {
						unstAm += 0.01f;
						unstunning = true;
				} else {
						unstAm = 0;
						cdToResp = 60f;
						unstunning = false;
				}
				if (unstAm >= 1) {
						transform.GetComponent<PhotonView> ().RPC ("Unstun", PhotonTargets.AllBuffered);
						unstAm = 0;
				}
		}
		
		[RPC]
		void Unstun ()
		{
				stunned = false;
				anim.SetBool ("Stunned", false);
				gameObject.layer = LayerMask.NameToLayer ("Player");
				Hp.hp = 25f;
				MainMessage.enabled = false;
		}

		[RPC]
		void Stun ()
		{
				stunned = true;
				anim.SetBool ("Stunned", true);
				wc.fire = false;
				wc.fire2 = false;
				gameObject.layer = LayerMask.NameToLayer ("Items");
				
		}
}
