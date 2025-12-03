using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public TileMap gcMap; 
    public GameObject ActivatedUnit;
    public List<GameObject> ActiveUnits;
    public List<GameObject> UnActiveUnits;
    public List<GameObject> Spawntiles;
    private float numberOfunitsSpawned;
    private GameObject SpawnedUnit;

    private void Start()
    {
        
    }
    public void UnitIsActivated()
    {
        Debug.Log(ActivatedUnit.name);
        if (gcMap == null)
        {
            gcMap = GameObject.Find("Map").GetComponent<TileMap>();
        }
        gcMap.activeUnit = ActivatedUnit;
        gcMap.UnitCoordinates();
        ActivatedUnit.GetComponent<Unit>().map = gcMap;
        ActivatedUnit.GetComponent<Unit>().CanMove = true;
        Debug.Log("Hi");

    }

    public void UnitIsDeactivated()
    {
        Debug.Log("a unit has been deselcted");
        if (gcMap == null)
        {
            gcMap = GameObject.Find("Map").GetComponent<TileMap>();
        }
        gcMap.activeUnit = ActivatedUnit;
    }

    public void UnitMovement()
    {

    }

    public void SpawnUnits()
    {
        numberOfunitsSpawned = 10;
        ShuffleSpawnPoints();

    }

    private void ShuffleSpawnPoints() // Fisher-Yates Shuffle Algorithm
    {
        for (int i = 0; i < Spawntiles.Count; i++)
        {
            GameObject temp = Spawntiles[i];
            int randomIndex = Random.Range(i, Spawntiles.Count);
            Spawntiles[i] = Spawntiles[randomIndex];
            Spawntiles[randomIndex] = temp;
        }
    }
}
