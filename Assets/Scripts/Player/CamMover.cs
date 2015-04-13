using UnityEngine;
using System.Collections;

public class CamMover : MonoBehaviour {
	public Transform Player;
	private Vector3 nextPos;
	public float yOffset;
	// Use this for initialization
	void Start () {
		nextPos.z = transform.position.z;
		Player = transform.root;
		Player.GetComponent<PlayerControl> ().cam = transform.GetChild (0).GetComponent<Camera>();
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		nextPos.x = Player.position.x;
		nextPos.y = Player.position.y+yOffset;
		transform.position = nextPos;
	}
}


