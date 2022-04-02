using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class TilesRendererData : ScriptableObject
{
    [field: SerializeField] public GameObject sandTileVisual;
    [field: SerializeField] public GameObject moatTileVisual;
    [field: SerializeField] public GameObject rampart1Visual;
    [field: SerializeField] public GameObject rampart2Visual;
}
