using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventoryCellsScaler : MonoBehaviour {
	public	void OnEnable () {
		GridLayoutGroup	grid = GetComponent<GridLayoutGroup> ();
		Rect gridRect = GetComponent<RectTransform> ().rect;
		if (transform.childCount > 0) {
						Rect cellRect = transform.GetChild (0).GetComponent<RectTransform> ().rect;
						float scale = transform.GetChild(0).localScale.x;
						int count = Mathf.RoundToInt ((gridRect.width) / cellRect.width/scale);
						grid.childAlignment = transform.childCount>=count ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
				}
	}
}
