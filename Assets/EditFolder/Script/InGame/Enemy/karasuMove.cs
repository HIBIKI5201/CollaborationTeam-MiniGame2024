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
    float _firstScale;
    Coroutine _dashCoroutine;
    float _firstAltitude = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PL = FindAnyObjectByType<PlayerController>().gameObject;
        _firstAltitude = transform.position.y;
        _firstScale = transform.localScale.x;
    }

    void Update()
    {
        _axis = Mathf.Sign(PL.transform.position.x -  transform.position.x);
        if (Vector2.Distance(PL.transform.position, transform.position) < _attackRange && !_isAttack && Mathf.Abs(transform.position.y - _firstAltitude) < 0.5f)
        {
            _dashCoroutine = StartCoroutine(Dash());
        }
        else if (transform.transform.position.y < _underLine )
        {
            _isAttack = false;
            StopCoroutine(_dashCoroutine);
        }
        if (!_isAttack)
        {
            if (Mathf.Abs(transform.position.y - _firstAltitude) > 0.5f)
            {
                rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x), Mathf.Sign(_firstAltitude - transform.position.y)).normalized * _moveSpeed;

            }
            else
            {
                if (Mathf.Abs(PL.transform.position.x - transform.position.x) > 1)
                {
                    transform.localScale = new Vector3(_firstScale * _axis, transform.localScale.y, transform.localScale.z);
                    rb.velocity = new Vector2(_axis, 0).normalized * _moveSpeed;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
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

    IEnumerator Dash()
    {
        transform.localScale = new Vector3(_firstScale * _axis, transform.localScale.y, transform.localScale.z);
        Vector2 axis = (PL.transform.position - transform.position).normalized;
        rb.velocity = axis * _moveSpeed;
        _isAttack = true;

        yield return new WaitForSeconds(3);
        _isAttack = false;
    }
}
