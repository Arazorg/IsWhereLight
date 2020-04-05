using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class Testing : MonoBehaviour
{

    private Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(100, 100);
    }
}
