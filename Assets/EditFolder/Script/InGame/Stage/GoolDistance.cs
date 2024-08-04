using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoolDistance : MonoBehaviour
{
    [SerializeField] GameObject PL;
    Vector2 _plPos;
    // Start is called before the first frame update
    void Start()
    {
        _plPos = PL.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
