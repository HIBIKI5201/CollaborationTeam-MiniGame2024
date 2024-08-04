using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SceneChanger;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    Button _button1;

    private void Start()
    {
        _button1.onClick.AddListener(() => StartCoroutine(LoadSceneInTitle(SceneKind.Stage1)));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && StartCanvas._isStage == true)
        {
            StartCoroutine(LoadSceneInTitle(SceneKind.Stage1));
        }
    }

    IEnumerator LoadSceneInTitle(SceneKind kind)
    {
        yield return new WaitForSeconds(0.5f);
        SceneChanger.SceneChange(kind);
    }
}
