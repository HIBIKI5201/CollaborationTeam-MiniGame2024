using UnityEngine;

public class karasuMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _moveSpeed;
    [SerializeField] GameObject PL;
    Rigidbody2D rb;
    Vector2 _plPos;
    Vector2 _pos;
    float _axis;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _plPos = PL.transform.position;
        _pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // rb.velocity = Vector2.right;
        _axis = Mathf.Sign(_plPos.x - _pos.x);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = new Vector2(_axis * _moveSpeed, rb.velocity.y);
            Debug.Log("test1");
        }
    }
}
