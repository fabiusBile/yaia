using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour
{
		public	string name;
		public string cls;
		string weaponName;
		Sprite weaponSprite;
		public PlayersData pd;
		public NetworkPlayersData npd;
		public Transform WeaponsHandle;
		// Use this for initialization
		void Start ()
		{
				GameObject ds = GameObject.Find ("PlayersData");
				pd = ds.GetComponent<PlayersData> ();
				npd = ds.GetComponent<NetworkPlayersData> ();
				if (gameObject.GetComponent<PhotonView> ().isMine) {
						name = pd.data ["name"].ToString ();
						cls = pd.data ["class"].ToString ();
						//Item weapon = pd.data["weapon"] as Item;
						
						weaponName = pd.CurentWeapon.itName;
						weaponSprite = Resources.Load("Sprites/"+pd.CurentWeapon.image,typeof(Sprite)) as Sprite;
						SpawnWeapon();
				} else {
						PlayersData otherPd = npd.Get (gameObject.GetComponent<PhotonView> ().owner.name);
						name = otherPd.data ["name"].ToString ();
						cls = otherPd.data ["class"].ToString ();
						weaponName = otherPd.data ["weapon"].ToString ();
						weaponSprite = Resources.Load("Sprites/"+otherPd.data["weaponSprite"],typeof(Sprite)) as Sprite;
						SpawnWeapon();
				}

		}

		void SpawnWeapon ()
		{
				GameObject weapon = Instantiate (Resources.Load ("Entity/Weapons/"+weaponName, typeof(GameObject))) as GameObject;
				weapon.GetComponent<SpriteRenderer>().sprite=weaponSprite;
				weapon.transform.SetParent (WeaponsHandle);
				weapon.transform.localPosition = Vector3.zero;
				weapon.transform.localRotation = Quaternion.Euler (Vector3.zero);
				weapon.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
}
