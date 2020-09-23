using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Контроллер предмета инвентаря в физическом обличие
/// </summary>
public class PhysicalItem : MonoBehaviour
{
    #region Переменные

    /// <summary>
    /// Rigidbody объекта
    /// </summary>
    [SerializeField] private Rigidbody _rb;

    /// <summary>
    /// Основные характеристики предмета
    /// </summary>
    [SerializeField] private ItemData _itemData;

    /// <summary>
    /// Камера игрока
    /// </summary>
    private Camera _mainCamera;

    /// <summary>
    /// Флаг перетаскивания предмета мышкой
    /// </summary>
    bool _drag = false;

    /// <summary>
    /// Коллайдеры объекта
    /// </summary>
    [SerializeField] private Collider[] colliders;


    /// <summary>
    /// Событие, что перетаскивание закончилось на объекте рюкзака
    /// </summary>
    public PhysicalItemEvent OnDropToBackPack = new PhysicalItemEvent();

    /// <summary>
    /// Событие, что начато перетаскивание предмета мышкой
    /// </summary>
    public UnityEvent OnStartDragItem = new UnityEvent();

    /// <summary>
    /// Событие, что завершено перетаскивание предмета мышкой
    /// </summary>
    public UnityEvent OnEndDragItem = new UnityEvent();


    #endregion

    #region Свойства

    /// <summary>
    /// Основные характеристики предмета
    /// </summary>
    public ItemData ItemData { get => _itemData; }


    /// <summary>
    /// Флаг перетаскивания предмета мышкой
    /// </summary>
    public bool IsDrag 
    { 
        get => _drag;
        set 
        { 
            _drag = value;
            if (_drag) OnStartDragItem.Invoke();
            else OnEndDragItem.Invoke();
        }
    }

    #endregion

    #region Функции

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        OnDropToBackPack.AddListener(LevelManager.Instance.OnItemDropToBackPack);
        OnStartDragItem.AddListener(LevelManager.Instance.OnStartDragItem);
        OnEndDragItem.AddListener(LevelManager.Instance.OnEndDragItem);
    }

    void OnMouseDown()
    {
        IsDrag = true;

        //Отключение влияния физики на перетаскиваемый объект
        _rb.isKinematic = true;

        //Включение игнора RayCast, чтобы отлавливать пересечение с рюкзаком
        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.layer = 2;

        //Разворот предмета для более хорошего обзора модельки перетаскиваемого объекта
        StartCoroutine(RotateToDrag());
    }

    /// <summary>
    /// Разворот предмета для более хорошего обзора модельки перетаскиваемого объекта
    /// </summary>
    private IEnumerator RotateToDrag()
    {
        Vector3 startEA = transform.eulerAngles;
        Vector3 startPosition = transform.position;

        float t = 0;
        while (t < LevelManager.ItemRotateOnDragDuration)
        {
            yield return new WaitForSeconds(0.001f);
            transform.eulerAngles = Vector3.Lerp(startEA, LevelManager.ItemRotationOnDrag, t / LevelManager.ItemRotateOnDragDuration);
            transform.position = Vector3.Lerp(startPosition, _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f)), t / LevelManager.ItemRotateOnDragDuration);
            t += Time.deltaTime;
        }
        transform.eulerAngles = LevelManager.ItemRotationOnDrag;
    }

    private void FixedUpdate()
    {
        if(IsDrag)
        {
            Vector3 currPoint = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f));
            
            //Защита от прохождения модельки перетаскиваемого объекта под землю
            float c = 0.25f;
            if (currPoint.y < c)
            {
                currPoint.y = c;
            }

            _rb.MovePosition(currPoint);
        }

    }

    void OnMouseUp()
    {
        IsDrag = false;
        _rb.isKinematic = false;
        
        //Если при окончании перетаскивания предмет был наведен на рюкзак
        if (IsRaycastHitBackPack) { OnDropToBackPack.Invoke(this); }

        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.layer = 0;
    }

    /// <summary>
    /// Проверка наведения предмета на рюкзак
    /// </summary>
    private bool IsRaycastHitBackPack
    {
        get
        {
            RaycastHit hit;
            //Настройка направления луча (от камеры через объект)
            Ray ray = new Ray(_mainCamera.transform.position, transform.position - _mainCamera.transform.position);
            
            Physics.Raycast(ray, out hit);


            if (hit.collider != null && hit.collider.gameObject.CompareTag("BackPack")) return true;
            return false;
        }
    }

    /// <summary>
    /// Прикрепить предмет к позиции на рюкзаке
    /// </summary>
    /// <param name="pointToEquip">Зарезервированная позиция для данного предмета на рюкзаке</param>
    public void MoveToInventory(Transform pointToEquip)
    {
        if(_moveToInventory_c == null)
        {
            //Отключаем возможность перетаскивать предмет мышкой
            DisableToTake();

            //Запускаем анимацию плавного помещения предмета на рюкзак
            _moveToInventory_c = MoveToInventory_c(pointToEquip);
            StartCoroutine(_moveToInventory_c); 
        }
    }

    /// <summary>
    /// Переменная для защиты от одновременного запуска нескольких анимаций
    /// </summary>
    private IEnumerator _moveToInventory_c;

    /// <summary>
    /// Анимация прикрепления предмет к позиции на рюкзаке
    /// </summary>
    /// <param name="pointToEquip">Зарезервированная позиция для данного предмета на рюкзаке</param>
    private IEnumerator MoveToInventory_c(Transform pointToEquip)
    {
        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.eulerAngles;

        float t = 0;
        while (t < LevelManager.ItemMoveToInventoryDuration)
        {
            yield return new WaitForSeconds(0.001f);
            transform.eulerAngles = Vector3.Lerp(startRotation, pointToEquip.eulerAngles, t / LevelManager.ItemMoveToInventoryDuration);
            transform.position = Vector3.Lerp(startPosition, pointToEquip.position, t / LevelManager.ItemMoveToInventoryDuration);
            t += Time.deltaTime;
        }
        transform.eulerAngles = pointToEquip.eulerAngles;
        transform.position = pointToEquip.position;

        _moveToInventory_c = null;
    }

    /// <summary>
    /// Снятие предмета с рюкзака
    /// </summary>
    public void Unequip()
    {
        //Включение возможности перетаскивать предмет мышкой
        EnableToTake();

        //Добавить импульс для предмета, чтобы визуализировать снятие
        _rb.AddForce((Vector3.up - (transform.position - _mainCamera.transform.position).normalized) * LevelManager.UnequipItemImpulsePower, ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        OnDropToBackPack.RemoveAllListeners();
        OnStartDragItem.RemoveAllListeners();
        OnEndDragItem.RemoveAllListeners();
    }

    /// <summary>
    /// Отключить возможность перетаскивать предмет мышкой
    /// </summary>
    private void DisableToTake()
    {
        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.SetActive(false);
        _rb.isKinematic = true;
    }

    /// <summary>
    /// Включить возможность перетаскивать предмет мышкой
    /// </summary>
    private void EnableToTake()
    {
        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.SetActive(true);
        _rb.isKinematic = false;
    }

    #endregion
}
