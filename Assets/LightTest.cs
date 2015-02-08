using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LightTest : MonoBehaviour
{
		public GameObject dummy;
		public float length = 10f;
		// Update is called once per frame
		void Update ()
		{
				MeshFilter mf = GetComponent<MeshFilter> ();
				List<Vector3> rays = CastRays ();
				Vector3[] vertices = new Vector3[rays.Count + 1];

				Mesh mesh = new Mesh ();
				int i = 1;
				vertices [0] = new Vector3 (0f, 0f, 0f);
				foreach (Vector3 v in rays) {
						vertices [i] = v;
						i++;
				}
				
				int[] triangles = new int[(rays.Count + rays.Count / 2) * 2];	
				int n = 1;
				Debug.Log (triangles.Length);
				for (int t=0; t<triangles.Length; t+=3) {
						triangles [t] = 0;
						triangles [t + 1] = n;
						triangles [t + 2] = n + 1;
						if ((n + 1) == (vertices.Length))
								triangles [t + 2] = 1;
						n++;
				}	
				
				Vector3[] normals = new Vector3[rays.Count + 1];
				for (int t=0; t!=normals.Length; t++) {
						normals [t] = -Vector3.forward;
				}
				n = 1;
				Vector2[] uv = new Vector2[rays.Count + 1];
				for (int t=0; t!=uv.Length; t++) {
						uv [t] = new Vector2 (0.5f + (vertices [t].x - vertices [0].x) / (2 * length), 
			                      0.5f + (vertices [t].y - vertices [0].y) / (2 * length));
				}

				mesh.vertices = vertices;
				mesh.triangles = triangles;
				mesh.normals = normals;
				mesh.uv = uv;
				mesh.Optimize();
				mf.mesh = mesh;
				Debug.Log (mesh.vertexCount);
				Debug.Log (mesh.triangles.Length);
				//foreach (int v in mesh.triangles) {
//				GameObject d = Instantiate (dummy, vertices [60], dummy.transform.rotation) as GameObject;
//
//				Destroy (d, 0.1f);
				//}

		}

	List<Vector3> CastRays ()
		{				
				List<Vector3> hits = new List<Vector3>();
				float j = 0;
				for (float i=-1f; i<=1f; i+=0.05f) {
						RaycastHit2D hit;
						hit = Physics2D.Raycast (transform.position, (new Vector2 (i, j)).normalized, length);
						Vector3 vert;
						if (hit.collider != null) {
								vert = new Vector3 (hit.point.x - transform.position.x, hit.point.y - transform.position.y, 0f);
						} else {
								vert = (new Vector2 (i, j)).normalized * length;
						}
						if (hits.Count > 2) {
				if (((hits [hits.Count - 1])).x == vert.x || (hits [hits.Count - 1]).y == vert.y)
								if ((hits [hits.Count - 2]).x == vert.x || (hits [hits.Count - 2]).y == vert.y)
										hits.RemoveAt (hits.Count - 1);
						}
						hits.Add (vert);
						j += i < 0 ? 0.025f : -0.025f; 
				}
				j = 0;
				for (float i=1f; i>=-1f; i-=0.05f) {
						RaycastHit2D hit;
						hit = Physics2D.Raycast (transform.position, (new Vector2 (i, j)).normalized, length);
						Vector3 vert;
						if (hit.collider != null) {
								vert = new Vector3 (hit.point.x - transform.position.x, hit.point.y - transform.position.y, 0f);
						} else {
								vert = (new Vector2 (i, j)).normalized * length;
						}
						if (hits.Count > 2) {
								if ((hits [hits.Count - 1]).x == vert.x || (hits [hits.Count - 1]).y == vert.y)
								if ((hits [hits.Count - 2]).x == vert.x || (hits [hits.Count - 2]).y == vert.y)
										hits.RemoveAt (hits.Count - 1);
						}
						hits.Add (vert);
						j += i < 0 ? 0.025f : -0.025f;
				}
				return hits;
		}
}
