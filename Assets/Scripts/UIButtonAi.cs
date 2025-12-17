using System;                                                    
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIButtonAi : MonoBehaviour
{
    public static Action<bool> SendAiButtonState;
    private Button _button;
    private TextMeshProUGUI _buttonText;
    private bool _buttonState = true;

    private void OnEnable()
    {
        _button.onClick.AddListener(PressButton);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(PressButton);
    }
    private void Awake()
    {
        _button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        if(PlayerPrefs.GetInt(Prefs.Prefs_VersusAiSet_Key, Prefs.Pref_VersusAiSet_DefaultValue) == 1)
        {
            _buttonState = true;
        }
        else
        {
            _buttonState = false;
        }
            SendAiButtonState?.Invoke(_buttonState);                

        SetAiButtonText(_buttonState);                           
    }

    private void PressButton()
    {
        _buttonState = !_buttonState;
        if(_buttonState)
        {
            PlayerPrefs.SetInt(Prefs.Prefs_VersusAiSet_Key, 1);
        }
        else
        {
            PlayerPrefs.SetInt(Prefs.Prefs_VersusAiSet_Key, 0);
        }
            SendAiButtonState?.Invoke(_buttonState);
        SetAiButtonText(_buttonState);
    }
    private void SetAiButtonText(bool state)
    {
        string buttonText = state ? "vs.\\nœ " : "vs.\\n»„ÓÍ";
        _buttonText.SetText(buttonText);
    }
}
