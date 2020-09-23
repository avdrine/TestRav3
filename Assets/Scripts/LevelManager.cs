using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер игрового уровня
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Переменные

    /// <summary>
    /// Singleton
    /// </summary>
    public static LevelManager Instance;

    /// <summary>
    /// Контроллер рюкзака игрока
    /// </summary>
    [SerializeField] private BackPackController _playerBackPack;
    
    /// <summary>
    /// Событие при успешном "одевании" предмета на рюкзак
    /// </summary>
    [SerializeField] private PhysicalItemEvent _equipEvent = new PhysicalItemEvent();

    /// <summary>
    /// Событие при успешном снятии предмета с рюкзака 
    /// </summary>
    [SerializeField] private PhysicalItemEvent _unequipEvent = new PhysicalItemEvent();

    /// <summary>
    /// Флаг состояния перетаскивания предметов
    /// </summary>
    private bool _dragItem = false;

    #region Глобальные параметры уровня
    [Header("Глобальные параметры уровня")]

    /// <summary>
    /// Угол разворота предмета при перетаскивании для удобного обзора
    /// </summary>
    [Tooltip("Угол разворота предмета при перетаскивании для удобного обзора")]
    [SerializeField] private Vector3 _itemRotationOnDrag = new Vector3(0, 150, 30);

    /// <summary>
    /// Продолжительность анимации разворота предмета при начале перетаскивания
    /// </summary>
    [Tooltip("Продолжительность анимации разворота предмета при начале перетаскивания")]
    [SerializeField] private float _itemRotateOnDragDuration = 0.2f;

    /// <summary>
    /// Продолжительность анимации "одевания" предмета на рюкзак
    /// </summary>
    [Tooltip("Продолжительность анимации \"одевания\" предмета на рюкзак")]
    [SerializeField] private float _itemMoveToInventoryDuration = 0.2f;

    /// <summary>
    /// Сила выталкивания одетого предмета при снятии с рюкзака
    /// </summary>
    [Tooltip("Сила выталкивания одетого предмета при снятии с рюкзака")]
    [SerializeField] private float _unequipItemImpulsePower = 2f;

    #endregion

    #endregion

    #region Свойства

    /// <summary>
    /// Флаг состояния перетаскивания предметов
    /// </summary>
    public bool IsDragItem { get => _dragItem; }

    /// <summary>
    /// Событие при успешном "одевании" предмета на рюкзак
    /// </summary>
    public PhysicalItemEvent EquipEvent { get => _equipEvent;  }

    /// <summary>
    /// Событие при успешном снятии предмета с рюкзака 
    /// </summary>
    public PhysicalItemEvent UnequipEvent { get => _unequipEvent;  }


    /// <summary>
    /// Угол разворота предмета при перетаскивании для удобного обзора
    /// </summary>
    public static Vector3 ItemRotationOnDrag { get => Instance._itemRotationOnDrag; }

    /// <summary>
    /// Продолжительность анимации разворота предмета при начале перетаскивания
    /// </summary>
    public static float ItemRotateOnDragDuration { get => Instance._itemRotateOnDragDuration; }

    /// <summary>
    /// Продолжительность анимации "одевания" предмета на рюкзак
    /// </summary>
    public static float ItemMoveToInventoryDuration { get => Instance._itemMoveToInventoryDuration; }


    /// <summary>
    /// Сила выталкивания одетого предмета при снятии с рюкзака
    /// </summary>
    public static float UnequipItemImpulsePower { get => Instance._unequipItemImpulsePower; }

    #endregion

    #region Функции

    #region Редактирование состояний флагов

    #region Изменение состояния флага перетаскивания

    public void OnStartDragItem()
    {
        _dragItem = true;
    }

    public void OnEndDragItem()
    {
        _dragItem = false;
    }

    #endregion

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    #region Действия после событий перетаскивания

    /// <summary>
    /// Действия, выполняемые при окончании перетаскивания предмета на рюкзак
    /// </summary>
    /// <param name="item">Перетаскиваемый предмет</param>
    public void OnItemDropToBackPack(PhysicalItem item)
    {
        _playerBackPack.TryEquipItem(item); // Выполнить запрос на помещение предмета в инвентарь
    }

    /// <summary>
    /// Действия, выполняемые при вытаскивании предмета из рюкзака
    /// </summary>
    /// <param name="item"></param>
    public void OnItemThrowFromBackPack(PhysicalItem item)
    {

    }

    #endregion

    #endregion
}
