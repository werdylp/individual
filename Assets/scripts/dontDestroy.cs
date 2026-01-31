using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class dontDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
}
