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
		///	Clockwise neighboor coordinates on a grid
		/// </summary>
        public static List<Vector2Int> neighboorsCoordinatesFour = new List<Vector2Int>(4)
        {
            new Vector2Int(0, 1), 	// North
            new Vector2Int(1, 0), 	// East
            new Vector2Int(0, -1),	// South
            new Vector2Int(-1, 0),  // West
        };
    }
}
