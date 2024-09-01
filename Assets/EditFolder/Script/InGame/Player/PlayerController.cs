using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D PlayerRB;
    Animator PlayerAnimator;
    SoundEffectManager soundEffectManager;

    [SerializeField, Tooltip("InputSystem")]
    PlayeInputSystem _inputSystem;

    [Tooltip("�擾�L�ʐ�")]
    float _haveCatFoodValue;

    [Header("�v���C���[�̈ړ��X�e�[�^�X")]

    [SerializeField, Tooltip("���s�̈ړ��X�s�[�h")]
    float _moveSpeed = 5;
    [Space]
    [SerializeField, Tooltip("�W�����v��")]
    float _jumpPower = 5;
    [SerializeField, Tooltip("�W�����v��")]
    int _jumpLimit = 1;
    [Space]
    [SerializeField, Tooltip("�Ǔo�葬�x")]
    float _wallclimbSpeed = 3;

    int jumpCount;
    float ScaleX;
    float _lastHorizontalAxis;

    [Header("�U���X�e�[�^�X")]
    [SerializeField, Tooltip("�U������̃R���C�_�[")]
    BoxCollider2D AttackCollider;
    [SerializeField, Tooltip("�ߐڍU����")]
    float _attackDamage;
    [Space]
    [SerializeField, Tooltip("�����e�ۂ̃I�u�W�F�N�g")]
    GameObject Bullet;
    [SerializeField, Tooltip("�e�ۂ̍U����")]
    float _bulletDamage;
    [SerializeField, Tooltip("�e�ۂ̃X�s�[�h")]
    float _bulletSpeed;
    [SerializeField, Tooltip("���˂̃C���^�[�o��")]
    float _bulletFireInterval;
    float _bulletIntervalTimer;

    [Header("�̗̓X�e�[�^�X")]
    [SerializeField, Tooltip("�̗̓o�[")]
    Image healthBar;

    [SerializeField, Tooltip("�̗�")]
    float _maxHealth;
    [Tooltip("���ݑ̗�")]
    float _currentHealth;
    [SerializeField, Tooltip("���G����")]
    float _hitIntarval;
    float _hitIntervalTimer;

    [Header("�v���C���[�̃��[�h")]
    [SerializeField] PlayerMode playerMode;
    public enum PlayerMode
    {
        Normal,
        Walk,
        WallRun,
    }


    void Start()
    {
        _inputSystem = new();
        _inputSystem.Enable();

        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        soundEffectManager = FindAnyObjectByType<SoundEffectManager>();
        if (soundEffectManager == null) Debug.LogWarning("SoundEffectManager������܂���");
        _currentHealth = _maxHealth;
        if (healthBar != null)
            healthBar.fillAmount = _currentHealth / _maxHealth;

        ScaleX = transform.localScale.x;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //�_���[�W���󂯂�����
        if (collision.gameObject.CompareTag("Enemy") && _hitIntervalTimer + _hitIntarval < Time.time)
        {
            _hitIntervalTimer = Time.time;
            HitDamage(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayerAnimator.SetBool("IsGround", true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //�n�ʂɒ��������̏���
        if (collision.gameObject.CompareTag("Ground") && collision.contacts[0].normal.y >= 0)
        {
            if (jumpCount > 0)
            {
                jumpCount = 0;
            }

            Vector2 CollisionNormal = collision.contacts[0].normal;
            if ((Mathf.Abs(CollisionNormal.x) >= 0.01 ? (int)Mathf.Sign(CollisionNormal.x) * -1 : 0) == Input.GetAxisRaw("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)
            {
                Debug.Log("�Ǔo��");
                if (PlayerMode.WallRun != playerMode)
                {
                    playerMode = PlayerMode.WallRun;
                }
                PlayerAnimator.SetBool("WallRun", true);
                PlayerRB.velocity = new Vector2(0, _wallclimbSpeed);
            }
            else if (PlayerMode.WallRun == playerMode)
            {
                Debug.Log("�Ǔo�����");
                PlayerAnimator.SetBool("WallRun", false);
                playerMode = PlayerMode.Normal;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayerAnimator.SetBool("IsGround", false);
            if (playerMode == PlayerMode.WallRun)
            {
                Debug.Log("�Ǔo�����");
                PlayerAnimator.SetBool("WallRun", false);
                playerMode = PlayerMode.Normal;
            }
        }
    }

    void Update()
    {
        //���E�ړ�
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != _lastHorizontalAxis)
        {
            if (horizontal != 0)
            {
                PlayerAnimator.SetBool("Walk", true);
            }
            else
            {
                PlayerAnimator.SetBool("Walk", false);
            }
        }

        if (horizontal != 0)
        {
            PlayerRB.velocity = new Vector2(_moveSpeed * horizontal, PlayerRB.velocity.y);
            transform.localScale = new Vector2(ScaleX * horizontal, transform.localScale.y);
        }
        else
        {
            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
        }
        _lastHorizontalAxis = horizontal;

        //�W�����v
        if (_inputSystem.Player.Jump.triggered && jumpCount < _jumpLimit)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

            PlayerAnimator.SetTrigger("Jump");

            jumpCount++;

            soundEffectManager.PlaySE(0);
        }

        //�ߐڍU��
        if (_inputSystem.Player.Attack.triggered)
        {
            StartCoroutine(Attack());
        }

        //�������U��
        if (_inputSystem.Player.Fire.triggered)
        {
            if (_bulletFireInterval + _bulletIntervalTimer < Time.time)
            {
                StartCoroutine(FireBullet());
            }
        }

        PlayerAnimator.SetFloat("SpeedY", PlayerRB.velocity.y);
    }

    private IEnumerator Attack()
    {
        PlayerAnimator.SetTrigger("Attack");
        AttackCollider.enabled = true;
        Debug.Log("�ߐ�");

        yield return new WaitForSeconds(0.2f);

        AttackCollider.enabled = false;
    }

    IEnumerator FireBullet()
    {
        PlayerAnimator.SetTrigger("Fire");
        Debug.Log("�������U��");
        _bulletIntervalTimer = Time.time;

        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Mathf.Sign(transform.localScale.x), 0);

        yield return new WaitForSeconds(2);

        Destroy(bullet);
    }

    public void HitDamage(float damage)
    {
        Debug.Log($"���ݑ̗͂�{_currentHealth}");
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Debug.Log("GameOver");
        }
        healthBar.fillAmount = _currentHealth / _maxHealth;
    }

    public void GetCatFood()
    {
        _haveCatFoodValue++;
    }
}
