using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    ItemKind kind;

    enum ItemKind
    {
        heal,
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (kind)
            {
                case ItemKind.heal:
                    collision.GetComponent<PlayerController>().HitDamage(-1);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
