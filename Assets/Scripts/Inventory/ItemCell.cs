using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{

		public GameObject panel;
		public Text panelText;
		public string info;
		bool showed;
		public Vector2 padding;

		public void ShowInfo (bool show)
		{
				if (show) {
						panelText.text = info;
						showed = true;
						Update();
						panel.SetActive (true);
				} else {
						panel.SetActive (false);
						showed = false;
				}
		}

		void Update ()
		{
				if (showed) {
					Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					pz.z=panel.transform.position.z;
					pz.x+=padding.x;
					pz.y+=padding.y;
					panel.transform.position=pz;
				}
		}


}
