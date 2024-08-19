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

    public void SetIsStayCheck(bool isCurrent)
    {
        isStay = isCurrent;
    }

    public void SetNodeCount(short X, short Y)
    {
        X_Pos = X;
        Y_Pos = Y;
    }

}
