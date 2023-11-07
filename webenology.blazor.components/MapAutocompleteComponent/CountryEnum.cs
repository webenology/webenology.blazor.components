using System;

namespace webenology.blazor.components;

[Flags]
public enum CountryEnum : short
{
    NONE =0,
    USA = 1,
    CAN = 2,
    MEX = 4
}