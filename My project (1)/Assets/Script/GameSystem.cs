using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public List<GameObject> EnemyList = new List<GameObject>();
    public GameObject[] Enemy;
    public GameObject[] Player;
    public bool IsGameStart = false;
    public bool isPCheck = false;
    public bool isECheck = false;
    private int _PlayerCount = 0;
    private int _EnemyCount = 0;


    private void Awake()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
   
        _PlayerCount = Player.Length;
        _EnemyCount = Enemy.Length;
    }

    public void Update()
    {
        if (!IsGameStart)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length != _PlayerCount)
            {
                Player = GameObject.FindGameObjectsWithTag("Player");
                _PlayerCount = Player.Length;
                isPCheck = true;
            }
            else
            {
                if (isPCheck)
                    isPCheck = false;
            }

            if (GameObject.FindGameObjectsWithTag("Enemy").Length != _EnemyCount)
            {
                Enemy = GameObject.FindGameObjectsWithTag("Enemy");
                _EnemyCount = Enemy.Length;
                isECheck = true;
            }
            else
            {
                if (isECheck)
                    isECheck = false;
            }
        }
    }
}
