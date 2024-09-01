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

    [Tooltip("取得猫缶数")]
    float _haveCatFoodValue;

    [Header("プレイヤーの移動ステータス")]

    [SerializeField, Tooltip("並行の移動スピード")]
    float _moveSpeed = 5;
    [Space]
    [SerializeField, Tooltip("ジャンプ力")]
    float _jumpPower = 5;
    [SerializeField, Tooltip("ジャンプ回数")]
    int _jumpLimit = 1;
    [Space]
    [SerializeField, Tooltip("壁登り速度")]
    float _wallclimbSpeed = 3;

    int jumpCount;
    float ScaleX;
    float _lastHorizontalAxis;

    [Header("攻撃ステータス")]
    [SerializeField, Tooltip("攻撃判定のコライダー")]
    BoxCollider2D AttackCollider;
    [SerializeField, Tooltip("近接攻撃力")]
    float _attackDamage;
    [Space]
    [SerializeField, Tooltip("肉球弾丸のオブジェクト")]
    GameObject Bullet;
    [SerializeField, Tooltip("弾丸の攻撃力")]
    float _bulletDamage;
    [SerializeField, Tooltip("弾丸のスピード")]
    float _bulletSpeed;
    [SerializeField, Tooltip("発射のインターバル")]
    float _bulletFireInterval;
    float _bulletIntervalTimer;

    [Header("体力ステータス")]
    [SerializeField, Tooltip("体力バー")]
    Image healthBar;

    [SerializeField, Tooltip("体力")]
    float _maxHealth;
    [Tooltip("現在体力")]
    float _currentHealth;
    [SerializeField, Tooltip("無敵時間")]
    float _hitIntarval;
    float _hitIntervalTimer;

    [Header("プレイヤーのモード")]
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
        if (soundEffectManager == null) Debug.LogWarning("SoundEffectManagerがありません");
        _currentHealth = _maxHealth;
        if (healthBar != null)
            healthBar.fillAmount = _currentHealth / _maxHealth;

        ScaleX = transform.localScale.x;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //ダメージを受けた処理
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
        //地面に着いた時の処理
        if (collision.gameObject.CompareTag("Ground") && collision.contacts[0].normal.y >= 0)
        {
            if (jumpCount > 0)
            {
                jumpCount = 0;
            }

            Vector2 CollisionNormal = collision.contacts[0].normal;
            if ((Mathf.Abs(CollisionNormal.x) >= 0.01 ? (int)Mathf.Sign(CollisionNormal.x) * -1 : 0) == Input.GetAxisRaw("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)
            {
                Debug.Log("壁登り");
                if (PlayerMode.WallRun != playerMode)
                {
                    playerMode = PlayerMode.WallRun;
                }
                PlayerAnimator.SetBool("WallRun", true);
                PlayerRB.velocity = new Vector2(0, _wallclimbSpeed);
            }
            else if (PlayerMode.WallRun == playerMode)
            {
                Debug.Log("壁登り解除");
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
                Debug.Log("壁登り解除");
                PlayerAnimator.SetBool("WallRun", false);
                playerMode = PlayerMode.Normal;
            }
        }
    }

    void Update()
    {
        //左右移動
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

        //ジャンプ
        if (_inputSystem.Player.Jump.triggered && jumpCount < _jumpLimit)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

            PlayerAnimator.SetTrigger("Jump");

            jumpCount++;

            soundEffectManager.PlaySE(0);
        }

        //近接攻撃
        if (_inputSystem.Player.Attack.triggered)
        {
            StartCoroutine(Attack());
        }

        //遠距離攻撃
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
        Debug.Log("近接");

        yield return new WaitForSeconds(0.2f);

        AttackCollider.enabled = false;
    }

    IEnumerator FireBullet()
    {
        PlayerAnimator.SetTrigger("Fire");
        Debug.Log("遠距離攻撃");
        _bulletIntervalTimer = Time.time;

        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Mathf.Sign(transform.localScale.x), 0);

        yield return new WaitForSeconds(2);

        Destroy(bullet);
    }

    public void HitDamage(float damage)
    {
        Debug.Log($"現在体力は{_currentHealth}");
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
