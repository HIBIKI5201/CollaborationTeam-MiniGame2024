using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int _score;
    public static void AddScore(int score)
    {
        _score += score;
    }
}
