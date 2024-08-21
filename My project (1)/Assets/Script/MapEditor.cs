using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public int X_Horizontal = 0;
    public int Y_Vertical = 0;
    public GameObject Tile = null;
    public List<GameObject> TileMap = new List<GameObject>();

    private float _minus_Y = 0;
    private float _plus_Y = 0;
    private float _line_Y = 0;
    private short _input_X = 0;

    private void Awake()
    {
        _BaseInit();
    }

    private void Start()
    {
        if (TileMap.Count <= 0)
        {
            _BaseInit();
        }
    }

    private void _BaseInit()
    {
        if (X_Horizontal != 0 && Y_Vertical != 0)
        {
            for (int i = 0; i < Y_Vertical; i++)
            {
                if (i % 2 != 0 && i != 0)
                {
                    _plus_Y += 1;
                    _line_Y = _plus_Y;
                }
                else if (i % 2 == 0 && i != 0)
                {
                    _minus_Y -= 1;
                    _line_Y = _minus_Y;
                }
                else
                {
                    _line_Y = 0;
                }
                            
                for (int j = 0; j < X_Horizontal; j++)
                {
                   GameObject a = Instantiate(Tile, this.gameObject.transform);
                    a.transform.position = new Vector2( -(X_Horizontal / 2) + j, _line_Y);
                   
                    switch (_line_Y)
                    {
                        case 2:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 0);
                            break;
                        case 1:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 1);
                            break;
                        case 0:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 2);
                            break;
                        case -1:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 3);
                            break;
                        case -2:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 4);
                            break;
                    }

                    _input_X += 1;
                    TileMap.Add(a);
                }
                _input_X = 0;
            }
        }
    }

}
