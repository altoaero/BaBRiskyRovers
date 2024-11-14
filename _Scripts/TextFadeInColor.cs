using UnityEngine;
using UnityEngine.UI; // or TMPro if using TextMeshPro
using TMPro;
using System.Collections;

public class TextFadeInColor : MonoBehaviour
{
    [System.Serializable]
    public class TextFadeInfo
    {
        public TMP_Text uiText; // For TextMeshPro
        // public Text uiText; // Uncomment for Unity's UI Text
        public float fadeDuration = 2f; // Duration of the fade
        public float delay = 0f; // Delay before starting the fade
    }

    public TextFadeInfo[] textsToFade;

    void Start()
    {
        foreach (var textInfo in textsToFade)
        {
            StartCoroutine(FadeIn(textInfo));
        }
    }

    private IEnumerator FadeIn(TextFadeInfo textInfo)
    {
        Color color = textInfo.uiText.color;
        color.a = 0;
        textInfo.uiText.color = color;

        yield return new WaitForSeconds(textInfo.delay); // Wait for the specified delay

        float elapsed = 0f;

        while (elapsed < textInfo.fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / textInfo.fadeDuration);
            textInfo.uiText.color = color;
            yield return null;
        }

        color.a = 1f; // Ensure it's fully visible
        textInfo.uiText.color = color;
    }
}