using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float _countDown = 3;
    private CameraController _cc;
    
    public TextMeshProUGUI _timerText;
    public Image _moveKeys;
    public Image _gravityFlipKey;

    void Start()
    {
        if (Camera.main != null)
            _cc = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        if (_countDown > -2f)
            _countDown -= Time.deltaTime;
        
        if (_countDown < 2f)
            _timerText.text = "2";
        
        if (_countDown < 1f)
            _timerText.text = "1";

        if (_countDown < 0f)
        {
            _timerText.text = "GO!";
            _cc._move = true;
        }

        if (_countDown < -1f)
        {
            _timerText.text = "";
            _moveKeys.enabled = false;
            _gravityFlipKey.enabled = false;
        }
    }
}