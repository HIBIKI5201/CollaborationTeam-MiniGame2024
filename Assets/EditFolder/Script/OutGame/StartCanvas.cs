using UnityEngine;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject stageCanvas;

    /// <summary></summary>
    public static bool _isStage = false;

    AudioSource _audio;

    void Start()
    {
        stageCanvas.SetActive(false);
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && titleCanvas)
        {
            _audio.Play();
            GetStageSelectUiActive();
            Debug.Log("StageCanvasアクティブ");
            _isStage = true;
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
}
