using UnityEngine;
using System.Collections;

public class enemy_simple : MonoBehaviour
{
		[HideInInspector]
		public bool
				facingRight = true;			// For determining which way the player is currently facing.
		[HideInInspector]
		public Transform
				jumpCheck;				//Диагональный визор. Проверяет на наличие ям.
		[HideInInspector]
		public Transform
				jumpCheckSecond;		//Горизонтальный визор. Проверяет на наличие стен. 
		[HideInInspector]
		public bool
				jump = false;				// Condition for whether the player should jump.

		public float moveDir;
		public bool ShouldJump = true;
		public float moveForce = 365f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
		public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
		public float jumpForce = 1000f;			// Amount of force added when the player jumps.
		public AudioClip[] taunts;				// Array of clips for when the player taunts.
		public float tauntProbability = 50f;	// Chance of a taunt happening.
		public float tauntDelay = 1f;			// Delay for when the taunt should happen.


		private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
		private Transform groundCheck;			// A position marking where to check if the player is grounded.
		private bool grounded = false;			// Whether or not the player is grounded.
		private Animator anim;					// Reference to the player's animator component.
		public float maxTargetDist = 10f;
		public Transform TargetPlayer;
		public float Damage = 5f;
		public float AttackDelay = 0.5f;
		float NextAttack = 0f;
		Vector3 realPos;

		void Awake ()
		{
				// Setting up references.
				groundCheck = transform.FindChild ("groundCheck");
				jumpCheckSecond = transform.FindChild ("jumpCheckSecond");
				jumpCheck = transform.FindChild ("jumpCheck");
				anim = GetComponent<Animator> ();
		}

		void Update ()
		{

				if (PhotonNetwork.isMasterClient) {
						//С помощью тернарного оператора определяем направление и на основе его даем переменной значение 1 или -1 (1 - право, -1 - лево)
						int facing = facingRight == true ? 1 : -1;
						Vector2 direction;
						direction.x = facing;
						direction.y = 0;


						//С помощью двух лучей определяем необходимость в прыжке\смене направления

						RaycastHit2D DiagJumpVisor = Physics2D.Linecast (transform.position, jumpCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
						RaycastHit2D HorizJumpVisor = Physics2D.Raycast (jumpCheckSecond.position, direction, 3, 1 << LayerMask.NameToLayer ("Ground"));


						//Может прыгать только тогда, когда находится на земле
						grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

						//Флажок Should Jump в инспекторе отвечает за то, будет бот прыгать либо поворачиваться

						if ((!DiagJumpVisor || HorizJumpVisor) && grounded) {
								if (ShouldJump == true)
										jump = true;
								else
										moveDir = -moveDir;
						}
				}
				if (TargetPlayer != null) {
						if (PhotonNetwork.isMasterClient) {
								if (Vector2.Distance (transform.position, TargetPlayer.position) > maxTargetDist)
										TargetPlayer = null;
								else if (Vector2.Distance (transform.position, TargetPlayer.position) > 2) {
										if (TargetPlayer.position.x > transform.position.x)
												moveDir = 1;
										else 
												moveDir = -1;
								}
						} 
						if (Vector2.Distance(transform.position,TargetPlayer.position)<1){
								moveDir = 0;
								object[] data = new object[2];
								data[0] = Damage;
								data[1]=new Vector3(TargetPlayer.position.x,TargetPlayer.position.y+2f,TargetPlayer.position.z);
								if (Time.time >= NextAttack) {
										TargetPlayer.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.All, data);
										NextAttack = Time.time + AttackDelay;
								}
						}
				}
		}

		void FixedUpdate ()
		{
				if (PhotonNetwork.isMasterClient) {
						// The Speed animator parameter is set to the absolute value of the horizontal input.
						anim.SetFloat ("Speed", Mathf.Abs (moveDir));

						// The Speed animator parameter is set to the absolute value of the horizontal input.
						// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
						if (moveDir * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			// ... add a force to the player.
								GetComponent<Rigidbody2D>().AddForce (Vector2.right * moveDir * moveForce);

						// If the player's horizontal velocity is greater than the maxSpeed...
						if (Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
								GetComponent<Rigidbody2D>().velocity = new Vector2 (Mathf.Sign (GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
				} else{
					PhotonView pview = transform.GetComponent<PhotonView> ();
					if (!pview.isMine) { 
							transform.position=Vector3.Lerp(transform.position,realPos,0.1f); 
					}
				}
				// If the input is moving the player right and the player is facing left...
				if (moveDir > 0 && !facingRight)
			// ... flip the player.
						Flip ();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (moveDir < 0 && facingRight)
			// ... flip the player.
						Flip ();

				// If the player should jump...
				if (jump) {
						// Set the Jump animator trigger parameter.
						//anim.SetTrigger ("Jump");

						// Add a vertical force to the player.
						GetComponent<Rigidbody2D>().AddForce (new Vector2 (0f, jumpForce));

						// Make sure the player can't jump again until the jump conditions from Update are satisfied.
						jump = false;
				}
		}

		public void PlayerDetected (Transform player)
		{
				if (TargetPlayer == null || TargetPlayer.GetComponent<PlayerControl> ().stunned)
						TargetPlayer = player;
		}
	
		void Flip ()
		{
				// Switch the way the player is labelled as facing.
				facingRight = !facingRight;

				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;
		}

		public bool facing ()
		{
				return facingRight;
		}

		void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
		
				if (stream.isWriting) { 
					stream.SendNext(transform.position);
					stream.SendNext(moveDir);
				}
				if (stream.isReading){
					realPos = (Vector3)stream.ReceiveNext();
					 moveDir =  (float)stream.ReceiveNext();
				}
		}
}
