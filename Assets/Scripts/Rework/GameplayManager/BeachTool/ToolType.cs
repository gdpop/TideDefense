using System;
using UnityEngine;

namespace TideDefense
{
    [Flags]
    public enum BeachToolType
    {
        None = 1,

        Mold = 2,
        Primary = 4,
        Shovel = 8,

        Path = 16,
        Corner = 32,
        Triple = 64,
        Foursome = 128,

        Bucket = Mold | Primary,

        RempartMoldPath = Path | Mold,
        RempartMoldCorner = Corner | Mold,
        RempartMoldTriple = Triple | Mold,
        RempartMoldFoursome = Foursome | Mold
    }
}
