using System;
using UnityEngine;

namespace TideDefense
{
    [Flags]
    public enum BeachToolType
    {
        None = 1,

        Mould = 2,
        Primary = 4,
        Shovel = 8,

        Path = 16,
        Corner = 32,
        Triple = 64,
        Foursome = 128,

        Bucket = Mould | Primary,

        RempartMoldPath = Path | Mould,
        RempartMoldCorner = Corner | Mould,
        RempartMoldTriple = Triple | Mould,
        RempartMoldFoursome = Foursome | Mould
    }
}
