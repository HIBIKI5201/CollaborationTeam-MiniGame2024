using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̃R���|�[�l���g")]
    [SerializeField] Rigidbody2D PlayerRB;

    [Header("�v���C���[�̃X�e�[�^�X")]
    [SerializeField] float _moveSpeed = 5;
    [Space]
    [SerializeField] float _jumpPower = 5;
    [SerializeField] int _jumpLimit = 1;
    int jumpCount;
    float ScaleX;
    float Angle;

    [Header("�U���X�e�[�^�X")]
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
        //���E�ړ�
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

        //�W�����v
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < _jumpLimit)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }

        //����
        if(Input.GetKeyDown(KeyCode.S) && jumpCount == 0)
        {
            Debug.Log("����");
        }

        //�ߐڍU��
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            AttackCollider.enabled = true;

            Debug.Log("�ߐ�");
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            AttackCollider.enabled = false;
        }

        //�������U��
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("�������U��");

            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Angle, 0);
        }
    }
}
