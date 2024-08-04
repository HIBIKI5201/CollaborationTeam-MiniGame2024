using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneChange���\�b�h�ŃV�[�������[�h���Ă�������
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
    /// �V�[����ς��郁�\�b�h
    /// </summary>
    /// <param name="sceneKind">SceneChanger.SceneKind��Enum</param>
    static public void SceneChange(SceneKind sceneKind)
    {
        SceneManager.LoadScene(SceneNames[sceneKind]);
    }
}

