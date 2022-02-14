using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Carousel : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField]
    private float dragTreshold = 20;

    [SerializeField]
    private float carouselWidth = 300;
    
    [SerializeField]
    private float elementWidth = 100;

    [SerializeField]
    private float minElementScaleBackRow = 0.5f;

    [SerializeField]
    private float minElementScaleFrontRow = 0.64f;

    [SerializeField]
    private float maxElementScaleFrontRow = 1f;

    [SerializeField]
    private Transform frontRowHolder;

    [SerializeField] 
    private List<RectTransform> itemsFrontRow;

    [SerializeField]
    private Transform backRowHolder;

    [SerializeField] 
    private List<RectTransform> itemsBackRow;

    private Vector2 previousFrameDragPosition;

    public void Start()
    {
        for (int i = 0; i < itemsFrontRow.Count; i++) 
        {
            itemsFrontRow[i].sizeDelta = new Vector2(elementWidth, itemsFrontRow[i].sizeDelta.y);
        }
        ScaleElements();
        SortElements();
        AssignSiblingIndices();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousFrameDragPosition = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        MoveElements(worldPos.x - previousFrameDragPosition.x);
        ScaleElements();
        SortElements();
        AssignSiblingIndices();
        previousFrameDragPosition = worldPos;
    }

    private void MoveElements(float distance) 
    {
        float farthestElementPosition = (carouselWidth - elementWidth)/2;

        //Element can't switch rows from back to front and to back again in one frame, so maximum distance is limited.
        distance = Math.Min(farthestElementPosition/2, distance);

        //We are gonna add items from front at the end of the back list, and we don't want to move them again.
        int itemsBackRowLastElementId = itemsBackRow.Count - 1; 

        for (int i = itemsFrontRow.Count - 1; i >= 0; i--) 
        {
            itemsFrontRow[i].position += Vector3.right * distance;
            if (itemsFrontRow[i].localPosition.x < -farthestElementPosition) 
            {
                itemsFrontRow[i].localPosition -= Vector3.right * (itemsFrontRow[i].localPosition.x + farthestElementPosition);
                MoveElementToBackRow(i);
            }
            else if(itemsFrontRow[i].localPosition.x > farthestElementPosition)
            {
                itemsFrontRow[i].localPosition -= Vector3.right * (itemsFrontRow[i].localPosition.x - farthestElementPosition);
                MoveElementToBackRow(i);
            }
        }

        for (int i = itemsBackRowLastElementId; i >= 0; i--) 
        {
            itemsBackRow[i].position -= Vector3.right * distance;
            if (itemsBackRow[i].localPosition.x < -farthestElementPosition) 
            {
                itemsBackRow[i].localPosition -= Vector3.right * (itemsBackRow[i].localPosition.x + farthestElementPosition);
                MoveElementToFrontRow(i);
            }
            else if (itemsBackRow[i].localPosition.x > farthestElementPosition)
            {
                itemsBackRow[i].localPosition -= Vector3.right * (itemsBackRow[i].localPosition.x - farthestElementPosition);
                MoveElementToFrontRow(i);
            }
        }
    }

    private void MoveElementToBackRow(int indexOfElementInFrontRow) 
    {
        itemsBackRow.Add(itemsFrontRow[indexOfElementInFrontRow]);
        itemsFrontRow[indexOfElementInFrontRow].SetParent(backRowHolder);
        itemsFrontRow.RemoveAt(indexOfElementInFrontRow);
    }

    private void MoveElementToFrontRow(int indexOfElementInBackRow) 
    {
        itemsFrontRow.Add(itemsBackRow[indexOfElementInBackRow]);
        itemsBackRow[indexOfElementInBackRow].SetParent(frontRowHolder);
        itemsBackRow.RemoveAt(indexOfElementInBackRow);
    }

    private void ScaleElements() 
    {
        for (int i = 0; i < itemsFrontRow.Count; i++)
        {
            itemsFrontRow[i].localScale = Vector3.one * (maxElementScaleFrontRow - (maxElementScaleFrontRow - minElementScaleFrontRow)
                * Math.Abs(itemsFrontRow[i].localPosition.x) / (carouselWidth / 2));
        }
        for (int i = 0; i < itemsBackRow.Count; i++)
        {
            itemsBackRow[i].localScale = Vector3.one * (minElementScaleFrontRow - (minElementScaleFrontRow - minElementScaleBackRow)
                * (1 - Math.Abs(itemsBackRow[i].localPosition.x) / (carouselWidth / 2)));
        }
    }

    private void SortElements() 
    {
        //Small list, so we can use bubblesort
        for (int i = 0; i < itemsFrontRow.Count; i++) 
        {
            for (int j = i+1; j < itemsFrontRow.Count; j++) 
            {
                if (itemsFrontRow[i].localScale.x > itemsFrontRow[j].localScale.x) 
                {
                    RectTransform save = itemsFrontRow[j];
                    itemsFrontRow[j] = itemsFrontRow[i];
                    itemsFrontRow[i] = save;
                }
            }
        }
    }

    private void AssignSiblingIndices() 
    {
        for (int i = 0; i < itemsFrontRow.Count; i++) 
        {
            itemsFrontRow[i].SetSiblingIndex(i);
        }
    }
}
