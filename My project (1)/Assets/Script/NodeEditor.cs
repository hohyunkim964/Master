using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditor : MonoBehaviour
{
    public bool isStay = false;
    public short X_Pos = 0;
    public short Y_Pos = 0;

    public bool GetIsStayCheck()
    {
        return isStay;
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
}
