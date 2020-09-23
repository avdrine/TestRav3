using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
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

    public bool IsDragItem { get => _dragItem; }

    /// <summary>
    /// Событие при успешном "одевании" предмета на рюкзак
    /// </summary>
    public PhysicalItemEvent EquipEvent { get => _equipEvent;  }

    /// <summary>
    /// Событие при успешном снятии предмета с рюкзака 
    /// </summary>
    public PhysicalItemEvent UnequipEvent { get => _unequipEvent;  }


    private void Awake()
    {
        Instance = this;
    }

    public void OnItemDropToBackPack(PhysicalItem item)
    {
        _playerBackPack.TryEquipItem(item);
    }

    public void OnItemThrowFromBackPack()
    {

    }
    
    public void OnStartDragItem()
    {
        _dragItem = true;
    }

    public void OnEndDragItem()
    {
        _dragItem = false;
    }
}
