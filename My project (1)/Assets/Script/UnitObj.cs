using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObj : MonoBehaviour
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

    public enum UnitBehavior
    {
        Idle,
        Attack,
        Move,
    }

    public UnitBehavior UnitBe = UnitBehavior.Idle;
    public Unit UnitJob = Unit.People;
    public UnitState State = UnitState.None;
    public bool IsAttack = false;
    public GameObject ContactOpponent = null;

    [SerializeField] private short _curPos_X = -1;
    [SerializeField] private short _curPos_Y = -1;
    [SerializeField] private float _hp = 0;

    private GameSystem _gameSystem;
    private SpriteRenderer _spriteRenderer = null;
    private UnitObj _contactEnemy = null; //한 방향 공격시 피격되는 적
    public List<UnitObj> _contactEnemyList = new List<UnitObj>(); //다중 공격시 피격되는 적
    private List<UnitObj> _opponentInfo = new List<UnitObj>();
    private Coroutine _corCoutine;
    private Rigidbody2D _rb2d;
    private Animator _animator;
    private bool _isDie = false;
    private float _time = 0.0f;
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_rb2d.bodyType != RigidbodyType2D.Kinematic)
        {
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
        
        CheckOpponentCount();
    }

    private void Start()
    {
        switch (UnitJob)
        {
            case Unit.Adventurer:
                _hp = 10;
                break;
            case Unit.Archor:
                _hp = 20;
                break;
            case Unit.Warrior:
                _hp = 30;
                break;
            default:
                _hp = 0;
                break;
        }

        ChangePattern();
    }

    private void Update()
    {
        if (!_gameSystem.IsGameStart && !_gameSystem.IsGameEnd)
        {
            if (!_spriteRenderer.enabled)
            {
                _spriteRenderer.enabled = true;
            }
            CheckOpponentCount();
        }
        else if (_gameSystem.IsGameStart && !_gameSystem.IsGameEnd)
        {
            if (!_isDie)
            {
                _AttackRangeChk();
                if (_gameSystem.IsGameTurnEnd)
                {
                    UnitBe = UnitBehavior.Idle;
                }
                else
                {
                    switch (State)
                    {
                        case UnitState.Player:
                            if (!IsAttack)
                            {
                                UnitBe = UnitBehavior.Move;
                            }
                            else 
                            {
                                UnitBe = UnitBehavior.Attack;
                            }
                            break;
                        case UnitState.Enemy:
                            if (!IsAttack)
                            {
                                UnitBe = UnitBehavior.Idle;
                            }
                            else 
                            {
                                UnitBe = UnitBehavior.Attack;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (_spriteRenderer.enabled)
                {
                    _spriteRenderer.enabled = false;
                }
            }
        }
    }

    private void CheckOpponentCount()
    {
        switch (State)
        {
            case UnitState.Enemy:
                if (_gameSystem.Player.Length != _opponentInfo.Count && _gameSystem.Player.Length > 0)
                {
                    _opponentInfo.Clear();
                    for (int i = 0; i < _gameSystem.Player.Length; i++)
                    {
                        _opponentInfo.Add(_gameSystem.Player[i].GetComponent<UnitObj>());                      
                    }
                }
                break;
            case UnitState.Player:
                if (_gameSystem.Enemy.Length != _opponentInfo.Count && _gameSystem.Enemy.Length > 0)
                {
                    _opponentInfo.Clear();
                    for (int i = 0; i < _gameSystem.Enemy.Length; i++)
                    {
                        _opponentInfo.Add(_gameSystem.Enemy[i].GetComponent<UnitObj>());
                    }
                }
                break;
            default:
                break;
        }       
    }
    
    public void ChangePattern()
    {
        // if (!_gameSystem.IsGameTurnEnd)
        // {

        if (!_isDie)
        {
            if (_corCoutine != null)
            {
                StopCoroutine(_corCoutine);
            }

            switch (UnitBe)
            {
                case UnitBehavior.Idle:
                    _corCoutine = StartCoroutine(_Idle());
                    break;
                case UnitBehavior.Attack:
                    _corCoutine = StartCoroutine(_Attack());
                    break;
                case UnitBehavior.Move:

                    break;
            }
        }
     //   }
     //   else
     //   {
     //       ChangePattern();
     //   }
    }
      
    private void _Astar()
    {

    }

    private void _Action(Unit _unit)
    {
        _AttackRangeChk();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_gameSystem.IsGameEnd && col.gameObject.TryGetComponent(out NodeEditor tile))
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
        if (!_gameSystem.IsGameEnd && col.gameObject.TryGetComponent(out NodeEditor tile))
        {
            Debug.Log(_curPos_X);

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
        if (_gameSystem.IsGameStart && collision.gameObject.TryGetComponent(out NodeEditor tile))
        {
            if (tile.GetIsStayCheck())
            {
                tile.SetIsStayCheck(false);
            }
        }
    }

    private void _AttackRangeChk()
    {
        if (_curPos_X > -1 && _curPos_Y > -1)
        {
            IsAttack = false;

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
                                if (!_opponentInfo[i]._isDie)
                                {
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
                            }
                            break;
                        case UnitState.Player:
                            for (int i = 0; i < _opponentInfo.Count; i++)
                            {
                                short enemyPosX = _opponentInfo[i]._curPos_X;
                                short enemyPosY = _opponentInfo[i]._curPos_Y;
                                if (!_opponentInfo[i]._isDie)
                                {
                                    if (enemyPosX != _curPos_X && enemyPosY != _curPos_Y)
                                    {
                                        //주변 여덟칸 중 대각선 4개 방향 체크
                                        if ((enemyPosX == _curPos_X - 1 || enemyPosX == _curPos_X + 1) && (enemyPosY == _curPos_Y - 1 || enemyPosY == _curPos_Y + 1))
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
                                    else if (enemyPosX == _curPos_X && enemyPosY != _curPos_Y)
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
                            for (int i = 0; i < _opponentInfo.Count; i++)
                            {
                                short playerPosX = _opponentInfo[i]._curPos_X;
                                short playerPosY = _opponentInfo[i]._curPos_Y;
                                if (!_opponentInfo[i]._isDie)
                                {
                                    if (playerPosX != _curPos_X && playerPosY != _curPos_Y)
                                    {
                                        if ((playerPosX >= _curPos_X - 2 && playerPosX <= _curPos_X + 2 && playerPosY >= _curPos_Y - 1 && playerPosY <= _curPos_Y + 1) 
                                            || (playerPosX >= _curPos_X - 1 && playerPosX <= _curPos_X + 1 && playerPosY >= _curPos_Y - 2 && playerPosY <= _curPos_Y + 2))                                         
                                        {
                                            IsAttack = true;
                                        }
                                    }
                                    else if (playerPosX != _curPos_X && playerPosY == _curPos_Y)
                                    {
                                        if (playerPosX >= _curPos_X - 3 && playerPosX <= _curPos_X + 3)
                                        {
                                            IsAttack = true;
                                        }
                                    }
                                    else if (playerPosX == _curPos_X && playerPosY != _curPos_Y)
                                    {
                                        if (playerPosY >= _curPos_Y - 3 && playerPosY <= _curPos_Y + 3)
                                        {
                                            IsAttack = true;
                                        }
                                    }

                                    if (IsAttack)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        case UnitState.Player:
                            for (int i = 0; i < _opponentInfo.Count; i++)
                            {
                                short enemyPosX = _opponentInfo[i]._curPos_X;
                                short enemyPosY = _opponentInfo[i]._curPos_Y;
                                if (!_opponentInfo[i]._isDie)
                                {
                                    if (enemyPosX != _curPos_X && enemyPosY != _curPos_Y)
                                    {
                                        //주변 여덟칸 중 대각선 4개 방향 체크
                                        if ((enemyPosX >= _curPos_X - 2 || enemyPosX <= _curPos_X + 2) && (enemyPosY >= _curPos_Y - 2 || enemyPosY <= _curPos_Y + 2))
                                        {
                                            IsAttack = true;
                                        }
                                    }
                                    else if (enemyPosX != _curPos_X && enemyPosY == _curPos_Y)
                                    {
                                        //주변 여덟칸 중 좌우 방향 체크
                                        if (enemyPosX >= _curPos_X - 3 || enemyPosX <= _curPos_X + 3)
                                        {
                                            IsAttack = true;
                                        }
                                    }
                                    else if (enemyPosX == _curPos_X && enemyPosY != _curPos_Y)
                                    {
                                        //주변 여덟칸 중 상하 방향 체크
                                        if (enemyPosY >= _curPos_Y - 3 || enemyPosY <= _curPos_Y + 3)
                                        {
                                            IsAttack = true;
                                        }
                                    }

                                    if (IsAttack)
                                    {
                                        break;
                                    }
                                }
                            }
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

    private IEnumerator _Attack()
    {
        while (!_gameSystem.IsGameTurnEnd)
        {
            switch (UnitJob)
            {
                //근거리 8방향중 한곳으로 공격
                case Unit.Adventurer:
                    if (_contactEnemy == null)
                    {
                        for (int i = 0; i < _opponentInfo.Count; i++)
                        {
                            // 상하
                            if (!_opponentInfo[i]._isDie && _opponentInfo[i]._curPos_Y == _curPos_Y
                               && (_opponentInfo[i]._curPos_X == _curPos_X + 1 || _opponentInfo[i]._curPos_X == _curPos_X - 1))
                            {
                                _contactEnemy = _opponentInfo[i];
                                break;
                            }
                            else if (!_opponentInfo[i]._isDie && _opponentInfo[i]._curPos_X == _curPos_X
                                && (_opponentInfo[i]._curPos_Y == _curPos_Y + 1 || _opponentInfo[i]._curPos_Y == _curPos_Y - 1))
                            {
                                _contactEnemy = _opponentInfo[i];
                                break;
                            }
                            else if (!_opponentInfo[i]._isDie && (_opponentInfo[i]._curPos_X == _curPos_X + 1 || _opponentInfo[i]._curPos_X == _curPos_X - 1)
                                && (_opponentInfo[i]._curPos_Y == _curPos_Y + 1 || _opponentInfo[i]._curPos_Y == _curPos_Y - 1))
                            {
                                _contactEnemy = _opponentInfo[i];
                                break;
                            }
                        }
                    }

                    if (_contactEnemy != null)
                    {
                        if (!_animator.GetBool("Attack"))
                        {
                            Debug.Log("Att");
                            _animator.SetBool("Attack", true);
                        }
                    }

                    break;
                //근거리 8방향 전부 공격
                case Unit.Warrior:
                    if (_contactEnemyList.Count <= 0)
                    {
                        for (int i = 0; i < _opponentInfo.Count; i++)
                        {
                            // 상하
                            if (!_opponentInfo[i]._isDie && _opponentInfo[i]._curPos_Y == _curPos_Y
                               && (_opponentInfo[i]._curPos_X == _curPos_X + 1 || _opponentInfo[i]._curPos_X == _curPos_X - 1))
                            {
                                _contactEnemyList.Add(_opponentInfo[i]);
                            }
                            if (!_opponentInfo[i]._isDie && _opponentInfo[i]._curPos_X == _curPos_X
                                && (_opponentInfo[i]._curPos_Y == _curPos_Y + 1 || _opponentInfo[i]._curPos_Y == _curPos_Y - 1))
                            {
                                _contactEnemyList.Add(_opponentInfo[i]);
                            }
                            if (!_opponentInfo[i]._isDie && (_opponentInfo[i]._curPos_X == _curPos_X + 1 || _opponentInfo[i]._curPos_X == _curPos_X - 1)
                                && (_opponentInfo[i]._curPos_Y == _curPos_Y + 1 || _opponentInfo[i]._curPos_Y == _curPos_Y - 1))
                            {
                                _contactEnemyList.Add(_opponentInfo[i]);
                            }
                        }
                    }
                    if (_contactEnemyList.Count > 0)
                    {
                        if (!_animator.GetBool("Attack"))
                        {
                            Debug.Log("Att");
                            _animator.SetBool("Attack", true);
                        }
                    }
                    break;
                //원거리 8방향에서 한곳으로 공격
                case Unit.Archor:
                    if (_contactEnemy == null)
                    {
                        for (int i = 0; i < _opponentInfo.Count; i++)
                        {
                            // 상하
                            if (!_opponentInfo[i]._isDie && _opponentInfo[i]._curPos_Y == _curPos_Y
                               && (_opponentInfo[i]._curPos_X <= _curPos_X + 3 && _opponentInfo[i]._curPos_X >= _curPos_X - 3))
                            {
                                _contactEnemy = _opponentInfo[i];
                                break;
                            }
                            else if (!_opponentInfo[i]._isDie && _opponentInfo[i]._curPos_X == _curPos_X
                                && (_opponentInfo[i]._curPos_Y <= _curPos_Y + 3 && _opponentInfo[i]._curPos_Y >= _curPos_Y - 3))
                            {
                                _contactEnemy = _opponentInfo[i];
                                break;
                            }
                            else if (!_opponentInfo[i]._isDie &&
                                (_opponentInfo[i]._curPos_X <= _curPos_X + 2 && _opponentInfo[i]._curPos_X >= _curPos_X - 2 && _opponentInfo[i]._curPos_Y <= _curPos_Y + 1 && _opponentInfo[i]._curPos_Y >= _curPos_Y - 1)
                               || (_opponentInfo[i]._curPos_X <= _curPos_X + 1 && _opponentInfo[i]._curPos_X >= _curPos_X - 1 && _opponentInfo[i]._curPos_Y <= _curPos_Y + 2 && _opponentInfo[i]._curPos_Y >= _curPos_Y - 2))
                            {
                                _contactEnemy = _opponentInfo[i];
                                break;
                            }
                        }
                    }

                    if (_contactEnemy != null)
                    {
                        if (!_animator.GetBool("Attack"))
                        {
                            Debug.Log("Att");
                            _animator.SetBool("Attack", true);
                        }
                    }
                    break;
                default:
                    break;
            }
            yield return null;
        }


        yield return new WaitForSecondsRealtime(0.5f);

        //while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) 
        //{
        //    
        //}

        switch (UnitJob)
        {
            case Unit.Adventurer:
            case Unit.Archor:
                if (_contactEnemy != null)
                {
                    _contactEnemy._hp -= 5f;
                    if (_contactEnemy._hp <= 0)
                    {
                        _contactEnemy._isDie = true;
                        _contactEnemy = null;
                    }
                }
               
                if(_contactEnemy == null)
                {
                    switch (State)
                    {
                        case UnitState.Enemy:
                            if (_gameSystem.PlayerCount > 0)
                            {
                                _gameSystem.PlayerCount -= 1;
                                UnitBe = UnitBehavior.Idle;
                            }
                            else
                            {
                                _gameSystem.EndGame();
                            }
                            break;
                        case UnitState.Player:
                            if (_gameSystem.EnemyCount > 0)
                            {
                                _gameSystem.EnemyCount -= 1;
                                UnitBe = UnitBehavior.Move;
                            }
                            else
                            {
                                _gameSystem.EndGame();
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
            case Unit.Warrior:
                if (_contactEnemyList.Count > 0)
                {
                    for (int i = _contactEnemyList.Count - 1; i >= 0; i--)
                    {
                        _contactEnemyList[i]._hp -= 5f;

                        if (_contactEnemyList[i]._hp <= 0)
                        {
                            _contactEnemyList[i]._isDie = true;
                            _contactEnemyList.RemoveAt(i);
                            switch (State)
                            {
                                case UnitState.Enemy:
                                    if (_gameSystem.PlayerCount > 0)
                                    {
                                        _gameSystem.PlayerCount -= 1;
                                    }
                                    else
                                    {
                                        _gameSystem.EndGame();
                                    }
                                    break;
                                case UnitState.Player:
                                    if (_gameSystem.EnemyCount > 0)
                                    {
                                        _gameSystem.EnemyCount -= 1;
                                    }
                                    else
                                    {
                                        _gameSystem.EndGame();
                                    }
                                    break;
                            }
                        }
                    }                   
                }

                if (_contactEnemyList.Count <= 0)
                {
                    switch (State)
                    {
                        case UnitState.Enemy:
                            if (_gameSystem.PlayerCount > 0)
                            {
                                UnitBe = UnitBehavior.Idle;
                            }
                            else
                            {
                                _gameSystem.EndGame();
                            }
                            break;
                        case UnitState.Player:
                            if (_gameSystem.EnemyCount > 0)
                            {
                                UnitBe = UnitBehavior.Move;
                            }
                            else
                            {
                                _gameSystem.EndGame();
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
        }
     
        ChangePattern();
    }

    private IEnumerator _Idle()
    {
        if (_animator.GetBool("Attack"))
        {
            _animator.SetBool("Attack", false);
        }

        if (_animator.GetBool("Move"))
        {
            _animator.SetBool("Move", false);
        }

        IsAttack = false;

        while (UnitBe == UnitBehavior.Idle) 
        {
            yield return null;
        }

        ChangePattern();
    }
}
