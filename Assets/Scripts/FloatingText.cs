using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {
	Color clr;
	public string Text;
	float multiplayer = 1.01f;
	// Use this for initialization
	void Start () {
		clr=transform.GetComponent<TextMesh>().color;
		transform.GetComponent<TextMesh>().text=Text;
		GameObject.Destroy(gameObject,1.5f);
	}
	void FixedUpdate(){
		transform.Translate(Vector3.up*0.09f*multiplayer);
		multiplayer *= 1.01f;
		float a = clr.a-0.02f;
		clr = new Color(clr.r,clr.g,clr.b,a);
		transform.GetComponent<TextMesh>().color=clr;
	}
}
