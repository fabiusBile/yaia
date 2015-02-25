using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventoryCellsScaler : MonoBehaviour {
	void OnEnable () {
		GridLayoutGroup	grid = GetComponent<GridLayoutGroup> ();
		Rect rect = GetComponent<RectTransform> ().rect;
		grid.cellSize = new Vector2 (rect.size.x / 20f,rect.size.x/20f);
		grid.spacing = grid.cellSize * 3f;
		grid.padding.top = Mathf.RoundToInt(grid.cellSize.y*1.5f);
		grid.padding.bottom = grid.padding.top;
		grid.padding.left = grid.padding.top;
		grid.padding.right = grid.padding.top;
		GetComponentInParent<ScrollRect> ().verticalScrollbar.value = 0;
	}
	void OnDisable(){
		for (int i=0; i!=transform.childCount; i++) {
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}
	}
}
