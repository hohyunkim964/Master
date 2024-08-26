using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public GameSystem _gameSystem;
    public GameObject BackGround;   
    public Slider HPSlider;

    private UnitObj _unit;
    private Transform PlayerCanvas = null;
    private Transform EnemyCanvas = null;
    private bool isOnce = false;
    private int ActiveOBJCount = 0;

    private void Awake()
    {
        isOnce = false;
        _gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        BackGround = this.gameObject.transform.Find("BackGround").gameObject;
        HPSlider = this.GetComponent<Slider>();
        PlayerCanvas = GameObject.Find("HPPlayerBar").transform;
        EnemyCanvas = GameObject.Find("HPEnemyBar").transform;
    }

    private void Update()
    {
        if (_gameSystem.IsGameStart)
        {
            if (!isOnce)
            {
                isOnce = true;
                if(!_unit.gameObject.activeSelf)
                    this.gameObject.SetActive(false);
                PlayerHPReposition();
            }

            HPSlider.value = _unit._hp;
        }
        else if (_gameSystem.IsGameEnd)
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayerHPReposition()
    {
        for (int i = 0; i < _gameSystem.Player.Length; i++)
        {
            if (_gameSystem.Player[i].activeSelf)
            {
                if (_gameSystem.Player[i] == _unit.gameObject)
                {
                    switch (_unit.State)
                    {
                        case UnitObj.UnitState.Player:
                            transform.localPosition = new Vector3(GetComponent<RectTransform>().rect.width, 200f + (ActiveOBJCount * -50f), 1f);
                            break;
                    }

                }
                ActiveOBJCount++;
            }
        }
    }

    public void unitCreate(UnitObj unit)
    {
        _unit = unit;

        HPSlider.maxValue = unit._hp;
        HPSlider.value = HPSlider.maxValue;

        switch (unit.State)
        {
            case UnitObj.UnitState.Player:
                this.gameObject.transform.SetParent(PlayerCanvas);
                transform.localPosition = new Vector3(GetComponent<RectTransform>().rect.width, 200f + (_gameSystem.PlayerCount * -50f), 1f);
                break;
            case UnitObj.UnitState.Enemy:
                this.gameObject.transform.SetParent(EnemyCanvas);
                transform.localPosition = new Vector3(-1f * GetComponent<RectTransform>().rect.width, 200f + (_gameSystem.EnemyCount * -50f), 1f);
                break;
        }

        unit.HPBar = this.gameObject;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }


}
