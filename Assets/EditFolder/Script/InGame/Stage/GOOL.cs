using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SceneChanger;
using UnityEngine.SceneManagement;

public class GOOL : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneNames[SceneKind.Result]);
        }
    }
}
