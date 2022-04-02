using System;
using UnityEngine;

[Serializable]
public struct RempartBlock
{
	public TilesetType type;
	public GameObject mesh;

	public Mesh meshMesh
	{
		get
		{
			return mesh.GetComponent<MeshFilter>().sharedMesh;
		}
	}


}