using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region SINGLETON
        private static GridManager instance = null;

        public static GridManager Instance {
            get {
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

    [SerializeField]
    private int _casualGridX;
    [SerializeField]
    private int _casualGridY;

    public TilesRendererData TilesRenderer;

    [HideInInspector]
    public Grid CurrentGrid;
    public GameObject PrefabTile;
    public void CreateGrid()
    {
        if (CurrentGrid == null)
        {
            CurrentGrid = new Grid();
            CurrentGrid.Generate(_casualGridX, _casualGridY);
        }
        else CurrentGrid.Generate(_casualGridX, _casualGridY);

        Debug.Log("grid created: x:" + CurrentGrid.XLenght + " // y:" + CurrentGrid.YLenght);
    }

}
