using System.Collections;
using UnityEngine;

public class karasuMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _moveSpeed;
    [SerializeField] float _attackRange;
    [SerializeField] float _underLine;
    GameObject PL;
    Rigidbody2D rb;
    float _axis;
    bool _isAttack = false;

    float _firstAltitude = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PL = FindAnyObjectByType<PlayerController>().gameObject;
        _firstAltitude = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        _axis = Mathf.Sign(PL.transform.position.x -  transform.position.x);
        if (Vector2.Distance(PL.transform.position, transform.position) < _attackRange && !_isAttack && transform.position.y >= _firstAltitude)
        {
            Vector2 axis = (PL.transform.position - transform.position).normalized;
            rb.velocity = axis * _moveSpeed;
            _isAttack = true;
        }
        else if (transform.transform.position.y < _underLine )
        {
            _isAttack = false;
        }
        if (!_isAttack )
        {
            if (transform.position.y <= _firstAltitude)
            {
                rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x), 1).normalized * _moveSpeed;

            }
            else
            {
                rb.velocity = new Vector2(_axis, 0).normalized * _moveSpeed;
            }
        }        
    }

    public void HitDamage(float damage)
    {
        if (damage > 0)
        {
            ScoreManager.AddScore(100);
            Destroy(gameObject);
        }
    }
}
