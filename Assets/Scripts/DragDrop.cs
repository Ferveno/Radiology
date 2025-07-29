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

    // ← new flag: has this draggable ever been dropped (wrongly) before?
    private bool hasMadeWrongAttempt = false;

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

                //GameOneManager.instance.DraggablePlacedCorrectly();
                //GameManager.instance.Score++;
                //GameManager.instance.ScoreUpdater();

                // only award score if they never made a wrong attempt on this one
                GameOneManager.instance.DraggablePlacedCorrectly();
                if (!hasMadeWrongAttempt)
                {
                    GameManager.instance.Score++;
                    GameManager.instance.ScoreUpdater();
                }

            }
            else
            {
                hasMadeWrongAttempt = true;

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

        hasMadeWrongAttempt = false;
    }
}