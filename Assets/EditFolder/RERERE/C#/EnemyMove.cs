using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    GameObject PL;
    Vector2 _plPos;
    Vector2 _pos;

    /// <summary> プレイヤーが自分のどちら側にいるか？ </summary>
    float _axis;
    float _cooltime;
    [SerializeField] float _CT = 2;
    [SerializeField] AudioSource _mouseAudioSource;

    Rigidbody2D rb;
    [SerializeField] float _moveSpeed = 4;

    //常にプレイヤーを追いかける
    //一定範囲内に入ったら
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

        //自分の向いている方向にプレイヤーがいるならば
        if (_axis == transform.localScale.x)
        {

            //プレイヤーとの距離が８以下ならば
            if (Vector2.Distance(_pos, _plPos) < 8)
            {


                StartCoroutine(weit(2));

                //ｘ方向の加速度が設定した速度よりも遅かった場合？
                //else if (Mathf.Abs(rb.velocity.x) < _moveSpeed)
                // {
                //     //速度を直接代入
                //     rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
                // }
            }
            else if (Vector2.Distance(_pos, _plPos) < 16f)
            {
                //Debug.Log(Vector2.Distance(_pos,_plPos));
                rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
            }

            
        }
        //向いている方向にプレイヤーがいない場合かつ速度が一定以下の場合
        else if (Mathf.Abs(rb.velocity.x) < _moveSpeed)
        {
            //weit(1)を実行
            StartCoroutine(weit(1));
        }
        if (Vector2.Distance(_pos, _plPos) > 14)
        {
            //rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
        }


    }
    IEnumerator weit(int num)
    {
        
        //X軸の速度を０にする。
        //rb.velocity = new Vector2(0, rb.velocity.y);
        

        switch (num)
        {
            case 1:
                //向きをプレイヤーの方向に合わせる。
                transform.localScale = new Vector3(_axis, 1, 1);
                //Debug.Log("向きが変わった");
                break;

            case 2:
                //プレイヤーのいる方向に突進
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
