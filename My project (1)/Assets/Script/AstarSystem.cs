using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarSystem : MonoBehaviour
{
    public List<NodeEditor> Nodegroup = new List<NodeEditor>();

    private MapEditor _map;

    public void Awake()
    {
        if (Nodegroup.Count == 0 && _map.TileMap.Count > 0)
        {
            for (int i = 0; i < _map.TileMap.Count; i++)
            {
                Nodegroup.Add(_map.TileMap[i].GetComponent<NodeEditor>());
            }
        }   
    }


}
