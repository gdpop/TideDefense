using System;
using UnityEngine;

namespace TideDefense
{
    [Flags]
    public enum BeachToolType
    {
        None = 1,

        Container = 2,
        Primary = 4,
        Shovel = 8,

        Path = 16,
        Corner = 32,
        Triple = 64,
        Foursome = 128,

        Bucket = Container | Primary,

        RempartMoldPath = Path | Container,
        RempartMoldCorner = Corner | Container,
        RempartMoldTriple = Triple | Container,
        RempartMoldFoursome = Foursome | Container
    }
}
