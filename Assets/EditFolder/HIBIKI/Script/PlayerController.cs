using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのコンポーネント")]
    [SerializeField] Rigidbody2D PlayerRB;
    [SerializeField] SpriteRenderer PlayerRenderer;

    [Header("プレイヤーのステータス")]
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

    [Header("体力ステータス")]
    [SerializeField, Tooltip("体力")]
    float _maxHealth;
    [HideInInspector, Tooltip("現在体力")]
    public float _currentHealth;
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
        ScaleX = transform.localScale.x;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && _hitIntervalTimer + _hitIntarval < Time.time)
        {
            _hitIntervalTimer = Time.time;
            _currentHealth--;
            Debug.Log($"現在体力は{_currentHealth}");
            Debug.Log($"{_hitIntervalTimer + _hitIntarval}と{Time.time}");
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


            Vector3 hitPosition = collision.collider.ClosestPoint(transform.position);
            Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);
            cellPosition = cellPosition + new Vector3Int((Mathf.Abs(CollisionNormal.x) >= 0.01) ? (int)Mathf.Sign(CollisionNormal.x) * -1 : 0, (Mathf.Abs(CollisionNormal.y) >= 0.01) ? (int)Mathf.Sign(CollisionNormal.y) * -1 : 0, 0);
            TileBase tile = tilemap.GetTile(cellPosition);

            if (tile != null)
            {
                switch (tile.name)
                {
                    case "Wall":

                        break;

                    case "Ground":

                        break;

                    default:

                        Debug.Log("設定されていないタイルに接触");

                        break;
                }
            }
            else
            {
                Debug.Log($"タイルが見つからない\n座標：{cellPosition}\n法線方向：{CollisionNormal}\n\n{hitPosition}と{new Vector3Int((int)Mathf.Sign(CollisionNormal.x) * -1, (int)Mathf.Sign(CollisionNormal.y) * -1, 0)}");
            }

            //
            if ((Mathf.Abs(CollisionNormal.x) >= 0.01 ? (int)Mathf.Sign(CollisionNormal.x) * -1 : 0) == Input.GetAxisRaw("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)
            {
                WallRun();
            }
            else if (WallRunning)
            {
                Debug.Log("壁登り解除");

                WallRunning = false;
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

    private void WallRun()
    {
        Debug.Log("壁登り");
        if (!WallRunning)
        {
            WallRunning = true;
            playerMode = PlayerMode.WallRun;
            PlayerRenderer.sprite = WallRunSprite;
        }

        PlayerRB.velocity = new Vector2(0, _wallclimbSpeed);
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("遠距離攻撃");

            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Angle, 0);
        }
    }

}
