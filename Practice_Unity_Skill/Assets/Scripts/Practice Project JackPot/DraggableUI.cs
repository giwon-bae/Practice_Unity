using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerClickHandler//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;
    private Transform previousParent;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private GridBehavior gridBehavior;
    public bool hold = false;


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gridBehavior = GameObject.Find("Grid Generator").GetComponent<GridBehavior>();
    }

    private void Update()
    {
        if (hold)
        {
            rect.position = Input.mousePosition;
        }
        else
        {
            if (transform.parent == canvas)
            {
                transform.SetParent(previousParent);
                rect.position = previousParent.GetComponent<RectTransform>().position;
            }

            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;

            gridBehavior.selectCard = false;
        }
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    previousParent = transform.parent;

    //    transform.SetParent(canvas);
    //    transform.SetAsLastSibling();

    //    canvasGroup.alpha = 0.6f;
    //    //canvasGroup.blocksRaycasts = false;

    //    gridBehavior.selectCard = true;
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    rect.position = eventData.position;
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    if(transform.parent == canvas)
    //    {
    //        transform.SetParent(previousParent);
    //        rect.position = previousParent.GetComponent<RectTransform>().position;
    //    }

    //    canvasGroup.alpha = 1.0f;
    //    canvasGroup.blocksRaycasts = true;

    //    gridBehavior.selectCard = false;
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!hold)
        {
            Debug.Log("Hold" + hold);
            previousParent = transform.parent;
            Debug.Log(previousParent);

            transform.SetParent(canvas);
            transform.SetAsLastSibling();

            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            gridBehavior.selectCard = true;
            hold = true;
        }
    }
}
