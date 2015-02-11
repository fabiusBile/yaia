using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {
	public Transform Player;
	Vector3 nextPos;
	int dir=1;
	public float margin;
	bool FacingRight=true;
	public float moveSpeed=5f;
	// Use this for initialization
	void Start () {
		nextPos.z = transform.position.z;
	}

	public void Flip(){
		margin = - margin;
		FacingRight = !FacingRight;
		dir = -dir;
	}
	
	// Update is called once per frame
	void Update () {
		nextPos.x = transform.parent.position.x+margin;
		nextPos.y = transform.parent.position.y;
		if (!FacingRight&&transform.position.x >= nextPos.x) 
			transform.Translate (Vector3.left*Time.deltaTime*moveSpeed);
		if (FacingRight&&transform.position.x <= nextPos.x) 
			transform.Translate (Vector3.right*Time.deltaTime*moveSpeed);
	}
}
