using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public CanvasGroup movementHint;  // CanvasGroup for movement hint
    public CanvasGroup jumpHint;      // CanvasGroup for jump hint
    public Transform player;          // Reference to the player
    public Transform jumpTriggerPosition; // Position where the jump hint should appear
    public float jumpTriggerRadius = 2f; // Radius around the trigger position for activation

    private bool movementHintShown = false;
    private bool jumpHintShown = false;

    void Start()
    {
        // Ensure tutorial hints are invisible initially
        if (movementHint != null) movementHint.alpha = 0;
        if (jumpHint != null) jumpHint.alpha = 0;

        // Start the movement hint after 2 seconds
        Invoke(nameof(ShowMovementHint), 1f);
    }

    void Update()
    {
        // Check if the player is near the jump trigger position
        if (!jumpHintShown && Vector3.Distance(player.position, jumpTriggerPosition.position) <= jumpTriggerRadius)
        {
            ShowJumpHint();
        }
    }

    private void ShowMovementHint()
    {
        if (!movementHintShown && movementHint != null)
        {
            // Play the AudioSource attached to the Movement Hint
            AudioSource audio = movementHint.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            StartCoroutine(FadeIn(movementHint));
            movementHintShown = true;
            Invoke(nameof(HideMovementHint), 5f); // Automatically hide after 5 seconds
        }
    }


    private void HideMovementHint()
    {
        if (movementHint != null)
        {
            StartCoroutine(FadeOut(movementHint));
        }
    }

    private void ShowJumpHint()
    {
        if (!jumpHintShown && jumpHint != null)
        {
            // Play the AudioSource attached to the Jump Hint
            AudioSource audio = jumpHint.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            StartCoroutine(FadeIn(jumpHint));
            jumpHintShown = true;
            Invoke(nameof(HideJumpHint), 3f); // Automatically hide after 5 seconds
        }
    }

    private void HideJumpHint()
    {
        if (jumpHint != null)
        {
            StartCoroutine(FadeOut(jumpHint));
        }
    }

    private System.Collections.IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float duration = 1f; // Fade-in duration
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 1f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Ensure it's fully visible
    }

    private System.Collections.IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float duration = 1f; // Fade-out duration
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Ensure it's fully invisible
    }
}
