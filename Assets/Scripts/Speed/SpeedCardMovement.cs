using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpeedCardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform defaultParent;
    [SerializeField] Transform canvasTransform;

    private RectTransform rectTransform;

    private RectTransform parentRectTransform;


    void Start()
    {
        canvasTransform = GameObject.Find("Canvas").transform;

        rectTransform = GetComponent<RectTransform>();

    }

    public void SetParentRect()
    {
        parentRectTransform = rectTransform.parent as RectTransform;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPosition = GetLocalPosition(eventData.position);
        transform.localPosition = new Vector2(localPosition.x + parentRectTransform.localPosition.x, localPosition.y);


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(defaultParent, false);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }

}
