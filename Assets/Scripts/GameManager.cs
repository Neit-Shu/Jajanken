using UnityEngine;
using TMPro;

public enum Figure { ROCK, SCISSORS, PAPER, NONE}
public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator _playerOneChoiceAnimator;
    [SerializeField] private Animator _playerTwoChoiceAnimator;
    [SerializeField] private GameObject _endRoundPanel;
    [SerializeField] private TextMeshProUGUI _endRoundMessageText;
    private Figure _playerOneChoice = Figure.NONE;
    private bool _isPlayerOneSelected = false;
    private Figure _playerTwoChoice = Figure.NONE;
    private bool _isPlayerTwoSelected = false;
    private bool _isRoundFinished = false;

    private void Start()
    {
        StartGame();
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
    }
    public void Choice(Figure choice, bool isPlayerOne)
    {
        if(_isRoundFinished)
        {
            return;
        }
        if(isPlayerOne)
        {
            _playerOneChoice = choice;
            _isPlayerOneSelected = true;
        }
        else
        {
            _playerTwoChoice = choice;
            _isPlayerTwoSelected = true;
        }
        if(_isPlayerOneSelected && _isPlayerTwoSelected)
        {
            _isRoundFinished = true;
            DetermineWinner();
        }
    }    
    private void DetermineWinner()
    {
        if(_playerOneChoice == _playerTwoChoice)
        {
            _endRoundMessageText.SetText("Ничья!");
        }
        else if(_playerOneChoice == Figure.ROCK && _playerTwoChoice == Figure.SCISSORS
            || _playerOneChoice == Figure.SCISSORS && _playerTwoChoice == Figure.PAPER
            || _playerOneChoice == Figure.PAPER && _playerTwoChoice == Figure.ROCK)
        {
            _endRoundMessageText.SetText("Игрок 1 победил!");
        }
        else
        {
            _endRoundMessageText.SetText("Игрок 2 победил!");
        }

        _endRoundPanel.SetActive(true);

        _playerOneChoiceAnimator.Play("PlayerOneChoiceMoveBackward");
        _playerTwoChoiceAnimator.Play("PlayerTwoChoiceMoveBackward");
    }
}
