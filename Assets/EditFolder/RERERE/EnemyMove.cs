using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] GameObject PL;
    Vector2 PLpos;
    Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PLpos = PL.transform.position;
        pos=transform.position;

    }
}
