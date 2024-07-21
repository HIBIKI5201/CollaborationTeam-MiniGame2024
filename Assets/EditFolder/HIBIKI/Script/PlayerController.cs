using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのコンポーネント")]
    [SerializeField] Rigidbody2D PlayerRB;
    [SerializeField] SpriteRenderer PlayerRenderer;

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
    float Angle;

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
        WallRun
    }
    bool WallRunning;

    [Space]
    [SerializeField] Sprite NormalSprite;
    [SerializeField] Sprite WallRunSprite;

    [Header("他コンポーネント")]
    [SerializeField] Tilemap tilemap;

    void Start()
    {
        _currentHealth = _maxHealth;
        healthBar.fillAmount = _currentHealth / _maxHealth;

        ScaleX = transform.localScale.x;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && _hitIntervalTimer + _hitIntarval < Time.time)
        {
            _hitIntervalTimer = Time.time;
            HitDamage(1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
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
                    PlayerRenderer.sprite = WallRunSprite;
                }

                PlayerRB.velocity = new Vector2(0, _wallclimbSpeed);
            }
            else if (PlayerMode.WallRun == playerMode)
            {
                Debug.Log("壁登り解除");

                playerMode = PlayerMode.Normal;
                PlayerRenderer.sprite = NormalSprite;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && WallRunning)
        {
            Debug.Log("壁登り解除");

            WallRunning = false;
            playerMode = PlayerMode.Normal;
            PlayerRenderer.sprite = NormalSprite;
        }
    }

    private IEnumerator Attack()
    {
        AttackCollider.enabled = true;

        Debug.Log("近接");

        yield return new WaitForSeconds(0.2f);

        AttackCollider.enabled = false;
    }


    void Update()
    {
        //左右移動
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            PlayerRB.velocity = new Vector2(_moveSpeed * horizontal, PlayerRB.velocity.y);
            transform.localScale = new Vector2(ScaleX * horizontal, transform.localScale.y);

            Angle = horizontal;
        }
        else
        {
            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < _jumpLimit)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

            jumpCount++;
        }

        //伏せ
        if (Input.GetKeyDown(KeyCode.S) && jumpCount == 0)
        {
            Debug.Log("伏せ");
        }

        //近接攻撃
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            StartCoroutine(Attack());
        }

        //遠距離攻撃
        if (Input.GetKey(KeyCode.Return))
        {
            if (_bulletFireInterval + _bulletIntervalTimer < Time.time)
            {
               StartCoroutine(FireBullet());
            }
        }
    }

    IEnumerator FireBullet()
    {
        Debug.Log("遠距離攻撃");
        _bulletIntervalTimer = Time.time;

        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Angle, 0);

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
