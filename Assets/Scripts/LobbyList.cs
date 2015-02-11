using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyList : MonoBehaviour
{
		public Transform list;
		public GameObject button;
		public GameObject lobby;
		public GameObject popup;
		public Font font;

		void Start ()
		{
				GridLayoutGroup ls = list.GetComponent<GridLayoutGroup> ();
				Vector2 sps = new Vector2 (list.parent.GetComponent<RectTransform> ().rect.width - 200f - ls.padding.left - ls.padding.right, ls.spacing.y);
				ls.spacing = sps;
		}

		public	void OnEnable ()
		{
				for (int i=0; i!=list.childCount; i++) {
						Destroy (list.GetChild (i).gameObject);
				}
				if (PhotonNetwork.connected) {
						foreach (RoomInfo r in PhotonNetwork.GetRoomList())
								addTolist (r);
				}
		}

		void addTolist (RoomInfo r)
		{
				GameObject btn = (GameObject)Instantiate (button);
				GameObject label = new GameObject ("roomLbl");
				label.AddComponent<Text> ();
				label.GetComponent<Text> ().font = font;
				label.GetComponent<Text> ().text = r.name;
				label.GetComponent<Text> ().color = Color.black;
				label.transform.SetParent (list);
				btn.transform.SetParent (list);
				btn.GetComponent<Button> ().onClick.AddListener (delegate {
						connect (r);
				});
		}

		void connect (RoomInfo r)
		{
				OnEnable ();
				PhotonNetwork.JoinRoom (r.name);
		}

		void OnJoinedRoom ()
		{
				gameObject.SetActive (false);
				lobby.SetActive (true);
		}

}
