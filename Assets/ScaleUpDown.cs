using UnityEngine;
using System.Collections;

public class ScaleUpDown : MonoBehaviour
{
    public Vector3 minScale = new Vector3(1f, 1f, 1f);
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);
    public float speed = 2f;

    private bool scalingUp = true;

    void Start()
    {
        StartCoroutine(ScaleCoroutine());
    }

    IEnumerator ScaleCoroutine()
    {
        while (true)
        {
            Vector3 targetScale = scalingUp ? maxScale : minScale;
            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0f;
            float duration = Vector3.Distance(initialScale, targetScale) / speed;

            while (elapsedTime < duration)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
            scalingUp = !scalingUp;
            yield return null;
        }
    }
}
