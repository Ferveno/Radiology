using UnityEngine;
using UnityEngine.UI;

public class GlassesPowerupButton : MonoBehaviour
{
    public Button glassesButton;
    public int glassesCost = 50;

    void Start()
    {
        UpdateButtonState();
    }

    void Update()
    {
        UpdateButtonState();
    }

    /// <summary>
    /// Updates the button interactability based on the player's score.
    /// </summary>
    void UpdateButtonState()
    {
        if (GameManager.instance.Score >= glassesCost)
        {
            glassesButton.interactable = true;
        }
        else
        {
            glassesButton.interactable = false;
        }
    }

    /// <summary>
    /// Called when the powerup button is clicked.
    /// </summary>
    public void OnGlassesButtonClicked()
    {
        if (GameManager.instance.Score >= glassesCost)
        {
            GameManager.instance.Score -= glassesCost;  // Deduct the powerup cost
            GameSixGamePlay_PanelUI.instance.ActivateGlasses();
        }
    }
}
