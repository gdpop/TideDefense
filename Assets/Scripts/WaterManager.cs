using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterManager : MonoBehaviour
{
    GameObject _tile;

    GameObject[][] grid;
    void Start()
    {
        grid = new GameObject[20][];
        StartCoroutine(Tic());
    }

    IEnumerator Tic()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
