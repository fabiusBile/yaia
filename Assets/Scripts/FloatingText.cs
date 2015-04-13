using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {
	Color clr;
	float multiplayer = 1.01f;
	public void Init (Vector2 position, string text) {
		float rand = Random.Range (-2, 1);
		transform.position=new Vector2(position.x+rand,position.y+1);
		GetComponent<Renderer>().sortingLayerName="Ui";
		clr=transform.GetComponent<TextMesh>().color;
		transform.GetComponent<TextMesh>().text=text;
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
