using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyspawner : MonoBehaviour
{
    [Header("出現する敵のプレハブ")]
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Player;
    
    Vector2 pos;
    Vector2 PLpos;
   // private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
         pos = this.transform.position;
         PLpos = Player.transform.position;
        Instantiate(Enemy, pos,transform.rotation);

        InvokeRepeating("SpawnEnemy", 0, 3);
    }
    void SpawnEnemy()
    {
        pos = this.transform.position;
        PLpos = Player.transform.position;
       
        
        if (Vector2.Distance(pos, PLpos) < 5)
        {
            Debug.Log("A");
            Instantiate(Enemy, pos,transform.rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        pos = this.transform.position;
        PLpos = Player.transform.position;
    }
}
