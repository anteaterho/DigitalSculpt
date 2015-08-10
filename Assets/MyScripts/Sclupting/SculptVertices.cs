using UnityEngine;
using System.Collections;

public class SculptVertices : MonoBehaviour {
	
	public float radius = 1.0f;
	public float pull = 10.0f;
	
	float distance;
	float falloff;
	float sqrMagnitude;

	private int pullDirection = 1;

	private bool isHit;
	public bool IsHit
	{
		get{
			return isHit;
		}
	}

	private Vector3 mousePos;
	public Vector3 MousePos
	{
		get{
			return mousePos;
		}
	}
	
	private MeshFilter unappliedMesh;
	
	public enum FallOff{
		Gauss, Linear, Neadle
	}

	public FallOff fallOff = FallOff.Gauss;


	float LinearFalloff(float distance, float inRadius)
	{
		return Mathf.Clamp01 (1.0f - distance / inRadius);
	}
	
	float GaussFalloff(float distance, float inRadius)
	{
		return Mathf.Clamp01 (Mathf.Pow (360.0f, - Mathf.Pow (distance / inRadius, 2.5f) - 0.01f));
	}
	
	float NeedleFalloff(float dist, float inRadius)
	{
		return -(dist * dist) / (inRadius * inRadius) + 1.0f;
	}
	
	void DeformMesh(Mesh mesh, Vector3 position, float power, float inRadius)
	{
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		float sqrRadius = inRadius * inRadius;
		
		//Calculate averaged normal of all surrounding vertices
		Vector3 averageNormal = Vector3.zero;
		
		for (int i = 0; i < vertices.Length; i++) {
			
			sqrMagnitude = (vertices[i] - position).sqrMagnitude;
			
			if(sqrMagnitude > sqrRadius)
				continue;
			distance = Mathf.Sqrt(sqrMagnitude);
			falloff = LinearFalloff(distance, inRadius);
			averageNormal += falloff * normals[i];
		}
		averageNormal = averageNormal.normalized;
		
		for (int i = 0; i < vertices.Length; i++) {
			
			sqrMagnitude = (vertices[i] - position).sqrMagnitude;
			if(sqrMagnitude > sqrRadius)
				continue;
			distance = Mathf.Sqrt(sqrMagnitude);
			
			switch(fallOff)
			{
			case FallOff.Gauss:
				falloff = GaussFalloff(distance, inRadius);
				break;
			case FallOff.Neadle:
				falloff = NeedleFalloff(distance, inRadius);
				break;
			default:
				falloff = LinearFalloff(distance, inRadius);
				break;
			}
			vertices[i] += averageNormal * falloff * power;
		}
		
		mesh.vertices = vertices;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
	}


	void Update()
	{
		if (Input.GetKey (KeyCode.LeftAlt)) {
			pullDirection = -1;
		} else {
			pullDirection = 1;
		}

		if (!Input.GetMouseButton (0)) {
			ApplyMeshCollier();
			return;
		}
		
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			isHit = true;
			mousePos = hit.point + hit.normal;

			MeshFilter filter = hit.collider.GetComponent<MeshFilter> ();
			if (filter) {
				if (filter != unappliedMesh) {
					ApplyMeshCollier ();
					unappliedMesh = filter;
				}

				Vector3 relativePoint = filter.transform.InverseTransformPoint (hit.point);
				DeformMesh (filter.mesh, relativePoint, (pull * pullDirection) * Time.deltaTime, radius);
			}
		} else {
			isHit = false;
		}
	}
	
	void ApplyMeshCollier()
	{
		if (unappliedMesh && unappliedMesh.GetComponent<MeshCollider> ()) {
			unappliedMesh.GetComponent<MeshCollider>().sharedMesh = unappliedMesh.mesh;
		}
		unappliedMesh = null;
	}
}
