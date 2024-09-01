using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMove : MonoBehaviour
{
    GameObject PL;

    /// <summary> プレイヤーが自分のどちら側にいるか？ </summary>
    float _axis;
    bool _isDash;

    AudioSource _mouseAudioSource;

    Rigidbody2D rb;
    [SerializeField] float _moveSpeed = 4;

    //常にプレイヤーを追いかける
    //一定範囲内に入ったら
    void Start()
    {
        _mouseAudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        PL = FindAnyObjectByType<PlayerController>().gameObject;
    }
    void Update()
    {
        _axis = Mathf.Sign(PL.transform.position.x - transform.position.x);
        //自分の向いている方向にプレイヤーがいるならば
        if (_axis == transform.localScale.x)
        {
            //プレイヤーとの距離が８以下ならば
            if (Vector2.Distance(transform.position, PL.transform.position) < 8)
            {
                StartCoroutine(Dash());
            }
            else if (Vector2.Distance(transform.position, PL.transform.position) < 16f)
            {
                rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
            }
        }
        //向いている方向にプレイヤーがいない場合かつ速度が一定以下の場合
        else if (Mathf.Abs(rb.velocity.x) < _moveSpeed)
        {
            transform.localScale = new Vector3(_axis, 1, 1);
        }
    }

    IEnumerator Dash()
    {
        //プレイヤーのいる方向に突進
        _mouseAudioSource.Play();
        float axis = _axis;
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(1);
        rb.velocity = new Vector2(axis * _moveSpeed * 3, 0);
        yield return new WaitForSeconds(3);
        rb.velocity = rb.velocity.normalized * _moveSpeed;
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
