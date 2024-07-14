using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̃R���|�[�l���g")]
    [SerializeField] Rigidbody2D PlayerRB;
    [SerializeField] SpriteRenderer PlayerRenderer;

    [Header("�v���C���[�̃X�e�[�^�X")]
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
    float Angle;

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

    [Header("�̗̓X�e�[�^�X")]
    [SerializeField, Tooltip("�̗�")]
    float _maxHealth;
    [HideInInspector, Tooltip("���ݑ̗�")]
    public float _currentHealth;
    [SerializeField, Tooltip("���G����")]
    float _hitIntarval;
    float _hitIntervalTimer;

    [Header("�v���C���[�̃��[�h")]
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

    [Header("���R���|�[�l���g")]
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
            Debug.Log($"���ݑ̗͂�{_currentHealth}");
            Debug.Log($"{_hitIntervalTimer + _hitIntarval}��{Time.time}");
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

                        Debug.Log("�ݒ肳��Ă��Ȃ��^�C���ɐڐG");

                        break;
                }
            }
            else
            {
                Debug.Log($"�^�C����������Ȃ�\n���W�F{cellPosition}\n�@�������F{CollisionNormal}\n\n{hitPosition}��{new Vector3Int((int)Mathf.Sign(CollisionNormal.x) * -1, (int)Mathf.Sign(CollisionNormal.y) * -1, 0)}");
            }

            //
            if ((Mathf.Abs(CollisionNormal.x) >= 0.01 ? (int)Mathf.Sign(CollisionNormal.x) * -1 : 0) == Input.GetAxisRaw("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)
            {
                WallRun();
            }
            else if (WallRunning)
            {
                Debug.Log("�Ǔo�����");

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
            Debug.Log("�Ǔo�����");

            WallRunning = false;
            playerMode = PlayerMode.Normal;
            PlayerRenderer.sprite = NormalSprite;
        }
    }

    private void WallRun()
    {
        Debug.Log("�Ǔo��");
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

        Debug.Log("�ߐ�");

        yield return new WaitForSeconds(0.2f);

        AttackCollider.enabled = false;
    }


    void Update()
    {
        //���E�ړ�
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

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < _jumpLimit)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

            jumpCount++;
        }

        //����
        if (Input.GetKeyDown(KeyCode.S) && jumpCount == 0)
        {
            Debug.Log("����");
        }

        //�ߐڍU��
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            StartCoroutine(Attack());
        }

        //�������U��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("�������U��");

            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Angle, 0);
        }
    }

}
