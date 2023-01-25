using System.Collections;
using System.Collections.Generic;
using ToolBox.Pools;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
#region Name
    [SerializeField]
    private GameObject _foundationPrefab = null;

    private GameObject _usedFoundationPrefab = null;

    private void InitializeRempartFoundation()
    {
        _foundationPrefab.gameObject.Populate(12);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_usedFoundationPrefab == null)
                _usedFoundationPrefab = _foundationPrefab.Reuse(
                    transform.position,
                    transform.rotation
                );
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            if(_usedFoundationPrefab != null)
            {
                _usedFoundationPrefab.Release();
                _usedFoundationPrefab = null;
            }
                
        }

    }

#endregion
}
