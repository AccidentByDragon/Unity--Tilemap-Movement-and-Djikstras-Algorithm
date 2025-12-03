using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    

    public int unitTileX;
    public int unitTileZ;
    public TileMap map;
    public Vector3 cPos;
    public Vector3 tPos;
    public GameController cG;
    public float unitSpeed;
    public float MovementSpeed;
    public bool CanMove;
    public int UnitID;

    public List<TileMap.Node> currentPath = null;

    private void Update()
    {
        DrawLine();
        if (cG == null)
        {
            cG = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }
    /*public void MoveNextile()
    {
        float remainingMovement = unitSpeed;
        while (remainingMovement > 0)
        {
            if (currentPath == null)
            {
                Debug.Log("WTF!");
                return;
            }
            
            unitTileX = currentPath[1].x;
            unitTileZ = currentPath[1].z;

            cPos = new Vector3(currentPath[0].x, gameObject.transform.position.y, currentPath[0].z);
            tPos = new Vector3(unitTileX, 0, unitTileZ);
            if (Vector3.Distance(cPos, tPos) > 0.1)
            {
                transform.position = Vector3.MoveTowards(cPos, tPos, MovementSpeed);
            }
            remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].z, currentPath[1].x, currentPath[1].z);

            currentPath.RemoveAt(0);
            if (currentPath.Count == 1)
            {
                currentPath = null;
            }
        }

    }*/
    private void DrawLine()
    {
        if ((currentPath != null) && (map != null))
        {
            int currNode = 0;

            while (currNode < currentPath.Count - 1)
            {
                Vector3 start = map.tileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].z) + new Vector3(0, 2f, 0);
                Vector3 end = map.tileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].z) + new Vector3(0, 2f, 0);

                Debug.DrawLine(start, end, Color.red);

                currNode++;
            }
        }
    }
    private void UnitMover()
    {
        if (currentPath != null && CanMove == true)
        {
            if (Vector3.Distance(cPos, tPos) > 0.1)
            {
                cPos = new Vector3(unitTileX, 0, unitTileZ);                
                transform.position = Vector3.MoveTowards(cPos, tPos, MovementSpeed);
                
            }
            else
            {
                currentPath.RemoveAt(0);
                if (currentPath.Count == 1)
                {
                    currentPath = null;
                }
                else
                {
                    unitTileX = currentPath[0].x;
                    unitTileZ = currentPath[0].z;

                    cPos = new Vector3(unitTileX, 0, unitTileZ);
                    tPos = new Vector3(currentPath[1].x, 0, currentPath[1].z);
                }



            }
        }
    }
    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            if (cG.ActivatedUnit == GetComponent<Unit>().gameObject)
            {
                cG.ActivatedUnit = null;
                cG.UnitIsDeactivated();
            }
            else if (cG.ActivatedUnit == null)
                {
                    Debug.Log("Hello" + cG.name);
                    cG.ActivatedUnit = GetComponent<Unit>().gameObject;
                    cG.UnitIsActivated();
                }
                else
                {
                    // put in a  piece of code to inform player that a unit is already selected
                }
            }
        }
    }


