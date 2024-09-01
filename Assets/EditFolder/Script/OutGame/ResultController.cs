using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Text _timeText;

    private void Start()
    {
        if(_scoreText!=null)
        {
            _scoreText.text = $"スコア　・・・　{ScoreManager.Score.ToString("0000")}";
        }
        if(_timeText!=null)
        {
            _timeText.text = $"タイム　・・・　{ScoreManager.ResultTime.ToString()}";
        }
    }
    public void ButtonClicked()
    {
        SceneChanger.SceneChange(SceneChanger.SceneKind.Title);
    }
}
