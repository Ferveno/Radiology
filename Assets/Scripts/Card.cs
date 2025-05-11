using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Import DOTween

public class Card : MonoBehaviour
{
    public Sprite cardArtWork; // Image of the tube or name.
    public Sprite nameArtWork; // Text-based image for names (not used currently).
    public int index;
    public bool isDistractor = false;
    private CardManager CM;
    public bool Open = false;

    void Awake()
    {
        CM = FindObjectOfType<CardManager>();
    }

    void OnMouseDown()
    {
        if (!Open)
        {
            CM.CardSelected(this);
        }
    }

    // Animate the card flip to reveal the card face.
    public void OpenCard()
    {
        // Animate rotation from 0 to 90 degrees.
        transform.DORotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            // Swap the sprite after half-flip.
            gameObject.GetComponent<SpriteRenderer>().sprite = cardArtWork;
            // Rotate back from 90 to 0 degrees.
            transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        });
        Open = true;
    }

    // Animate the card flip to hide the card (flip back to face down).
    public void NotChosen(Sprite FaceDown)
    {
        transform.DORotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = FaceDown;
            transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        });
        Open = false;
    }

    public void NoMatch()
    {
        Open = false;
    }

    // When a card is chosen (matched), you might want to play a fade-out before hiding it.
    public void Chosen()
    {
        // Animate scale to zero over 0.3 seconds.
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad);

        // Optionally, fade out the card by animating the SpriteRenderer's alpha.
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.DOFade(0f, 0.3f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            // Once fade out and scale animations are complete, disable the card.
            gameObject.SetActive(false);
        });

        // Increase the score.
        GameManager.instance.Score++;
        GameManager.instance.ScoreUpdater();

    }
}
