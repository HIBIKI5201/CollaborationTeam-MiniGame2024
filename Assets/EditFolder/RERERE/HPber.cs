using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPber : MonoBehaviour
{
    [SerializeField] GameObject HPbar;
    //[SerializeField] GameObject HPbarBack;
    Vector2 _hpPos;
    [SerializeField] int _hp = 5;
    [SerializeField] float _hpDistance = 100;
    GameObject[] _HPclone;

    
    // Start is called before the first frame update
    void Start()
    {
        _HPclone = new GameObject[_hp];
        for (int i = 0; i < _hp;i++)
        {
            GameObject barFragment = Instantiate(HPbar);
            RectTransform barRect = barFragment.GetComponent<RectTransform>();
            barRect.anchoredPosition = new Vector2(_hpDistance * i, 0);
            barFragment.transform.parent = transform;
            _HPclone[i] = barFragment;
            
            barRect.localScale = Vector3.one;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
