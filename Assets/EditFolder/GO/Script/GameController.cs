using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject stageCanvas;
    [SerializeField] GameObject titleCanvas;

    AudioSource _audio;

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

    void Start()
    {
        stageCanvas.SetActive(false);
        _audio = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && stageCanvas.active)
        {
            _audio.Play();
            GetStage1();
        }

        if (Input.GetKeyDown(KeyCode.Return) && titleCanvas.active)
        {
            _audio.Play();
            GetStageSelectUiActive();
            Debug.Log("aa");
        }
    }

    private void StageSelectUiActive()
    {
        stageCanvas.SetActive(true);
        titleCanvas.SetActive(false);
    }

    public void GetStageSelectUiActive()
    {
        Invoke(nameof(StageSelectUiActive), 0.5f);
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

    static void SceneChange(SceneKind sceneKind)
    {
        SceneManager.LoadScene(SceneNames[sceneKind]);
    }

    public void GetStage1()
    {
        Invoke(nameof(Stage1), 0.5f);
    }

    public void Audio()
    {
        _audio.Play();
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
