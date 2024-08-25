using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LosePanel;
    public GameObject WinPanel;

    private GameObject _CreateOBJHP = null;
    public GameObject HPBar = null;
    private HPBar _hpBar = null;

    private void Awake()
    {
        if (LosePanel == null)
        {
            LosePanel = GameObject.Find("LosePanel").gameObject;
        }
        if (WinPanel == null)
        {
            WinPanel = GameObject.Find("WinPanel").gameObject;
        }
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
    }

    public void CreateHPBar(GameObject OBJ)
    {
        _CreateOBJHP = Instantiate(HPBar, new Vector3(0f, 0f, 0f), Quaternion.identity);
        _hpBar = _CreateOBJHP.GetComponent<HPBar>();
        _hpBar.unitCreate(OBJ.GetComponent<UnitObj>());
    }

    public void Update()
    {
        
    }
}
