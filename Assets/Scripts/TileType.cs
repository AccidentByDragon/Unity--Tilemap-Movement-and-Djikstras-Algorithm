using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType
{

    [SerializeField]
    private string name;

    public float movementCost = 1;

    public GameObject tileVisualPrefab;

}
