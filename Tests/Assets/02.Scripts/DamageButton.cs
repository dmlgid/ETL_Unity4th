using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour
{
    [SerializeField] private Player _player;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(DepletePlayerHP5);
        // ���ٽ� (�͸��Լ�) 
        // �����Ϸ��� �Ǵ� �� �� �ִ� �κе��� �����ϰ� �̸��� ������� �ʴ� ������ �Լ���
        _button.onClick.AddListener(() => _player.hp -= 5.0f);
    }

    private void DepletePlayerHP5()
    {
        _player.hp -= 5.0f;
    }
}