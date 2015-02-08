using UnityEngine;
using System.Collections;

public class SpawnFloatingText : ScriptableObject {
	public Vector2 position;
	public string text;
	//Конструктор
	public SpawnFloatingText (Vector2 position, string text)
	{
		float rand = Random.Range (-2, 1);
		this.position=new Vector2(position.x+rand,position.y+1);
		this.text=text;
	}

	public void Spawn(){
		GameObject ft = Resources.Load("FloatingTextPref") as GameObject;
		GameObject floatText = Instantiate(ft.gameObject,position,ft.transform.rotation) as GameObject;
		floatText.GetComponent<FloatingText>().Text=text;
		floatText.renderer.sortingLayerName="Ui";
 	}
}
