using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action<State> SendState;
    // Ссылки на UI элементы и компоненты
    [SerializeField] private GameObject _AIToggleButton;
    
    [SerializeField] private Sprite _rockImage;
    [SerializeField] private Sprite _paperImage;
    [SerializeField] private Sprite _scissorsImage;
    [SerializeField] private Image _playerOneSelectedImage;
    [SerializeField] private Image _playerTwoSelectedImage;
    
    [SerializeField] private GameObject _endRoundPanel;
    

    // Переменные состояния игры
    
    private Figure _playerOneChoice = Figure.NONE;
    private bool _isPlayerOneSelected = false;
    private Figure _playerTwoChoice = Figure.NONE;
    private bool _isPlayerTwoSelected = false;
    private bool _isRoundFinished = false;
    private bool _isVersusAi = true;
    private TextMeshProUGUI _AIToggleButtonText;

    private void OnEnable()
    {
        ButtonChoice.SendChoice += OnChoice; // Подписка на событие выбора
    }

    private void OnDisable()
    {
        ButtonChoice.SendChoice -= OnChoice; // Отписка от события
    }
    
    private void Start()
    {
        _AIToggleButtonText = _AIToggleButton.GetComponentInChildren<TextMeshProUGUI>(); 
        SetAiButtonText(_isVersusAi);
        StartGame(); // Запуск первого раунда
    }

    public void StartGame()
    {
        _playerOneChoice = Figure.NONE;
        _playerTwoChoice = Figure.NONE;
        _isPlayerOneSelected = false;
        _isPlayerTwoSelected = false;

        _isRoundFinished = false;

        SendState?.Invoke(State.STARTROUND);
    }

    public void OnChoice(Figure choice, bool isPlayerOne)
    {
        if (_isRoundFinished) return;

        if (isPlayerOne)
        {
            _playerOneChoice = choice;
            _isPlayerOneSelected = true;
            SetSelectedImage(choice, _playerOneSelectedImage);

            if (_isVersusAi)
            {                
                _playerTwoChoice = (Figure)UnityEngine.Random.Range(0, 3);
                _isPlayerTwoSelected = true;
                SetSelectedImage(_playerTwoChoice, _playerTwoSelectedImage);
            }
        }
        else
        {
            if (!_isVersusAi)
            {
                _playerTwoChoice = choice;
                _isPlayerTwoSelected = true;
                SetSelectedImage(choice, _playerTwoSelectedImage);
            }
        }

        if (_isVersusAi)
        {
            if (_isPlayerOneSelected && _isPlayerTwoSelected)
            {
                _isRoundFinished = true;
                DetermineWinner();
            }
        }
        else
        {
            if (_isPlayerOneSelected && _isPlayerTwoSelected)
            {
                _isRoundFinished = true;
                DetermineWinner();
            }
        }
    }

    private void DetermineWinner()
    {
        if (_playerOneChoice == _playerTwoChoice)
        {          
            SendState?.Invoke(State.DRAW);
        }
        else if (_playerOneChoice == Figure.ROCK && _playerTwoChoice == Figure.SCISSORS
            || _playerOneChoice == Figure.SCISSORS && _playerTwoChoice == Figure.PAPER
            || _playerOneChoice == Figure.PAPER && _playerTwoChoice == Figure.ROCK)
        {           
            SendState?.Invoke(State.PLAYER1WIN);
        }
        else
        {           
            SendState?.Invoke(State.PLAYER2WIN);
        }

        _endRoundPanel.SetActive(true);               
        
    }

    private void SetSelectedImage(Figure figure, Image image)
    {
        switch (figure)
        {
            case Figure.ROCK: image.sprite = _rockImage; break;
            case Figure.SCISSORS: image.sprite = _scissorsImage; break;
            case Figure.PAPER: image.sprite = _paperImage; break;
        }
    }

    

    private void SetAiButtonText(bool state)
    {
        if (state)
        {
            _AIToggleButtonText.SetText("vs.\nПК");
        }
        else
        {
            _AIToggleButtonText.SetText("vs.\nИгрок");
        }
    }

    public void AiButtonToggle()
    {
        _isVersusAi = !_isVersusAi;
        SetAiButtonText(_isVersusAi);
    }
}