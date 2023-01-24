using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private RectTransform rect;
    private DraggableUI draggableUI;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        draggableUI = GameObject.Find("Tmp_Card").GetComponent<DraggableUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.black;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerClick != null)
        {
            eventData.pointerClick.transform.SetParent(transform);
            eventData.pointerClick.GetComponent<RectTransform>().position = rect.position;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (eventData.pointerClick != null)
        //{
        //    eventData.pointerClick.transform.SetParent(transform);
        //    eventData.pointerClick.GetComponent<RectTransform>().position = rect.position;
        //}

        if (draggableUI.hold)
        {
            eventData.pointerClick.transform.SetParent(transform);
            eventData.pointerClick.GetComponent<RectTransform>().position = rect.position;
            draggableUI.hold = false;
        }
    }
}
