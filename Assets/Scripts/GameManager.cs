using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Ссылки на UI элементы и компоненты
    [SerializeField] private GameObject _AIToggleButton;
    [SerializeField] private TextMeshProUGUI _playerOneScoreText;
    [SerializeField] private TextMeshProUGUI _playerTwoScoreText;
    [SerializeField] private Sprite _rockImage;
    [SerializeField] private Sprite _paperImage;
    [SerializeField] private Sprite _scissorsImage;
    [SerializeField] private Image _playerOneSelectedImage;
    [SerializeField] private Image _playerTwoSelectedImage;
    [SerializeField] private Animator _playerOneSelectedImageAnimator;
    [SerializeField] private Animator _playerTwoSelectedImageAnimator;
    [SerializeField] private Animator _playerOneChoiceAnimator;
    [SerializeField] private Animator _playerTwoChoiceAnimator;
    [SerializeField] private GameObject _endRoundPanel;
    [SerializeField] private TextMeshProUGUI _endRoundMessageText;

    // Переменные состояния игры
    private int _playerOneCurrentScore = 0;
    private int _playerTwoCurrentScore = 0;
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

    private void Awake()
    {
        SetScoreText(_playerOneScoreText, _playerOneCurrentScore, _playerTwoCurrentScore);
        SetScoreText(_playerTwoScoreText, _playerTwoCurrentScore, _playerOneCurrentScore);
        _AIToggleButtonText = _AIToggleButton.GetComponentInChildren<TextMeshProUGUI>(); // Получение текста кнопки
    }

    private void Start()
    {
        SetAiButtonText(_isVersusAi);
        StartGame(); // Запуск первого раунда
    }

    public void StartGame()
    {
        _endRoundMessageText.SetText("");

        _playerOneChoice = Figure.NONE;
        _playerTwoChoice = Figure.NONE;
        _isPlayerOneSelected = false;
        _isPlayerTwoSelected = false;

        _isRoundFinished = false;

        _playerOneChoiceAnimator.Play("PlayerOneChoiceMoveForward");
        _playerTwoChoiceAnimator.Play("PlayerTwoChoiceMoveForward");

        _playerOneSelectedImageAnimator.Play("PlayerOneSelectedImageMoveBackward");
        _playerTwoSelectedImageAnimator.Play("PlayerTwoSelectedImageMoveBackward");
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
                _playerTwoChoice = (Figure)Random.Range(0, 3);
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
            _endRoundMessageText.SetText("Ничья!");
        }
        else if (_playerOneChoice == Figure.ROCK && _playerTwoChoice == Figure.SCISSORS
            || _playerOneChoice == Figure.SCISSORS && _playerTwoChoice == Figure.PAPER
            || _playerOneChoice == Figure.PAPER && _playerTwoChoice == Figure.ROCK)
        {
            _endRoundMessageText.SetText("Игрок 1 победил!");
            _playerOneCurrentScore++;
        }
        else
        {
            _endRoundMessageText.SetText("Игрок 2 победил!");
            _playerTwoCurrentScore++;
        }

        _endRoundPanel.SetActive(true);

        _playerOneChoiceAnimator.Play("PlayerOneChoiceMoveBackward");
        _playerTwoChoiceAnimator.Play("PlayerTwoChoiceMoveBackward");

        _playerOneSelectedImageAnimator.Play("PlayerOneSelectedImageMoveForward");
        _playerTwoSelectedImageAnimator.Play("PlayerTwoSelectedImageMoveForward");

        SetScoreText(_playerOneScoreText, _playerOneCurrentScore, _playerTwoCurrentScore);
        SetScoreText(_playerTwoScoreText, _playerTwoCurrentScore, _playerOneCurrentScore);
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

    private void SetScoreText(TextMeshProUGUI text, int player1Score, int player2Score)
    {
        text.SetText($"{player1Score} / {player2Score}");
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