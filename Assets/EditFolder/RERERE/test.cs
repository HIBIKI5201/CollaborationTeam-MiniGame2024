using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class test : MonoBehaviour
{
    Vector3 targetPosition = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(targetPosition, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
