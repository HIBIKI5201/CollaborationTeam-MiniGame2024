using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのコンポーネント")]
    [SerializeField] Rigidbody2D PlayerRB;

    [Header("プレイヤーのステータス")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpPower;
    [SerializeField] int _jumpLimit;
    int jumpCount;
    void Start()
    {
        
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal != 0)
        {
            PlayerRB.velocity = new Vector2(_moveSpeed * horizontal, PlayerRB.velocity.y);
        } else
        {
            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < _jumpLimit)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }
    }
}
