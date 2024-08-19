using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public int X_Horizontal = 0;
    public int Y_Vertical = 0;
    public GameObject Tile = null;

    public List<GameObject> TileMap = new List<GameObject>();

    private int minus_y = 0;
    private int plus_y = 0;
    private int line_y = 0;
    private short input_x = 0;

    private void Awake()
    {
        BaseInit();
    }

    private void Start()
    {
        if (TileMap.Count <= 0)
        {
            BaseInit();
        }
    }

    private void BaseInit()
    {
        if (X_Horizontal != 0 && Y_Vertical != 0)
        {
            for (int i = 0; i < Y_Vertical; i++)
            {
                if (i % 2 != 0 && i != 0)
                {
                    plus_y += 1;
                    line_y = plus_y;
                }
                else if (i % 2 == 0 && i != 0)
                {
                    minus_y -= 1;
                    line_y = minus_y;
                }
                else
                {
                    line_y = 0;
                }
                            
                for (int j = 0; j < X_Horizontal; j++)
                {
                   GameObject a = Instantiate(Tile, this.gameObject.transform);
                    a.transform.position = new Vector2( -(X_Horizontal / 2) + j, line_y);
                   
                    switch (line_y)
                    {
                        case 2:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(input_x, 0);
                            break;
                        case 1:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(input_x, 1);
                            break;
                        case 0:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(input_x, 2);
                            break;
                        case -1:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(input_x, 3);
                            break;
                        case -2:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(input_x, 4);
                            break;
                    }

                    input_x += 1;
                    TileMap.Add(a);
                }
                input_x = 0;
            }
        }
    }

}
