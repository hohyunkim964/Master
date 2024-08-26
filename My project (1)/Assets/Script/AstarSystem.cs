using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarSystem : MonoBehaviour
{
    public List<NodeEditor> Nodegroup = new List<NodeEditor>();


    private MapEditor _map;
    private NodeEditor _node;
    private NodeEditor _insertNode;

    public void Awake()
    {
        if (_map == null)
            _map = GameObject.Find("MapEditor").GetComponent<MapEditor>();

    }

    private void Start()
    {
        if (Nodegroup.Count == 0 && _map.TileMap.Count > 0)
        {
            for (int i = 0; i < _map.TileMap.Count; i++)
            {
                Nodegroup.Add(_map.TileMap[i].GetComponent<NodeEditor>());
            }
        }        
    }

    public void FindEnemy(int posX, int posY) 
    {
        int minimumX = int.MaxValue;
        int minimumY = int.MaxValue;
        for (int i = 0; i < _map.Enemy_Pos.Count; i++) 
        {
            if ((_map.Enemy_Pos[i]._curPos_X - posX) < minimumX) 
            {
                minimumX = _map.Enemy_Pos[i]._curPos_X;
            }

            if ((_map.Enemy_Pos[i]._curPos_Y - posY) < minimumY)
            {
                minimumY = _map.Enemy_Pos[i]._curPos_Y;
            }
        }

    }
    private bool isCheck = false;

    public void GetDirect(NodeEditor Node, UnitObj unit, int y)
    {
        if (Nodegroup.Count > 0)
        {
            for (int i = 0; i < Nodegroup.Count; i++)
            {
                if (Nodegroup[i] == Node)
                {
                    _node = Node;
                    break;
                }
            }
        }

        if (unit.NodeDirction.Count > 0) 
        {
            unit.NodeDirction.Clear();
        }

        //상하좌우 방향 가져오기
        for (int i = 0; i < Nodegroup.Count; i++)
        {
            if ((_node.GetNodeCountY() + 1) == Nodegroup[i].GetNodeCountY() && _node.GetNodeCountX() == Nodegroup[i].GetNodeCountX())
            {
         
                //위에 방향
                unit.NodeDirction.Add(Nodegroup[i]);
            }
            if ((_node.GetNodeCountX() + 1) == Nodegroup[i].GetNodeCountX() && _node.GetNodeCountY() == Nodegroup[i].GetNodeCountY())
            {
                //오른쪽에 방향
                unit.NodeDirction.Add(Nodegroup[i]);
            }

            if ((_node.GetNodeCountY() - 1) == Nodegroup[i].GetNodeCountY() && _node.GetNodeCountX() == Nodegroup[i].GetNodeCountX())
            {
                //아래에 방향
                unit.NodeDirction.Add(Nodegroup[i]);
            }

            if ((_node.GetNodeCountX() - 1) == Nodegroup[i].GetNodeCountX() && _node.GetNodeCountY() == Nodegroup[i].GetNodeCountY())
            {
                //왼쪽에 방향
                unit.NodeDirction.Add(Nodegroup[i]);
            }
          
        }

     

        if (unit.CloseList.Count > 0)
        {
            for (int j = 0; j < unit.CloseList.Count; j++)
            {
                for (int x = 0; x < unit.NodeDirction.Count; x++)
                {
                    unit.isNotFoundPath = false;
                    if ((unit.CloseList[j] != unit.NodeDirction[x] && !unit.NodeDirction[x].GetIsStayCheck()))
                    {
                        if (unit.NodeDirction[x].GetPrevNodeNum(y) == null && unit.NodeDirction[x] != unit._StartNode)
                        {
                            unit.NodeDirction[x].SetPrevNode(_node, y);
                        }

                        isCheck = true;

                        for (int i = 0; i < unit.OpenList.Count; i++)
                        {
                            if (unit.OpenList[i].gameObject == unit.NodeDirction[x].gameObject)
                            {
                                isCheck = false;
                                break;
                            }
                        }

                        if (isCheck)
                        {
                            unit.OpenList.Add(unit.NodeDirction[x]);
                        }
                    }                  
                    else if (unit.CloseList[j] != unit.NodeDirction[x] && unit.NodeDirction[x].GetIsStayCheck() && unit.NodeDirction[x].GetIsEnemyCheck())
                    {
                        //   Debug.Log(unit.NodeDirction[x].X_Pos + "            " + unit.NodeDirction[x].Y_Pos);
                        unit._node = _node;
                        unit.isFindPath = true;
                        return;
                    }
                    else
                    {
                        unit.isNotFoundPath = true;
                    }
                }
            }
        }
        else
        {          
            for (int x = 0; x < unit.NodeDirction.Count; x++)
            {
                if (!unit.NodeDirction[x].GetIsStayCheck() && unit.NodeDirction[x] != unit._StartNode)
                {
                    if (unit.NodeDirction[x].GetPrevNodeNum(y) == null)
                    {
                        unit.NodeDirction[x].SetPrevNode(_node, y);
                    }

                    unit.OpenList.Add(unit.NodeDirction[x]);
                }             
                else if (unit.NodeDirction[x].GetIsEnemyCheck() && unit.NodeDirction[x].GetIsStayCheck())
                {
                    return;
                }
            }
        }


        if (unit.OpenList.Count > 0 && unit.NodeDirction.Count > 1)
        {
            if (!unit.isNotFoundPath)
            {
                unit.CloseList.Add(_node);
            }

            if (unit.OpenList.Count > 1)
            {
                for (int i = 0; i < unit.OpenList.Count; i++)
                {
                    if (_node == unit.OpenList[i])
                    {
                        unit.OpenList.RemoveAt(i);
                    }
                }
               
            }
        }

    }

    public void GetFindPath(NodeEditor Node)
    {
       // _insertNode = Node;
       //
       // while (_insertNode.GetPrevNode() != null)
       // {
       //     Path.Add(_insertNode);
       //     _insertNode = _insertNode.GetPrevNode();
       // }
    }

    public void Directinfo(UnitObj unit) 
    {
    }




    public void AstarOBj(Vector2 _pos) 
    {

    }


}
