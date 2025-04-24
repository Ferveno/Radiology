using UnityEngine;
using UnityEngine.UI;

public class ImageData : MonoBehaviour
{
    // Set this in the Inspector for each card.
    public bool isAbnormal;

    // Feedback messages specific to this image.
    [TextArea]
    public string normalFeedback = "There is no abnormality in the positioning of the ETT/CVC in this CXR.";

    [TextArea]
    public string abnormalFeedback = "A mispositioned CVC/ ETT is present in this CXR.\nPlease identify the abnormal region.";

    // For abnormal images, the target position (in local coordinates) where the player should tap.
    // (Set this up in the Inspector on your zoom panel image preview.)
    public Vector2 tapTargetPosition;

    // Acceptable tolerance (in local units/pixels) for a correct tap.
    public float tapTolerance = 50f;

    [TextArea]
    public string tapSuccessFeedback; // Message for a correct first tap
    [TextArea]
    public string tapFailureFeedback; // Message to display after all attempts are exhausted

    [TextArea]
    public string abnormalImageDescription; // Abnormal Image Description to display after all attempts are exhauste

    public Sprite DescriptionImage;
}
