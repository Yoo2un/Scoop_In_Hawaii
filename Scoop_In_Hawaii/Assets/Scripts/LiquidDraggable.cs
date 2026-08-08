using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LiquidDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public LiquidType liquidType;

    [SerializeField]
    private RectTransform draggingCanvasRect;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Canvas rootCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        Canvas inParentCanvas = GetComponentInParent<Canvas>();
        if (inParentCanvas != null)
        {
            rootCanvas = inParentCanvas.rootCanvas;
            draggingCanvasRect = rootCanvas.GetComponent<RectTransform>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        if (rootCanvas != null)
        {
            transform.SetParent(rootCanvas.transform);
        }

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingCanvasRect == null) return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            draggingCanvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent);
        ReturnToOriginalPosition();
    }

    public void ReturnToOriginalPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}