using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public UIManager Uimanager = null;
    public List<GameObject> EnemyList = new List<GameObject>();
    public GameObject Map = null;
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
    private int _isCheckEnemyNum = 0;
    private int _isCheckPlayerNum = 0;
    private void Awake()
    {
        if (Uimanager != null)
        {
            Uimanager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }

        Player = GameObject.FindGameObjectsWithTag("Player");
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        Map = GameObject.Find("MapEditor").gameObject;
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
                _isCheckPlayerNum = 0;
                _isCheckEnemyNum = 0;
                for (int i = 0; i < Enemy.Length; i++) 
                {
                    if (!Enemy[i].GetComponent<UnitObj>()._isDie) 
                    {
                        _isCheckEnemyNum++;
                    }
                }

                for (int i = 0; i < Player.Length; i++)
                {
                    if (!Player[i].GetComponent<UnitObj>()._isDie)
                    {
                        _isCheckPlayerNum++;                       
                    }
                }

                if (_isCheckPlayerNum != PlayerCount) 
                {
                    PlayerCount = _isCheckPlayerNum;
                }
                if (_isCheckEnemyNum != EnemyCount)
                {
                    EnemyCount = _isCheckEnemyNum;
                }

                if (PlayerCount <= 0)
                {
                    _Lose();
                }
                else if(EnemyCount <= 0)
                {
                    _Win();
                }

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
        for (int i = 0; i < Player.Length; i++)
        {
            Player[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < Enemy.Length; i++)
        {
            Enemy[i].gameObject.SetActive(false);
        }
        Map.SetActive(false);
        Uimanager.WinPanel.SetActive(true);
    }

    private void _Lose()
    {
        for (int i = 0; i < Player.Length; i++)
        {
            Player[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < Enemy.Length; i++)
        {
            Enemy[i].gameObject.SetActive(false);
        }
        Map.SetActive(false);
        Uimanager.LosePanel.SetActive(true);
    }

    private void _Draw()
    {

    }
}
