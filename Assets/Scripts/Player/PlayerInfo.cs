using UnityEngine;
using System.Collections;
using System;

public class PlayerInfo : MonoBehaviour
{
		public	string name;
		public string cls;
		string weaponName;
		string weaponType;
		Sprite weaponSprite;
		public PlayersData pd;
		public NetworkPlayersData npd;
		public Transform WeaponsHandle;
		Hashtable prefs;
		// Use this for initialization
		void Start ()
		{
				GameObject ds = GameObject.Find ("PlayersData");
				npd = ds.GetComponent<NetworkPlayersData> ();
				pd = npd.localPd;
				if (gameObject.GetComponent<PhotonView> ().isMine) {
						name = pd.data ["name"].ToString ();
						cls = pd.data ["class"].ToString ();
						//Item weapon = pd.data["weapon"] as Item;
						weaponType = pd.CurentWeapon.type;
						weaponName = pd.CurentWeapon.itName;
						prefs = pd.CurentWeapon.prefs;
						weaponSprite = Resources.Load ("Sprites/" + pd.CurentWeapon.image, typeof(Sprite)) as Sprite;
						SpawnWeapon ();
				} else {
						PlayersData otherPd = npd.Get (gameObject.GetComponent<PhotonView> ().owner.name);
						name = otherPd.data ["name"].ToString ();
						cls = otherPd.data ["class"].ToString ();
						weaponType = otherPd.CurentWeapon.type;
						weaponName = otherPd.CurentWeapon.itName;
						prefs = otherPd.CurentWeapon.prefs;
						weaponSprite = Resources.Load ("Sprites/" + otherPd.CurentWeapon.image, typeof(Sprite)) as Sprite;
						SpawnWeapon ();
				}

		}

		void SpawnWeapon ()
		{
				GameObject weapon = Instantiate (Resources.Load ("Entity/Weapons/" + weaponType, typeof(GameObject))) as GameObject;
				weapon.GetComponent<SpriteRenderer> ().sprite = weaponSprite;
				weapon.transform.SetParent (WeaponsHandle);
				weapon.transform.localPosition = Vector3.zero;
				weapon.transform.localRotation = Quaternion.Euler (Vector3.zero);
				weapon.transform.localScale = new Vector3 (1f, 1f, 1f);
				switch (weaponType) {
				case "sword":
						Sword sword = weapon.GetComponent<Sword> ();
						sword.swingDamage = Convert.ToSingle (prefs ["swingDamage"]);
						sword.stubDamage = Convert.ToSingle (prefs ["stubDamage"]);
						sword.maxMultiplayer = Convert.ToSingle (prefs ["maxMultiplayer"]);
						sword.minMultiplayer = Convert.ToSingle (prefs ["minMultiplayer"]);
						break;
				}
		}
}
