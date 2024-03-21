using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationParams
{
    public int Seed;
    public bool UseSeed;

    public int MaxRoomsApprox = 15;
    public int MinRooms = 5;

    public int UpperBound = 1;
    public int LowerBound = 1;
    public int LeftBound = 1;
    public int RightBound = 1;

    public Room StartRoom;
}
