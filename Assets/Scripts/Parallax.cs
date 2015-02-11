using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
	
		Vector3 prevPos;
		public Transform[] bg;
		public float parallaxScale=2f;
		public float[] parallaxScales;
		float yDif;

		// Use this for initialization
		void Start ()
		{
				backgrounds Backgrounds = GameObject.Find ("backgrounds").GetComponent<backgrounds>();
				bg = Backgrounds.bgs;
		parallaxScales=Backgrounds.scales;
				prevPos = transform.position;
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				Vector3 p = new Vector2 ((prevPos.x - transform.position.x) * parallaxScale, (prevPos.y - transform.position.y) * parallaxScale);
				for (int i=0; i!=bg.Length; i++) {
			Vector3 pi = new Vector3 (p.x *-i/10*parallaxScales[i] + bg [i].position.x, p.y * -i/10*parallaxScales[i] + bg [i].position.y, bg [i].position.z);
						bg [i].position = Vector3.Lerp (bg [i].transform.position, pi, Time.deltaTime);

				}
				prevPos = transform.position;
		}
}
