using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]
public class ProceduralQuad : MonoBehaviour {
	
	
	public float length;
	public float width;
	public float height;
	
	public int SegmentCount; 

	public float timer = 0;
	public float addToTimer =0.5f;
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		timer-=Time.deltaTime;
		if(timer <0)
		{

			BuildMesh ();
			timer +=addToTimer;
		}

	}

	public void BuildMesh()
	{
		MeshBuilder meshBuilder = new MeshBuilder();

		for (int i = 0; i <= SegmentCount; i++)
		{
			float z = length * i;
			float v = (1.0f / SegmentCount) * i;
			
			for (int j = 0; j <= SegmentCount; j++)
			{
				float x = width * j;
				float u = (1.0f / SegmentCount) *j;
				
				Vector3 offset = new Vector3(x, Random.Range(0.0f, height), z);
				
				Vector2 uv = new Vector2(u,v);
				bool buildTriangles = i > 0 && j > 0;
				
				BuildQuadForGrid(meshBuilder, offset, uv, buildTriangles, SegmentCount +1);
				
			} 	
		}

		Mesh mesh = meshBuilder.CreateMesh ();
		mesh.RecalculateNormals();

		MeshFilter filter = GetComponent<MeshFilter>();
		
		if (filter != null)
		{
			//Destroy the mesh, otherwise it will constantly add new meshes and not know to delete them, thus increasing RAM usage.
			//Possibly use this to destroy the temporary texture in project
			Destroy(filter.sharedMesh);
			filter.sharedMesh = mesh;
			//GetComponent<MeshCollider>().sharedMesh = null;
			//GetComponent<MeshCollider>().sharedMesh = filter.mesh;
		}
	}
	
	public void BuildPlane()
	{		
		MeshBuilder meshBuilder = new MeshBuilder();
		
		//Set up the vertices and triangles:
		meshBuilder.Vertices.Add(new Vector3(0.0f,0.0f,0.0f));
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(0.0f,0.0f,length));
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(width,0.0f,length));
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(width,0.0f,0.0f));
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.AddTriangle(0,1,2);
		meshBuilder.AddTriangle(0,2,3);
		
		//Create the mesh:
		
		MeshFilter filter = GetComponent<MeshFilter>();
		
		if(filter != null)
		{
			filter.sharedMesh = meshBuilder.CreateMesh();
			

		}
	}
	
	public void BuildQuad(MeshBuilder meshBuilder, Vector3 offset)
	{
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, length) + offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(width, 0.0f, length) + offset);
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(width, 0.0f, 0.0f) + offset);
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		int baseIndex = meshBuilder.Vertices.Count - 4;
		
		meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
		
		MeshFilter filter = GetComponent<MeshFilter>();

		if(filter != null)
		{

			filter.sharedMesh = meshBuilder.CreateMesh();

		}
		
	}

	public void BuildQuadForGrid(MeshBuilder meshBuilder, Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow)
	{
		meshBuilder.Vertices.Add (position);
		meshBuilder.UVs.Add (uv);

		if (buildTriangles) 
		{
			int baseIndex = meshBuilder.Vertices.Count - 1;

			int index0 = baseIndex;
			int index1 = baseIndex - 1;
			int index2 = baseIndex - vertsPerRow;
			int index3 = baseIndex - vertsPerRow - 1;

			meshBuilder.AddTriangle(index0,index2,index1);
			meshBuilder.AddTriangle(index2, index3, index1);
		}
	}
}