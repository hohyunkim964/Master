using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    public GameObject CreatePrefab = null;

    private GameObject _CreateOBJ = null;

    public void onCreate()
    {      
        _CreateOBJ = Instantiate(CreatePrefab, this.gameObject.transform.position, Quaternion.identity);
    }
}
