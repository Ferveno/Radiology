using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = new Vector3(0f, 1f, 0f);
    public float rotationSpeed = 45f;

    void Start()
    {
        StartCoroutine(RotateCoroutine());
    }

    IEnumerator RotateCoroutine()
    {
        while (true)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
