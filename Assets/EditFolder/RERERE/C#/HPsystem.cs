using UnityEngine;
using UnityEngine.UI;

public class HPsystem : MonoBehaviour
{

    // Start is called bfloat _hp = 5;//�v���C���[��HP
    float _maxHP;//�v���C���[��HP���
    [SerializeField] Image HPBar;//HPbar�̉摜���i�[���锠efore the first frame update
    float _hp = 5;//�v���C���[��HP
    void Start()
    {
        _maxHP = _hp;//HP�̏���ݒ�@���
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _hp--;//HP���P���炷
        HPBar.fillAmount = _hp / _maxHP;//HPbar��HP/MaxHP�ɑ傫����ύX
    }
}
