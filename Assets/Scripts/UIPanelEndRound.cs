using UnityEngine;
using UnityEngine.UI;
public class UIPanelEndRound : MonoBehaviour
{
    private Button _buttonComponent;
    private Image _imageComponent;
    private GameManager _gameManagerScript;

    private void Awake()
    {
        _buttonComponent = GetComponent<Button>();
        _imageComponent = GetComponent<Image>();
    }
    private void Start()
    {
        _gameManagerScript = FindFirstObjectByType<GameManager>();
        if( _gameManagerScript == null )
        {
            throw new System.Exception("GameManager commponent was missing on the " + gameObject.name);
        }
    }
    private void OnEnable()
    {
        GameManager.SendState += OnState;
        _buttonComponent.onClick.AddListener(OnButtonClick);
    }
    private void OnDisable()
    {
        GameManager.SendState -= OnState;
        _buttonComponent.onClick.RemoveListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        _imageComponent.raycastTarget = false;
        _gameManagerScript.StartGame();
    }
    private void OnState(State state)
    {
        _imageComponent.raycastTarget = state == State.STARTROUND ? false : true;
    }
}
