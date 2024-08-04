using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneChangeメソッドでシーンをロードしてください
/// </summary>
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

    /// <summary>
    /// シーンを変えるメソッド
    /// </summary>
    /// <param name="sceneKind">SceneChanger.SceneKindのEnum</param>
    static public void SceneChange(SceneKind sceneKind)
    {
        SceneManager.LoadScene(SceneNames[sceneKind]);
    }
}

