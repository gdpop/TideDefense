namespace VirtuoseReality.Utils.TransformTools
{
	using System;
	using UnityEngine;

	[Serializable]
    public class SphericalCoordinates
    {
        public Vector3 origin;

        public float radius;

        [Range(0f, 360f)]
        public float phi;

        [Range(-90f, 90f)]
        public float theta;

		public override string ToString()
		{
			string data = "";
			data += $"Origin :{origin}\r\n";
			data += $"radius :{radius}\r\n";
			data += $"phi :{phi}\r\n";
			data += $"theta :{theta}\r\n";
			return data;
		}
    }
}
