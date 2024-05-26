using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのコンポーネント")]
    [SerializeField] Rigidbody2D PlayerRB;

    [Header("プレイヤーのステータス")]
    [SerializeField] float _moveSpeed = 5;
    [Space]
    [SerializeField] float _jumpPower = 5;
    [SerializeField] int _jumpLimit = 1;
    int jumpCount;
    float ScaleX;
    float Angle;

    [Header("攻撃ステータス")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] float _attackDamage;
    [Space]
    [SerializeField] GameObject Bullet;
    [SerializeField] float _bulletDamage;
    [SerializeField] float _bulletSpeed;
    void Start()
    {
        ScaleX = transform.localScale.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

    void Update()
    {
        //左右移動
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal != 0)
        {
            PlayerRB.velocity = new Vector2(_moveSpeed * horizontal, PlayerRB.velocity.y);
            transform.localScale = new Vector2(ScaleX * horizontal, transform.localScale.y);

            Angle = horizontal;
        } else
        {
            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
        }

        //ジャンプ
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < _jumpLimit)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }

        //伏せ
        if(Input.GetKeyDown(KeyCode.S) && jumpCount == 0)
        {
            Debug.Log("伏せ");
        }

        //近接攻撃
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            AttackCollider.enabled = true;

            Debug.Log("近接");
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            AttackCollider.enabled = false;
        }

        //遠距離攻撃
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("遠距離攻撃");

            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Angle, 0);
        }
    }
}
