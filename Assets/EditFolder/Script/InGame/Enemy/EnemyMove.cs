using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMove : MonoBehaviour
{
    GameObject PL;

    /// <summary> �v���C���[�������̂ǂ��瑤�ɂ��邩�H </summary>
    float _axis;
    bool _isDash;

    AudioSource _mouseAudioSource;

    Rigidbody2D rb;
    [SerializeField] float _moveSpeed = 4;

    //��Ƀv���C���[��ǂ�������
    //���͈͓��ɓ�������
    void Start()
    {
        _mouseAudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        PL = FindAnyObjectByType<PlayerController>().gameObject;
    }
    void Update()
    {
        _axis = Mathf.Sign(PL.transform.position.x - transform.position.x);
        //�����̌����Ă�������Ƀv���C���[������Ȃ��
        if (_axis == transform.localScale.x)
        {
            //�v���C���[�Ƃ̋������W�ȉ��Ȃ��
            if (Vector2.Distance(transform.position, PL.transform.position) < 8)
            {
                StartCoroutine(Dash());
            }
            else if (Vector2.Distance(transform.position, PL.transform.position) < 16f)
            {
                rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
            }
        }
        //�����Ă�������Ƀv���C���[�����Ȃ��ꍇ�����x�����ȉ��̏ꍇ
        else if (Mathf.Abs(rb.velocity.x) < _moveSpeed)
        {
            transform.localScale = new Vector3(_axis, 1, 1);
        }
    }

    IEnumerator Dash()
    {
        //�v���C���[�̂�������ɓːi
        _mouseAudioSource.Play();
        float axis = _axis;
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(1);
        rb.velocity = new Vector2(axis * _moveSpeed * 3, 0);
        yield return new WaitForSeconds(3);
        rb.velocity = rb.velocity.normalized * _moveSpeed;
    }

    public void HitDamage(float damage)
    {
        if (damage > 0)
        {
            ScoreManager.AddScore(100);
            Destroy(gameObject);
        }
    }
}
