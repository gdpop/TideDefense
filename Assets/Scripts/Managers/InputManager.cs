using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region SINGLETON
    private static InputManager instance = null;

    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }
    #region [ MONOBEHAVIOR ]
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
    #endregion
    #endregion

    private Tile hoveredTile;
    private Tile clickedTile;
    void Start()
    {

    }

    void Update()
    {
        DetectHover();
        DetectClick();
    }

    private void DetectHover()
    {
        //Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 6;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {

            Tile tile = hit.transform.transform.parent.GetComponent<Tile>();

            if (tile == null)
            {
                if (hoveredTile != null) hoveredTile.OnHover(false);
                return;
            }
            if (hoveredTile != null && tile != hoveredTile) hoveredTile.OnHover(false);
            hoveredTile = tile;
            tile.OnHover(true);
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
        }
        else
        {
            if (hoveredTile != null) hoveredTile.OnHover(false);
            hoveredTile = null;
        }


    }

    private void DetectClick()
    {
        if (Input.GetMouseButtonDown(0) && hoveredTile != null)
        {
            clickedTile = hoveredTile;
            clickedTile.OnClick(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (clickedTile != null) clickedTile.OnClick(false);
            clickedTile = null;
        }
    }
}
