using UnityEngine;
using UnityEngine.UI;

public class HPsystem : MonoBehaviour
{

    // Start is called bfloat _hp = 5;//プレイヤーのHP
    float _maxHP;//プレイヤーのHP上限
    [SerializeField] Image HPBar;//HPbarの画像を格納する箱efore the first frame update
    float _hp = 5;//プレイヤーのHP
    void Start()
    {
        _maxHP = _hp;//HPの上限設定　代入
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _hp--;//HPを１減らす
        HPBar.fillAmount = _hp / _maxHP;//HPbarをHP/MaxHPに大きさを変更
    }
}
