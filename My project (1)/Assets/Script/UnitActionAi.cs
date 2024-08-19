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

    public Unit UnitState = Unit.People;

    private Coroutine coroutine;
    private Rigidbody2D _rb2d;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        if (_rb2d.bodyType != RigidbodyType2D.Kinematic)
        {
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
        }

    }

    public void ChangePattern()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        switch (UnitState)
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

        ChangePattern();
    }

    private void _Action(Unit _unit)
    {

        ChangePattern();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out NodeEditor tile))
        {
            if (!tile.GetIsStayCheck())
            {
                tile.SetIsStayCheck(true);
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
}
