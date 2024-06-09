using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject stageCanvas;
    [SerializeField] GameObject titleCanvas;

    // Start is called before the first frame update
    void Start()
    {
        stageCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && titleCanvas.active)
        {
            StageSelectUiActive();
            Debug.Log("aa");
        }
    }

    public void StageSelectUiActive()
    {
        stageCanvas.SetActive(true);
        titleCanvas.SetActive(false);
    }
    public void Stage1 ()
    {
        SceneManager.LoadScene("HIBIKIscene");
    }
}
