using UnityEngine;

public class DragDropUi : MonoBehaviour
{
    public GameSystem GameSystem = null;

    private GameObject _DragOBJ = null;
    private Vector2 _Pos;
    [SerializeField] private bool isClick = false;



    private void Awake()
    {
        GameSystem = GameObject.Find("GameSystem").gameObject.GetComponent<GameSystem>();
        isClick = true;
        _DragOBJ = this.gameObject;
        _Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _DragOBJ.transform.position = _Pos;
    }

    private void Update()
    {
        if (!GameSystem.IsGameStart)
        {
            _Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(_Pos, Vector2.zero, 0f, LayerMask.GetMask("Over UI"));
            if (hit.collider != null)
            {
                Debug.Log(hit.transform.gameObject);
                if (hit.transform.gameObject == this.gameObject)
                {
                    if (isClick)
                    {
                        if (!Input.GetMouseButtonUp(0))
                        {
                            _DragOBJ.transform.position = _Pos;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            isClick = true;
                        }
                    }

                }
                else
                {
                    isClick = false;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && !GameSystem.IsGameStart)
        {
            isClick = false;
        }
    }
}
