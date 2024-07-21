using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̃R���|�[�l���g")]
    [SerializeField] Rigidbody2D PlayerRB;
    [SerializeField] SpriteRenderer PlayerRenderer;

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
                Debug.Log("�Ǔo��");
                if (PlayerMode.WallRun != playerMode)
                {
                    playerMode = PlayerMode.WallRun;
                    PlayerRenderer.sprite = WallRunSprite;
                }

                PlayerRB.velocity = new Vector2(0, _wallclimbSpeed);
            }
            else if (PlayerMode.WallRun == playerMode)
            {
                Debug.Log("�Ǔo�����");

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
        Debug.Log("�������U��");
        _bulletIntervalTimer = Time.time;

        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Angle, 0);

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
