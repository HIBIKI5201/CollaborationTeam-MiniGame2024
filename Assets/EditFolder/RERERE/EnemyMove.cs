using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] GameObject PL;
    Vector2 PLpos;
    Vector2 pos;
    int r;
    Rigidbody rb;
    [SerializeField]float Movespeed=5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PLpos = PL.transform.position;
        pos=transform.position;
        //enemy‚©‚çŒ©‚ÄPL‚ª‚Ç‚¿‚ç‚É‚¢‚é‚©”»’è‚µ‚»‚Ì•ûŒü‚Éi‚Ş
        if(PLpos.x-pos.x<0)
        {
            r = 1;
        }
        else
        {
            r = -1;
        }
        rb.velocity=new Vector2(r*Movespeed,rb.velocity.y );
        
    }
}
