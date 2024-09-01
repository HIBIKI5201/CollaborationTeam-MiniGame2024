using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Text _timeText;

    [SerializeField] GameObject _bellOne;
    [SerializeField] GameObject _bellTwo;
    [SerializeField] GameObject _bellThree;

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
        StartCoroutine(BellAnim());
    }
    public void ButtonClicked()
    {
        SceneChanger.SceneChange(SceneChanger.SceneKind.Title);
    }
    IEnumerator BellAnim()
    {
        if (ScoreManager.BellValue < 1)
        {
            yield break;
        }
        _bellOne.GetComponent<Animator>().Play("BellAnime");
        yield return new WaitForSeconds(0.5f);
        if (ScoreManager.BellValue < 2)
        {
            yield break;
        }
        _bellTwo.GetComponent<Animator>().Play("BellAnime");
        yield return new WaitForSeconds(0.5f);
        if (ScoreManager.BellValue < 3)
        {
            yield break;
        }
        _bellThree.GetComponent<Animator>().Play("BellAnime");
    }
}
