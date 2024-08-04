using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public enum SceneKind
    {
        Title,
        Stage1,
        Result
    }

    public static Dictionary<SceneKind, string> SceneNames = new Dictionary<SceneKind, string>()
    {
        {SceneKind.Title, "TitleScene2" },
        {SceneKind.Stage1, "GOscene" },
        {SceneKind.Result, "ResultScene" }
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && StartCanvas._isStage == true)
        {
            GetStage1();
        }
    }

    private void Stage1()
    {
        SceneManager.LoadScene(SceneNames[SceneKind.Stage1]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneNames[SceneKind.Result]);
        }
    }

    /// <summary>
    /// シーンを変えるメソッド
    /// </summary>
    /// <param name="sceneKind">SceneChanger.SceneKindのEnum</param>
    static void SceneChange(SceneKind sceneKind)
    {
        SceneManager.LoadScene(SceneNames[sceneKind]);
    }

    public void GetStage1()
    {
        Invoke(nameof(Stage1), 0.5f);
    }

    private void Title()
    {
        SceneManager.LoadScene(SceneNames[SceneKind.Title]);
    }

    public void GetTitle()
    {
        Invoke(nameof(Title), 0.5f);
    }
}

