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


    public Grid CurrentGrid;
    public GameObject PrefabTile;
    public void CreateGrid(int xLength, int yLength)
    {
        if (CurrentGrid == null)
        {
            CurrentGrid = new Grid();
        }
        else CurrentGrid.Generate(xLength, yLength);

        Debug.Log("grid created: x:" + CurrentGrid.XLenght + " // y:" + CurrentGrid.YLenght);
    }
}
