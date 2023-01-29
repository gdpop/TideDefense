using System;
using UnityEngine;

namespace TideDefense
{
    [Flags]
    public enum BeachToolType
    {
        None = 0,

        Container = 1,
        Primary = 2,
        Shovel = 4,

        Path = 8,
        Corner = 16,
        Triple = 32,
        Foursome = 64,

        Bucket = Container | Primary,

        RempartMoldPath = Path | Container,
        RempartMoldCorner = Corner | Container,
        RempartMoldTriple = Triple | Container,
        RempartMoldFoursome = Foursome | Container
    }
}
