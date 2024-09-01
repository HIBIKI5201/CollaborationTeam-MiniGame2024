using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static int _score;
    public static int Score { get { return  _score; } }

    static int _bell;
    public static int BellValue { get { return _bell; } }

    static float _startTime;

    public static ScoreTime ResultTime;
    public static void AddScore(int score)
    {
        _score += score;
    }

    public static void AddBell()
    {
        _bell++;
    }

    private void Start()
    {
        _score = 0;
        _startTime = Time.time;
    }

    static public void StopTimer()
    {
        ResultTime = new ScoreTime((int)Time.time - (int)_startTime);
        Debug.Log($"ƒ^ƒCƒ€‚Í{ResultTime.ToString()}");
    }

    public struct ScoreTime
    {
        public int minute;
        public int second;

        public ScoreTime(int secondTime)
        {
            minute = secondTime / 60;
            second = secondTime % 60;
        }

        public override string ToString()
        {
            return $"{minute.ToString("00")}:{second.ToString("00")}";
        }
    }
}
