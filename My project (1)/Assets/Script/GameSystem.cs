using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public UIManager Uimanager = null;
    public List<GameObject> EnemyList = new List<GameObject>();
    public GameObject[] Enemy;
    public GameObject[] Player;
    public bool IsGameStart = false;
    public bool IsGameTurnEnd = false;
    public bool IsGameEnd = false;
    public bool isPCheck = false;
    public bool isECheck = false;

    public int PlayerCount = 0;
    public int EnemyCount = 0;

    private float _turnTime = 0.0f;
    private float _GameTime = 0.0f;

    private void Awake()
    {
        if (Uimanager != null)
        {
            Uimanager = GameObject.Find("UImanager").GetComponent<UIManager>();
        }

        Player = GameObject.FindGameObjectsWithTag("Player");
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");

        PlayerCount = Player.Length;
        EnemyCount = Enemy.Length;
    }

  
    IEnumerator TimeIncrease() 
    {
        IsGameTurnEnd = true;
        yield return new WaitForSecondsRealtime(1f);
        IsGameTurnEnd = false;
    }

    public void Update()
    {
        if (!IsGameStart && !IsGameEnd)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length != PlayerCount)
            {
                Player = GameObject.FindGameObjectsWithTag("Player");
                PlayerCount = Player.Length;
                isPCheck = true;
            }
            else
            {
                if (isPCheck)
                    isPCheck = false;
            }

            if (GameObject.FindGameObjectsWithTag("Enemy").Length != EnemyCount)
            {
                Enemy = GameObject.FindGameObjectsWithTag("Enemy");
                EnemyCount = Enemy.Length;
                isECheck = true;
            }
            else
            {
                if (isECheck)
                    isECheck = false;
            }
        }
        else if(IsGameStart && !IsGameEnd)
        {
            if (_GameTime < 600f)
            {
                _GameTime += Time.deltaTime;

                if (!IsGameTurnEnd)
                {
                    if (_turnTime < 60f)
                    {
                        _turnTime += Time.deltaTime;
                    }
                    else
                    {
                        _turnTime = 0.0f;
                    }

                    IsGameTurnEnd = true;
                }
                else
                {
                    IsGameTurnEnd = false;
                }
            }
            else
            {
                IsGameStart = false;
                IsGameEnd = true;
                _GameTime = 0.0f;
            }
        }
    }

    public void StartAction()
    {
        _turnTime = 0.0f;
        _GameTime = 0.0f;
        IsGameEnd = false;
        IsGameStart = true;     
    }

    public void EndGame()
    {
        if (_GameTime < 60)
        {        
            IsGameEnd = true;
            IsGameStart = false;
            _GameTime = 0.0f;
        }

        if (EnemyCount > PlayerCount)
        {
            _Lose();
        }
        else if (EnemyCount < PlayerCount)
        {
            _Win();
        }
        else
        {
            _Draw();
        }
    }

    private void _Win()
    {

    }

    private void _Lose()
    {

    }

    private void _Draw()
    {

    }
}
