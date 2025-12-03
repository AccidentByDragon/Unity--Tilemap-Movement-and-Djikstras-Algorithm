using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;


public class TileMap : MonoBehaviour
{

    public TileType[] tileTypes;
    public GameObject activeUnit;
    public GameController gaCo;

    private int[,] tiles;
    Node[,] graph;


    private int tileSize = 16;
    [SerializeField]
    private int mapSizeX = 10;
    [SerializeField]
    private int mapSizeZ = 10;
    [SerializeField]
    private int maxNrBlockerTiles;

    private int numberOfBlockerTiles;
    void Start()
    {
        gaCo = GameObject.Find("GameController").GetComponent<GameController>();
        tiles = new int[mapSizeX, mapSizeZ];
        InitialiseMap();
        GeneratePathfindingGraph();
        GenerateMapVisuals();


    }
    public void UnitCoordinates()
    {
        activeUnit.GetComponent<Unit>().unitTileX = (int)activeUnit.transform.position.x;
        activeUnit.GetComponent<Unit>().unitTileZ = (int)activeUnit.transform.position.z;
        activeUnit.GetComponent<Unit>().map = this;
    }

    private void InitialiseMap()
    {
        //Intialise the map tiles to be grass
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                if (z < 4 || z > mapSizeZ - 4)
                {
                    tiles[x, z] = 1;
                }
                else
                {
                    if (numberOfBlockerTiles < maxNrBlockerTiles)
                    {
                        tiles[x, z] = Random.Range(0, 3);
                        if (tiles[x, z] == 2)
                        {
                            numberOfBlockerTiles = numberOfBlockerTiles + 1;
                        }
                    }
                    else
                    {
                        tiles[x, z] = 0;
                    }
                }
            }
        }

    }
    public float CostToEnterTile(int sourceX, int sourceZ, int targetX, int targetZ)
    { 
        TileType tt = tileTypes[tiles[targetX, targetZ]];

        float cost = tt.movementCost;
        if (sourceX != targetX && sourceZ != targetZ)
        {
            cost += 0.001f;
        }

        return cost;
    }
    public class Node
    {
        public List<Node> neighbours;
        public int x;
        public int z;

        public Node()
        {
            neighbours = new List<Node>();
        }

        public float DistanceTo(Node n)
        {
            return Vector3.Distance(
                new Vector3(x, 0, z),
                new Vector3(n.x, 0, n.z));
        }
    }


    void GeneratePathfindingGraph()
    {
        graph = new Node[mapSizeX, mapSizeZ];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                graph[x, z] = new Node();

                graph[x, z].x = x;
                graph[x, z].z = z;
            }
        }
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                /*if (x > 0)
                {
                graph[x, z].neighbours.Add(graph[x - 1, z]);
                }
                if ( x < mapSizeX - 1)
                {
                    graph[x, z].neighbours.Add(graph[x + 1, z]);
                }
                if (z > 0)
                {
                    graph[x, z].neighbours.Add(graph[x,z - 1]);
                }
                if (z < mapSizeZ - 1)
                {
                    graph[x, z].neighbours.Add(graph[x, z + 1]);
                }*/
                // eight way version
                if (x > 0)
                {
                    graph[x, z].neighbours.Add(graph[x - 1, z]);
                    if (z > 0)
                    {
                        graph[x, z].neighbours.Add(graph[x - 1, z - 1]);
                    }
                    if (z < mapSizeZ - 1)
                    {
                        graph[x, z].neighbours.Add(graph[x - 1, z + 1]);

                    }
                }
                if (x < mapSizeX - 1)
                {
                    graph[x, z].neighbours.Add(graph[x + 1, z]);
                    if (z > 0)
                    {
                        graph[x, z].neighbours.Add(graph[x + 1, z - 1]);
                    }
                    if (z < mapSizeZ - 1)
                    {
                        graph[x, z].neighbours.Add(graph[x + 1, z + 1]);

                    }
                }
                if (z > 0)
                {
                    graph[x, z].neighbours.Add(graph[x, z - 1]);
                }
                if (z < mapSizeZ - 1)
                {
                    graph[x, z].neighbours.Add(graph[x, z + 1]);

                }
            }
        }
    }

    private void GenerateMapVisuals()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                TileType tt = tileTypes[tiles[x, z]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x*tileSize, 0, z*tileSize), Quaternion.Euler(-90,0,0));
                if(tiles[x,z] == 1)
                {
                    gaCo.Spawntiles.Add(go);
                }
                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileZ = z;
                ct.map = this;
            }
        }
    }
    public Vector3 tileCoordToWorldCoord(int x, int z)
    {
        return new Vector3(x*tileSize, 0, z*tileSize);
    }

    public void GeneratePathTo(int x, int z)
    {
        activeUnit.GetComponent<Unit>().currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        // This is "Q" for Dijkstra's Algorithm which represents unchecked nodes
        List<Node> unvisited = new List<Node>();

        Node source = graph[
            activeUnit.GetComponent<Unit>().unitTileX,
            activeUnit.GetComponent<Unit>().unitTileZ
            ];
        Node target = graph[
            x,
            z
            ];
        dist[source] = 0;
        prev[source] = null;

        /* initialise everything to have infinity distance,
         *  since we don't know any better and 
         *  because some nodes might be impossible to reach from the source
         making infinty a valid result*/
        foreach(Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;

            }

            unvisited.Add(v);
        }
        
        while(unvisited.Count > 0)
        {
            // u is going to be the u woth the smallest distance.
            Node u = null;

            foreach(Node possibleU in unvisited)
            {
                if( u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if ( u == target)
            {
                break; // exit the while loop
            }

            unvisited.Remove(u);

            foreach(Node v in u.neighbours)
            {
                float alt = dist[u] + CostToEnterTile(u.x, u.z, v.x, v.z);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        if (prev[target] == null)
        {
            // no route between target and source
            return;
        }

        List<Node> currentPath = new List<Node>();
        Node currNode = target;
         // adds prevs to current path
        while(currNode != null)
        {
            currentPath.Add(currNode);
            currNode = prev[currNode];
        }
        // currentPath now describes a route to source from target

        currentPath.Reverse();

        activeUnit.GetComponent<Unit>().currentPath = currentPath;
    }

}
