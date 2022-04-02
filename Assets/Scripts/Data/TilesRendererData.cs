using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class TilesRendererData : ScriptableObject
{
    [field: SerializeField] public MeshFilter sandTileMeshFilter;
    [field: SerializeField] public MeshFilter moatTileMeshFilter;
    [field: SerializeField] public MeshFilter rampart1MeshFilter;
    [field: SerializeField] public MeshFilter rampart2MeshFilter;
}
