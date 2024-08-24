using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditor : MonoBehaviour
{
    public bool isStay = false;
    public int X_Pos = 0;
    public int Y_Pos = 0;

    public bool isEnemy = false;

   [SerializeField] private NodeEditor prevNode = null;


    public bool GetIsStayCheck()
    {
        return isStay;
    }

    public bool GetIsEnemyCheck()
    {
        return isEnemy;
    }

    public int GetNodeCountX()
    {
        return X_Pos;
    }

    public int GetNodeCountY()
    {
        return Y_Pos;
    }

    public NodeEditor GetPrevNode()
    {
        return prevNode;
    }

    public void SetIsStayCheck(bool _isCurrent)
    {
        isStay = _isCurrent;
    }

    public void SetNodeCount(short _X, short _Y)
    {
        X_Pos = _X;
        Y_Pos = _Y;
    }
    public void SetisEnemyCheck(bool _isCurrent)
    {
        isEnemy = _isCurrent;
    }
    public void SetNodeInfo(NodeEditor node) 
    {
        prevNode = node;
    }
}
