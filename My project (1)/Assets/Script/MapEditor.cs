using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public GameObject Tile = null;
    public GameObject EnemyOBJ_Adventurer = null;
    public GameObject EnemyOBJ_Warrior = null;
    public GameObject EnemyOBJ_Archor = null;
    public List<GameObject> TileMap = new List<GameObject>();
    public List<UnitObj> Enemy_Pos = new List<UnitObj>();

    public int X_Horizontal = 0;
    public int Y_Vertical = 0;
    public int Enemy_Warrior = 0;
    public int Enemy_Archor = 0;
    public int Enemy_Adventurer = 0;

    private UIManager _uiManager;
    private List<Vector2> _SpawnTilePos = new List<Vector2>();
    private GameSystem _gameSystem;

    private float _minus_Y = 0;
    private float _plus_Y = 0;
    private float _line_Y = 0;
    private short _input_X = 0;
    private int _Enemy_Count = 0;

    private void Awake()
    {
        _Enemy_Count = Enemy_Warrior + Enemy_Archor + Enemy_Adventurer;
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        _BaseInit();

        _EnemySpawn();
    }

    private void Start()
    {
        if (TileMap.Count <= 0)
        {
            _BaseInit();
        }
    }

    private void _BaseInit()
    {
        if (X_Horizontal != 0 && Y_Vertical != 0)
        {
            for (int i = 0; i < Y_Vertical; i++)
            {
                if (i % 2 != 0 && i != 0)
                {
                    _plus_Y += 1;
                    _line_Y = _plus_Y;
                }
                else if (i % 2 == 0 && i != 0)
                {
                    _minus_Y -= 1;
                    _line_Y = _minus_Y;
                }
                else
                {
                    _line_Y = 0;
                }

                for (int j = 0; j < X_Horizontal; j++)
                {
                    GameObject a = Instantiate(Tile, this.gameObject.transform);
                    a.transform.position = new Vector2(-(X_Horizontal / 2) + j, _line_Y);

                    switch (_line_Y)
                    {
                        case 2:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 0);
                            break;
                        case 1:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 1);
                            break;
                        case 0:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 2);
                            break;
                        case -1:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 3);
                            break;
                        case -2:
                            a.GetComponent<NodeEditor>().SetIsStayCheck(false);
                            a.GetComponent<NodeEditor>().SetNodeCount(_input_X, 4);
                            break;
                    }

                    _input_X += 1;
                    
                    TileMap.Add(a);

                    if ((X_Horizontal/2 + 1) <= _input_X) 
                    {
                        _SpawnTilePos.Add(new Vector2(a.transform.position.x, _line_Y));
                    }
                }
                _input_X = 0;
            }
        }
    }

    private void _EnemySpawn()
    {
        if (_SpawnTilePos.Count >= _Enemy_Count)
        {
            //오브젝트 생성
            for (int i = 0; i < Enemy_Warrior; i++)
            {
                GameObject a = Instantiate(EnemyOBJ_Warrior, this.gameObject.transform);
                int randonCount = Random.Range(0, _Enemy_Count);
                a.transform.position = _SpawnTilePos[randonCount];
                a.transform.SetParent(null);
                Enemy_Pos.Add(a.GetComponent<UnitObj>());
                _SpawnTilePos.RemoveAt(randonCount);               
                _uiManager.CreateHPBar(a);
                _gameSystem.EnemyCount += 1;
            }

            for (int i = 0; i < Enemy_Adventurer; i++)
            {
                GameObject a = Instantiate(EnemyOBJ_Adventurer, this.gameObject.transform);
                int randonCount = Random.Range(0, _Enemy_Count);
                a.transform.position = _SpawnTilePos[randonCount];
                a.transform.SetParent(null);
                Enemy_Pos.Add(a.GetComponent<UnitObj>());
                _SpawnTilePos.RemoveAt(randonCount);     
                _uiManager.CreateHPBar(a);
                _gameSystem.EnemyCount += 1;
            }

            for (int i = 0; i < Enemy_Archor; i++)
            {
                GameObject a = Instantiate(EnemyOBJ_Archor, this.gameObject.transform);
                int randonCount = Random.Range(0, _Enemy_Count);
                a.transform.position = _SpawnTilePos[randonCount];
                a.transform.SetParent(null);
                Enemy_Pos.Add(a.GetComponent<UnitObj>());
                _SpawnTilePos.RemoveAt(randonCount);
                _uiManager.CreateHPBar(a);
                _gameSystem.EnemyCount += 1;
            }
        }
    }
}
