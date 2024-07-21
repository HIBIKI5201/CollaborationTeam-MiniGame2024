using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject stageCanvas;
    [SerializeField] GameObject titleCanvas;

    AudioSource _audio;
    
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
        SceneManager.LoadScene("GOscene");
    }

    public void GetStage1()
    {
        Invoke(nameof(Stage1), 0.5f);
    }

    public void Audio()
    {
        _audio.Play();
    }
}
