using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileZ;
    public TileMap map;




    void OnMouseUp()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            if (map.activeUnit != null)
            {
                map.GeneratePathTo(tileX, tileZ);
            }

        }

        
    }

}
