using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject ObjectToDrag;
    public GameObject ObjectDragToPos;

    public float DropDistance;

    Vector2 ObjectInitPos;

    public bool isLocked = false;

    void Start()
    {
        ObjectInitPos = ObjectToDrag.transform.position;
    }

    public void DragObject() {
        if (!isLocked) { 
            ObjectToDrag.transform.position = Input.mousePosition;
        }
    }

    public void DropObject() {

        if (ObjectDragToPos != null)
        {
            float Distance = Vector3.Distance(ObjectToDrag.transform.position, ObjectDragToPos.transform.position);
            if (Distance < DropDistance)
            {
                isLocked = true;
                ObjectToDrag.transform.position = ObjectDragToPos.transform.position;

                GameOneManager.instance.DraggablePlacedCorrectly();
                GameManager.instance.Score++;

            }
            else
            {
                ObjectToDrag.transform.position = ObjectInitPos;
            }
        }
        else
        {
            ObjectToDrag.transform.position = ObjectInitPos;
        }
    }

    public void ResetPosition() { 
        ObjectToDrag.transform.position = ObjectInitPos;
        isLocked = false;
    }
}