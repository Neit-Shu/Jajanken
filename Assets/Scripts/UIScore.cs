using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerOneScoreText;
    [SerializeField] private TextMeshProUGUI _playerTwoScoreText;
    [SerializeField] private TextMeshProUGUI _endRoundMessageText;

    private int _playerOneCurrentScore = 0;
    private int _playerTwoCurrentScore = 0;

    private void Awake()
    {
        SetScoreText(_playerOneScoreText, _playerOneCurrentScore, _playerTwoCurrentScore);
        SetScoreText(_playerTwoScoreText, _playerTwoCurrentScore, _playerOneCurrentScore);
    }
    private void SetScoreText(TextMeshProUGUI text, int player1Score, int player2Score)
    {
        text.SetText($"{player1Score} / {player2Score}");
    }

    private void OnEnable()
    {
        GameManager.SendState += OnState;
    }

    private void OnDisable()
    {
        GameManager.SendState -= OnState;
    }

    private void OnState(State state)
    {
        if (state == State.PLAYER1WIN)
        {
            _playerOneCurrentScore++;
            _endRoundMessageText.SetText("Игрок 1 победил!");
        }
        else if (state == State.PLAYER2WIN)
        {
            _playerTwoCurrentScore++;
            _endRoundMessageText.SetText("Игрок 2 победил!");
        }
        else if (state == State.DRAW)
        {
            _endRoundMessageText.SetText("Ничья!");
        }
        else
        {
            _endRoundMessageText.SetText("");
        }

        SetScoreText(_playerOneScoreText, _playerOneCurrentScore, _playerTwoCurrentScore);
        SetScoreText(_playerTwoScoreText, _playerTwoCurrentScore, _playerOneCurrentScore);
    }
}
