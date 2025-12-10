using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Перечисление возможных фигур в игре
public enum Figure { ROCK, SCISSORS, PAPER, NONE }

public class GameManager : MonoBehaviour
{
    // Ссылки на UI элементы и компоненты
    [SerializeField] private GameObject _AIToggleButton; // Кнопка переключения режима
    [SerializeField] private TextMeshProUGUI _playerOneScoreText; // Текст счета игрока 1
    [SerializeField] private TextMeshProUGUI _playerTwoScoreText; // Текст счета игрока 2
    [SerializeField] private Sprite _rockImage; // Спрайт камня
    [SerializeField] private Sprite _paperImage; // Спрайт бумаги
    [SerializeField] private Sprite _scissorsImage; // Спрайт ножниц
    [SerializeField] private Image _playerOneSelectedImage; // Изображение выбора игрока 1
    [SerializeField] private Image _playerTwoSelectedImage; // Изображение выбора игрока 2
    [SerializeField] private Animator _playerOneSelectedImageAnimator; // Аниматор изображения игрока 1
    [SerializeField] private Animator _playerTwoSelectedImageAnimator; // Аниматор изображения игрока 2
    [SerializeField] private Animator _playerOneChoiceAnimator; // Аниматор UI выбора игрока 1
    [SerializeField] private Animator _playerTwoChoiceAnimator; // Аниматор UI выбора игрока 2
    [SerializeField] private GameObject _endRoundPanel; // Панель окончания раунда
    [SerializeField] private TextMeshProUGUI _endRoundMessageText; // Текст сообщения о результате раунда

    // Переменные состояния игры
    private int _playerOneCurrentScore = 0; // Текущий счет игрока 1
    private int _playerTwoCurrentScore = 0; // Текущий счет игрока 2 (исправлена опечатка в названии)
    private Figure _playerOneChoice = Figure.NONE; // Выбор игрока 1
    private bool _isPlayerOneSelected = false; // Флаг выбора игрока 1
    private Figure _playerTwoChoice = Figure.NONE; // Выбор игрока 2
    private bool _isPlayerTwoSelected = false; // Флаг выбора игрока 2
    private bool _isRoundFinished = false; // Флаг завершения раунда
    private bool _isVersusAi = true; // Режим игры: true - против ИИ, false - против игрока
    private TextMeshProUGUI _AIToggleButtonText; // Текст кнопки переключения режима

    private void Awake()
    {
        // Инициализация UI при запуске
        SetScoreText(_playerOneScoreText, _playerOneCurrentScore, _playerTwoCurrentScore);
        SetScoreText(_playerTwoScoreText, _playerTwoCurrentScore, _playerOneCurrentScore);
        _AIToggleButtonText = _AIToggleButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        // Начальная настройка игры
        SetAiButtonText(_isVersusAi);
        StartGame();
    }

    public void StartGame()
    {
        // Сброс состояния для нового раунда
        _endRoundMessageText.SetText("");

        _playerOneChoice = Figure.NONE;
        _playerTwoChoice = Figure.NONE;
        _isPlayerOneSelected = false;
        _isPlayerTwoSelected = false;

        _isRoundFinished = false;

        // Анимация UI элементов
        _playerOneChoiceAnimator.Play("PlayerOneChoiceMoveForward");
        _playerTwoChoiceAnimator.Play("PlayerTwoChoiceMoveForward");

        _playerOneSelectedImageAnimator.Play("PlayerOneSelectedImageMoveBackward");
        _playerTwoSelectedImageAnimator.Play("PlayerTwoSelectedImageMoveBackward");
    }

    public void Choice(Figure choice, bool isPlayerOne)
    {
        // Обработка выбора игрока
        if (_isRoundFinished)
        {
            return; // Если раунд завершен, игнорируем выбор
        }

        if (isPlayerOne)
        {
            // Обработка выбора первого игрока
            _playerOneChoice = choice;
            _isPlayerOneSelected = true;
            SetSelectedImage(choice, _playerOneSelectedImage);

            // Если режим против ИИ, ИИ делает случайный выбор
            if (_isVersusAi)
            {
                _playerTwoChoice = (Figure)Random.Range(0, 3);
                _isPlayerTwoSelected = true;
                SetSelectedImage(_playerTwoChoice, _playerTwoSelectedImage);
            }
        }
        else
        {
            // Обработка выбора второго игрока (только в режиме игрок против игрока)
            // ВАЖНО: Проверяем, что второй игрок может выбирать ТОЛЬКО в режиме игрок против игрока
            if (!_isVersusAi)
            {
                _playerTwoChoice = choice;
                _isPlayerTwoSelected = true;
                SetSelectedImage(choice, _playerTwoSelectedImage);
            }
        }

        // Проверяем, оба ли игрока сделали выбор
        // В режиме против ИИ: первый игрок + ИИ
        // В режиме игрок против игрока: оба игрока должны сделать выбор отдельно
        if (_isVersusAi)
        {
            // В режиме против ИИ: проверяем только выбор первого игрока
            // ИИ уже сделал выбор автоматически при выборе первого игрока
            if (_isPlayerOneSelected && _isPlayerTwoSelected)
            {
                _isRoundFinished = true;
                DetermineWinner();
            }
        }
        else
        {
            // В режиме игрок против игрока: проверяем выбор обоих игроков
            if (_isPlayerOneSelected && _isPlayerTwoSelected)
            {
                _isRoundFinished = true;
                DetermineWinner();
            }
        }
    }

    private void DetermineWinner()
    {
        // Определение победителя раунда
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

        // Активация панели окончания раунда
        _endRoundPanel.SetActive(true);

        // Анимация UI элементов
        _playerOneChoiceAnimator.Play("PlayerOneChoiceMoveBackward");
        _playerTwoChoiceAnimator.Play("PlayerTwoChoiceMoveBackward");

        _playerOneSelectedImageAnimator.Play("PlayerOneSelectedImageMoveForward");
        _playerTwoSelectedImageAnimator.Play("PlayerTwoSelectedImageMoveForward");

        // Обновление счета
        SetScoreText(_playerOneScoreText, _playerOneCurrentScore, _playerTwoCurrentScore);
        SetScoreText(_playerTwoScoreText, _playerTwoCurrentScore, _playerOneCurrentScore);
    }

    private void SetSelectedImage(Figure figure, Image image)
    {
        // Установка спрайта выбранной фигуры
        switch (figure)
        {
            case Figure.ROCK: image.sprite = _rockImage; break;
            case Figure.SCISSORS: image.sprite = _scissorsImage; break;
            case Figure.PAPER: image.sprite = _paperImage; break;
        }
    }

    private void SetScoreText(TextMeshProUGUI text, int player1Score, int player2Score)
    {
        // Установка текста счета
        text.SetText($"{player1Score} / {player2Score}");
    }

    private void SetAiButtonText(bool state)
    {
        // Установка текста кнопки переключения режима
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
        // Переключение режима игры
        _isVersusAi = !_isVersusAi;
        SetAiButtonText(_isVersusAi);
    }
}