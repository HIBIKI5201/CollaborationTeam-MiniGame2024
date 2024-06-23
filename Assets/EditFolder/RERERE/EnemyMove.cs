using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] GameObject PL;
    Vector2 _plPos;
    Vector2 _pos;

    /// <summary> �v���C���[�������̂ǂ��瑤�ɂ��邩�H </summary>
    float _axis;
    float _cooltime;
    [SerializeField] float _CT = 2;

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
    }

    // Update is called once per frame
    void Update()
    {
        _plPos = PL.transform.position;
        _pos = transform.position;
        _axis = Mathf.Sign(_plPos.x - _pos.x);
        Debug.Log(rb.velocity.x);
        _cooltime -= Time.deltaTime;

        //�����̌����Ă�������Ƀv���C���[������Ȃ��
        if (_axis == transform.localScale.x)
        {

            //�v���C���[�Ƃ̋������W�ȉ��Ȃ��
            if (Vector2.Distance(_pos, _plPos) < 8)
            {

                //�_�b�V��������葬�x��PL�Ɍ������悤�ɂ�����
                //�_�b�V�����O�Ɉ�U��~
                //�N�[���^�C�����O�ȉ��̎��Q�����s
                if (_cooltime < 0)
                {

                    StartCoroutine(weit(2));
                }
                //�������̉����x���ݒ肵�����x�����x�������ꍇ�H
                //if (Mathf.Abs(rb.velocity.x) < _moveSpeed)
                //{
                //    //���x�𒼐ڑ��
                //    rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
                //}
            }
            else
            {
                
                rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
            }
        }
        //�����Ă�������Ƀv���C���[�����Ȃ��ꍇ�����x�����ȉ��̏ꍇ
        else if (Mathf.Abs(rb.velocity.x) <= _moveSpeed)
        {
            //weit(1)�����s
            StartCoroutine(weit(1));
        }


    }
    IEnumerator weit(int num)
    {
        _cooltime = _CT;
        //X���̑��x���O�ɂ���B
        //rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(1);

        switch (num)
        {
            case 1:
                //�������v���C���[�̕����ɍ��킹��B
                transform.localScale = new Vector3(_axis, 1, 1);
                //Debug.Log("�������ς����");
                break;

            case 2:
                //�v���C���[�̂�������ɓːi
                
                rb.velocity = new Vector2(0, rb.velocity.y);
                yield return new WaitForSeconds(1);
                rb.AddForce(_axis * new Vector2(_moveSpeed,0)*2, ForceMode2D.Impulse);
                
                break;
        }

    }

}
