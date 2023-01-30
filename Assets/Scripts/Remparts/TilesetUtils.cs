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
            new Vector2Int(0, 1), 	// Top
            new Vector2Int(1, 0), 	// Right
            new Vector2Int(0, -1),	// Bot
            new Vector2Int(-1, 0),  // Left
        };

        /// <summary> 
		///	Neighboord coordinates from a rotation on a trigonometric circle
		/// </summary>
        public static List<Vector2Int> trigNeighboorsCoordinatesFour = new List<Vector2Int>(4)
        {
            new Vector2Int(1, 0), 	// 0°
            new Vector2Int(0, 1), 	// 90°
            new Vector2Int(-1, 0),  // 180°
            new Vector2Int(0, -1),	// 270°
        };
    }
}
