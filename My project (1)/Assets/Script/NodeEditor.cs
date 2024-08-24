using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditor : MonoBehaviour
{
    public int X_Pos = 0;
    public int Y_Pos = 0;
    public bool isEnemy = false;
    public bool isStay = false;

    private GameSystem _gameSystem;
    [SerializeField] private List<NodeEditor> _prevNode = new List<NodeEditor>();
   [SerializeField] private NodeEditor prevNode = null;
    private bool isOnce = false;

    private void Awake()
    {
        _gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }

    private void Update()
    {
        if (_gameSystem.IsGameStart && _gameSystem.PlayerCount > 0 && !isOnce)
        {
            isOnce = true;
            for (int i = 0; i < _gameSystem.PlayerCount; i++)
            {
                _prevNode.Add(null);
            }
        }
    }

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

    public NodeEditor GetPrevNodeNum(int i)
    {
        return _prevNode[i];
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
    
    public void SetPrevNode(NodeEditor node, int i)
    {
        _prevNode[i] = node;
    }
}
