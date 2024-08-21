using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionAi : MonoBehaviour
{
    public enum Unit
    {
        Adventurer,
        Warrior,
        Archor,
        People,
    }

    public enum UnitState
    {
        Player,
        Enemy,
        None,
    }

    public Unit UnitJob = Unit.People;
    public UnitState State = UnitState.None;
    public bool IsAttack = false;

    [SerializeField] private short _curPos_X = -1;
    [SerializeField] private short _curPos_Y = -1;
    private Coroutine _corCoutine;
    private Rigidbody2D _rb2d;
    private GameSystem _gameSystem;

    [SerializeField] private List<UnitActionAi> _opponentInfo = new List<UnitActionAi>();

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (_rb2d.bodyType != RigidbodyType2D.Kinematic)
        {
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
        
        CheckOpponentCount();
    }

    private void Update()
    {
        if (!_gameSystem.IsGameStart)
        {
            switch (State)
            {
                case UnitState.Enemy:
                    if (_gameSystem.Player.Length != _opponentInfo.Count && _gameSystem.Player.Length > 0)
                    {
                        _opponentInfo.Clear();
                        for (int i = 0; i < _gameSystem.Player.Length; i++)
                        {
                            _opponentInfo.Add(_gameSystem.Player[i].GetComponent<UnitActionAi>());
                        }
                    }
                    break;
                case UnitState.Player:
                    if (_gameSystem.Enemy.Length != _opponentInfo.Count && _gameSystem.Enemy.Length > 0)
                    {
                        _opponentInfo.Clear();
                        for (int i = 0; i < _gameSystem.Enemy.Length; i++)
                        {
                            _opponentInfo.Add(_gameSystem.Enemy[i].GetComponent<UnitActionAi>());
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        AttackRangeChk();
    }

    private void CheckOpponentCount()
    {
        switch (State)
        {
            case UnitState.Enemy:
                if (_gameSystem.Player.Length > 0)
                {
                    for (int i = 0; i < _gameSystem.Player.Length; i++)
                    {
                        _opponentInfo.Add(_gameSystem.Player[i].GetComponent<UnitActionAi>());
                    }
                }
                break;
            case UnitState.Player:
                if (_gameSystem.Enemy.Length > 0)
                {
                    for (int i = 0; i < _gameSystem.Enemy.Length; i++)
                    {
                        _opponentInfo.Add(_gameSystem.Enemy[i].GetComponent<UnitActionAi>());
                    }
                }
                break;
            default:
                break;
        }
    }

    public void ChangePattern()
    {
        if (_corCoutine != null)
        {
            StopCoroutine(_corCoutine);
        }

        switch (UnitJob)
        {
            case Unit.Adventurer:
                //근처에 적이 있는지 유무 확인
                break;
            case Unit.Warrior:
                //근처에 적이 있는지 유무 확인
                break;
            case Unit.Archor:
                //근처에 적이 있는지 유무 확인
                break;
            default:
                break;
        }
    }

   

    private void _Astar()
    {
        AttackRangeChk();
    }

    private void _Action(Unit _unit)
    {
        AttackRangeChk();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out NodeEditor tile))
        {
            if (!tile.GetIsStayCheck())
            {
                tile.SetIsStayCheck(true);
            }
            if (_curPos_Y != tile.GetNodeCountY() || _curPos_X != tile.GetNodeCountX())
            {
                _curPos_X = tile.GetNodeCountX();
                _curPos_Y = tile.GetNodeCountY();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out NodeEditor tile))
        {
            if (!tile.GetIsStayCheck())
            {
                tile.SetIsStayCheck(true);
            }
            if (_curPos_Y != tile.GetNodeCountY() || _curPos_X != tile.GetNodeCountX())
            {
                _curPos_X = tile.GetNodeCountX();
                _curPos_Y = tile.GetNodeCountY();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out NodeEditor tile))
        {
            if (tile.GetIsStayCheck())
            {
                tile.SetIsStayCheck(false);
            }
        }
    }

    private void AttackRangeChk()
    {
        if (_curPos_X > -1 && _curPos_Y > -1)
        {
            switch (UnitJob)
            {
                //근거리 8방향에 적이 있는지 체크
                case Unit.Adventurer:
                case Unit.Warrior:
                    switch (State)
                    {
                        case UnitState.Enemy:
                            for (int i = 0; i < _opponentInfo.Count; i++)
                            {
                                short playerPosX = _opponentInfo[i]._curPos_X;
                                short playerPosY = _opponentInfo[i]._curPos_Y;

                                if (playerPosX != _curPos_X && playerPosY != _curPos_Y)
                                {
                                    //주변 여덟칸 중 대각선 4개 방향 체크
                                    if ((playerPosX == _curPos_X - 1 || playerPosX == _curPos_X + 1) && (playerPosY == _curPos_Y - 1 || playerPosY == _curPos_Y + 1))
                                    {
                                        IsAttack = true;
                                    }
                                }
                                else if (playerPosX != _curPos_X && playerPosY == _curPos_Y)
                                {
                                    //주변 여덟칸 중 좌우 방향 체크
                                    if (playerPosX == _curPos_X - 1 || playerPosX == _curPos_X + 1)
                                    {
                                        IsAttack = true;
                                    }
                                }
                                else if (playerPosX == _curPos_X && playerPosY != _curPos_Y)
                                {
                                    //주변 여덟칸 중 상하 방향 체크
                                    if (playerPosY == _curPos_Y - 1 || playerPosY == _curPos_Y + 1)
                                    {
                                        IsAttack = true;
                                    }
                                }

                                if (IsAttack)
                                {
                                    break;
                                }
                            }
                            break;
                        case UnitState.Player:
                            for (int i =0;i < _opponentInfo.Count; i++)
                            {
                                short enemyPosX = _opponentInfo[i]._curPos_X;
                                short enemyPosY = _opponentInfo[i]._curPos_Y;

                                if (enemyPosX != _curPos_X && enemyPosY != _curPos_Y)
                                {
                                    //주변 여덟칸 중 대각선 4개 방향 체크
                                    if((enemyPosX == _curPos_X - 1 || enemyPosX == _curPos_X + 1) && (enemyPosY == _curPos_Y - 1 || enemyPosY == _curPos_Y + 1))
                                    {
                                        IsAttack = true;
                                    }
                                }
                                else if (enemyPosX != _curPos_X && enemyPosY == _curPos_Y)
                                {
                                    //주변 여덟칸 중 좌우 방향 체크
                                    if (enemyPosX == _curPos_X - 1 || enemyPosX == _curPos_X + 1)
                                    {
                                        IsAttack = true;
                                    }                                  
                                }
                                else if(enemyPosX == _curPos_X && enemyPosY != _curPos_Y)
                                {
                                    //주변 여덟칸 중 상하 방향 체크
                                    if (enemyPosY == _curPos_Y - 1 || enemyPosY == _curPos_Y + 1)
                                    {
                                        IsAttack = true;
                                    }
                                }

                                if (IsAttack)
                                {
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                //원거리 8방향에 적이 있는지 체크
                case Unit.Archor:
                    switch (State)
                    {
                        case UnitState.Enemy:
                            break;
                        case UnitState.Player:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void Attack()
    {

    }
}
