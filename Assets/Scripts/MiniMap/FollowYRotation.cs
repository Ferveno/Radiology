using UnityEngine;


public class FollowYRotation : MonoBehaviour
{
    [Header("Minimap rotations")]
    public Transform playerReference;
    public float playerOffset = 10f;


    /**/


    private void Update()
    {
        if (playerReference != null)
        {
            transform.position = new Vector3(playerReference.position.x, playerReference.position.y + -10f);
            //transform.rotation = Quaternion.Euler(90f, playerReference.eulerAngles.y, 0f);
        }
    }
}