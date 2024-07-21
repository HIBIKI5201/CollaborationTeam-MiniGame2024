using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    GameObject PL;
    Vector2 _plPos;
    Vector2 _pos;

    /// <summary> �v���C���[�������̂ǂ��瑤�ɂ��邩�H </summary>
    float _axis;
    float _cooltime;
    [SerializeField] float _CT = 2;
    [SerializeField] AudioSource _mouseAudioSource;

    Rigidbody2D rb;
    [SerializeField] float _moveSpeed = 4;

    //��Ƀv���C���[��ǂ�������
    //���͈͓��ɓ�������
    //
    //
    //

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PL=GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _plPos = PL.transform.position;
        _pos = transform.position;
        _axis = Mathf.Sign(_plPos.x - _pos.x);
        //Debug.Log(rb.velocity.x);
        _cooltime -= Time.deltaTime;

        //�����̌����Ă�������Ƀv���C���[������Ȃ��
        if (_axis == transform.localScale.x)
        {

            //�v���C���[�Ƃ̋������W�ȉ��Ȃ��
            if (Vector2.Distance(_pos, _plPos) < 8)
            {


                StartCoroutine(weit(2));

                //�������̉����x���ݒ肵�����x�����x�������ꍇ�H
                //else if (Mathf.Abs(rb.velocity.x) < _moveSpeed)
                // {
                //     //���x�𒼐ڑ��
                //     rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
                // }
            }
            else if (Vector2.Distance(_pos, _plPos) < 16f)
            {
                //Debug.Log(Vector2.Distance(_pos,_plPos));
                rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
            }

            
        }
        //�����Ă�������Ƀv���C���[�����Ȃ��ꍇ�����x�����ȉ��̏ꍇ
        else if (Mathf.Abs(rb.velocity.x) < _moveSpeed)
        {
            //weit(1)�����s
            StartCoroutine(weit(1));
        }
        if (Vector2.Distance(_pos, _plPos) > 14)
        {
            //rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
        }


    }
    IEnumerator weit(int num)
    {
        
        //X���̑��x���O�ɂ���B
        //rb.velocity = new Vector2(0, rb.velocity.y);
        

        switch (num)
        {
            case 1:
                //�������v���C���[�̕����ɍ��킹��B
                transform.localScale = new Vector3(_axis, 1, 1);
                //Debug.Log("�������ς����");
                break;

            case 2:
                //�v���C���[�̂�������ɓːi
                _mouseAudioSource.Play();
                float axis = _axis;
                rb.velocity = new Vector2(0, rb.velocity.y);
                yield return new WaitForSeconds(1);
                rb.velocity = new Vector2(axis * _moveSpeed * 3, 0);

                break;
        }

    }

    public void HitDamage(float damage)
    {
        if (damage > 0)
        {
            ScoreManager._score += 100;
            Destroy(gameObject);
        }
    }

}
