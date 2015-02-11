using UnityEngine;
using System.Collections;

public class MenusInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] menus = GameObject.FindGameObjectsWithTag ("Menu");
				foreach (GameObject item in menus) {
						if (item!=gameObject){
							item.transform.position=transform.position;
							item.SetActive(false);
						}
				}
	}
}
