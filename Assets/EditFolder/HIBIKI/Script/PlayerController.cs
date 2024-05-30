using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのコンポーネント")]
    [SerializeField] Rigidbody2D PlayerRB;
    [SerializeField] SpriteRenderer PlayerRenderer;

    [Header("プレイヤーのステータス")]
    [SerializeField] float _moveSpeed = 5;
    [Space]
    [SerializeField] float _jumpPower = 5;
    [SerializeField] int _jumpLimit = 1;
    [Space]
    [SerializeField] float _wallclimbSpeed = 3;
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
        ScaleX = transform.localScale.x;
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
                switch(tile.name)
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

            //壁登り
            if ((Mathf.Abs(CollisionNormal.x) >= 0.01 ? (int)Mathf.Sign(CollisionNormal.x) * -1 : 0) == Input.GetAxisRaw("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)
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
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (WallRunning)
            {
                Debug.Log("壁登り解除");

                WallRunning = false;
                playerMode = PlayerMode.Normal;
                PlayerRenderer.sprite = NormalSprite;
            }
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
        } else
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
