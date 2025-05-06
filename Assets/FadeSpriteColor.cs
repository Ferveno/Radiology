using UnityEngine;
using System.Collections;

public class FadeSpriteColor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color startColor = Color.white;
    public Color endColor = new Color(1f, 1f, 1f, 0f);
    public float duration = 2f;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        while (true)
        {
            // Fade out
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = endColor;

            // Fade in
            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.Lerp(endColor, startColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = startColor;
        }
    }
}
