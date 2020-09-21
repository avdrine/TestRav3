using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhysicalItem : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    private Camera _mainCamera;
    bool drag = false;

    Vector3 targetRotationOnDrag = new Vector3(0, 150, 30);
    // Start is called before the first frame update
    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        drag = true;
        _rb.isKinematic = true;

        StartCoroutine(RotateToDrag());
    }

    float _rotateToDragDuration = 0.2f;
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
        if(drag)
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

        drag = false;
        //_rb.useGravity = true;
        _rb.isKinematic = false;
    }
}
