using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    public GameObject CreatePrefab = null;

    private UIManager _uiManager = null;
 
    private GameObject _CreateOBJ = null;


    private void Awake()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void onCreatePlayer()
    {      
        _CreateOBJ = Instantiate(CreatePrefab, this.gameObject.transform.position, Quaternion.identity);
        _uiManager.CreateHPBar(_CreateOBJ);
    }
}
