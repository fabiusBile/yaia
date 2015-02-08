using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button>().onClick.AddListener(() => clk());
	}
	
	void clk(){
		Application.Quit ();
	}
}
