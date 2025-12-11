using System;
using UnityEngine;
using UnityEngine.UI;
public class ButtonChoice : MonoBehaviour
{
    public static event Action<Figure, bool> SendChoice; // Статическое событие для отправки выбора фигуры и флага игрока

    [SerializeField] private Figure _buttonChoice; // Выбор фигуры, связанный с кнопкой (настраивается в Inspector)
    [SerializeField] private bool _isPlayerOne; // Флаг: true если кнопка принадлежит первому игроку (настраивается в Inspector)

    private Button _button; // Ссылка на компонент Button

    void Awake()
    {
        _button = GetComponent<Button>(); // Получаем ссылку на компонент Button при инициализации объекта
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ChoiceSelected); // Подписываемся на событие клика кнопки при активации объекта
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ChoiceSelected); // Отписываемся от события при деактивации объекта (во избежание утечек памяти)
    }

    private void ChoiceSelected()
    {
        SendChoice?.Invoke(this._buttonChoice, this._isPlayerOne); // Вызываем событие, передавая выбранную фигуру и флаг игрока (проверка на null)
    }
}