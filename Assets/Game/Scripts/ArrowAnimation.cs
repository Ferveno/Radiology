using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    public float amplitude = 10f; // Maximum movement in units.
    public float speed = 2f;      // Speed of movement.

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // PingPong creates a back-and-forth motion.
        float offset = Mathf.PingPong(Time.time * speed, amplitude) - (amplitude / 2f);
        transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
    }
}
