using Cinemachine;
using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public CamMovementDetect followScript; // Reference to your CamMovementDetect script
    public CinemachineBrain cinemachine;   // Reference to Cinemachine Brain
    public Transform secondaryObject;     // The UI game object to shake (e.g., fighters)

    private bool wasFollowingEnabled;

    public IEnumerator Shake(float duration, float initialMagnitude)
    {
        Debug.Log("I have started the shaking");

        // Store the original positions
        Vector3 originalPosition = transform.localPosition;
        Vector3 secondaryOriginalPosition = secondaryObject != null ? secondaryObject.localPosition : Vector3.zero;

        float elapsed = 0.0f;

        // Disable follow script (if provided)
        if (followScript != null)
        {
            wasFollowingEnabled = followScript.enabled;
            followScript.enabled = false;
            cinemachine.enabled = false;
        }

        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float magnitude = Mathf.Lerp(initialMagnitude, 0, progress);

            // Random offsets for shake
            float x = Random.Range(-1f, 1f) * magnitude;

            // Apply shake relative to the original position
            transform.localPosition = originalPosition + new Vector3(x, 0, 0);

            // Apply shake to secondary object (if exists)
            if (secondaryObject != null)
            {
                secondaryObject.localPosition = secondaryOriginalPosition + new Vector3(x, 0, 0);
            }

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Restore the original positions
        transform.localPosition = originalPosition;

        if (secondaryObject != null)
        {
            secondaryObject.localPosition = secondaryOriginalPosition;
        }

        // Re-enable follow script
        if (followScript != null && wasFollowingEnabled)
        {
            followScript.enabled = true;
            cinemachine.enabled = true;
        }

        Debug.Log("I have stopped shaking");
    }
}
