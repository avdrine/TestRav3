using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PhysicalItem : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ItemData _itemData;
    private Camera _mainCamera;
    bool _drag = false;
    [SerializeField] private Collider[] colliders;
    public PhysicalItemEvent OnDropToBackPack = new PhysicalItemEvent();
    public UnityEvent OnStartDragItem = new UnityEvent();
    public UnityEvent OnEndDragItem = new UnityEvent();

    Vector3 targetRotationOnDrag = new Vector3(0, 150, 30);

    public ItemData ItemData { get => _itemData; }
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

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        IsDrag = true;
        _rb.isKinematic = true;
        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.layer = 2;


        StartCoroutine(RotateToDrag());
    }

    readonly float _rotateToDragDuration = 0.2f;
    private IEnumerator RotateToDrag()
    {
        Vector3 startEA = transform.eulerAngles;
        Vector3 startPosition = transform.position;

        float t = 0;
        while (t < _rotateToDragDuration)
        {
            yield return new WaitForSeconds(0.001f);
            transform.eulerAngles = Vector3.Lerp(startEA, targetRotationOnDrag, t / _rotateToDragDuration);
            transform.position = Vector3.Lerp(startPosition, _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f)), t / _rotateToDragDuration);
            t += Time.deltaTime;
        }
        transform.eulerAngles = targetRotationOnDrag;
    }

    private void FixedUpdate()
    {
        if(IsDrag)
        {
            Vector3 currPoint = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f));
            
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
        if (IsRaycastHitBackPack) { OnDropToBackPack.Invoke(this); }
        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.layer = 0;
    }

    private bool IsRaycastHitBackPack
    {
        get
        {
            RaycastHit hit;
            //сам луч, начинается от позиции этого объекта и направлен в сторону цели
            Ray ray = new Ray(_mainCamera.transform.position, transform.position - _mainCamera.transform.position);
            //пускаем луч
            Physics.Raycast(ray, out hit);

            //если луч с чем-то пересёкся, то..
            if (hit.collider != null && hit.collider.gameObject.CompareTag("BackPack")) return true;
            return false;
        }
    }


    public void MoveToInventory(Transform pointToEquip)
    {
        if(_moveToInventory_c == null)
        {
            DisableToTake();
            _moveToInventory_c = MoveToInventory_c(pointToEquip);
            StartCoroutine(_moveToInventory_c); 
        }
    }

    readonly float _moveToInventorygDuration = 0.2f;
    private IEnumerator _moveToInventory_c;
    private IEnumerator MoveToInventory_c(Transform pointToEquip)
    {
        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.eulerAngles;

        float t = 0;
        while (t < _moveToInventorygDuration)
        {
            yield return new WaitForSeconds(0.001f);
            transform.eulerAngles = Vector3.Lerp(startRotation, pointToEquip.eulerAngles, t / _moveToInventorygDuration);
            transform.position = Vector3.Lerp(startPosition, pointToEquip.position, t / _moveToInventorygDuration);
            t += Time.deltaTime;
        }
        transform.eulerAngles = pointToEquip.eulerAngles;
        transform.position = pointToEquip.position;

        _moveToInventory_c = null;
    }

    private void OnDestroy()
    {
        OnDropToBackPack.RemoveAllListeners();
        OnStartDragItem.RemoveAllListeners();
        OnEndDragItem.RemoveAllListeners();
    }

    private void DisableToTake()
    {
        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.SetActive(false);
        _rb.isKinematic = true;
    }

    private void EnableToTake()
    {
        for (int i = 0; i < colliders.Length; i++) colliders[i].gameObject.SetActive(true);
        _rb.isKinematic = false;
    }
}
