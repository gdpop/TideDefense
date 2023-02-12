using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.TilesetUtils
{
    public class TilesetUtils
    {
        public static List<Vector2Int> neighboorsCoordinatesEight = new List<Vector2Int>(8)
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
        };

		/// <summary> 
		///	Clockwise neighboor coordinates on a grid starting from the Top
		/// </summary>
        public static List<Vector2Int> neighboorsCoordinatesFour = new List<Vector2Int>(4)
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

        public static Dictionary<TilesetTypeFour, List<Vector2Int>> tilesetFortificationToLinkedDirection = new Dictionary<TilesetTypeFour, List<Vector2Int>>()
        {
            {TilesetTypeFour.Path_Right_Left, new List<Vector2Int>(){Vector2Int.right, Vector2Int.left}},
            {TilesetTypeFour.Path_Up_Down, new List<Vector2Int>(){Vector2Int.up, Vector2Int.down}},
        };

        /// <summary> 
        /// For a given direction is linked a list of compatible tileset
        /// For exemple : 
        /// The direction "right" is compatible with TilesetTypeFour.Path_Right_Left because there is a left connection
        /// Logic is "In & Out", as in for a given direction the tileset contains the opposite/meeting direction 
        /// </summary>
        public static Dictionary<Vector2Int, List<TilesetTypeFour>> directionToLinkableTilesetTypes = new Dictionary<Vector2Int, List<TilesetTypeFour>>()
        {
            {Vector2Int.right, new List<TilesetTypeFour>(){TilesetTypeFour.Path_Right_Left, TilesetTypeFour.Foursome}},
            {Vector2Int.left, new List<TilesetTypeFour>(){TilesetTypeFour.Path_Right_Left, TilesetTypeFour.Foursome}},
            {Vector2Int.up, new List<TilesetTypeFour>(){TilesetTypeFour.Path_Up_Down, TilesetTypeFour.Foursome}},
            {Vector2Int.down, new List<TilesetTypeFour>(){TilesetTypeFour.Path_Up_Down, TilesetTypeFour.Foursome}},
        };
    }
}
