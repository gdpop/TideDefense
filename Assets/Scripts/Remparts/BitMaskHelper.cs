//
//   BitMaskHelper.cs
//
//   Created by Hadrien ESTELA on 18/06/2020
//   Copyright © 2020 Virtuose Reality. All rights reserved.
//

namespace VirtuoseReality.Helpers
{

	public static class BitMaskHelper
	{

		public static int SetBit ( int mask, byte bit )
		{
			return (mask | (1 << bit) );
		}

		public static int UnsetBit ( int mask, byte bit )
		{
			return (mask & ~(1 << bit) );
		}

		public static int ToggleBit ( int mask, byte bit )
		{
			return (mask ^ (1 << bit) );
		}

		public static bool CheckBit ( int mask, byte bit )
		{
			return ( (mask >> bit) & 1) > 0;
		}

		public static int SetMask ( int mask, int maskValue )
		{
			return (mask | maskValue);
		}

		public static int UnsetMask ( int mask, int maskValue )
		{
			return (mask & ~maskValue);
		}

		public static bool CheckMask ( int mask, int maskValue )
		{
			return (mask & maskValue) > 0;
		}

	}

}
