using System.Collections;
using UnityEngine;
using UnityEngine.UI;
class TextFadeOut : MonoBehaviour
{
    //Fade time in seconds
    public float fadeOutTime;

    private Color textOriginalColor;

    private Text text;
    private void Start()
    {
        text = GetComponent<Text>();
        textOriginalColor = text.color;
    }

    public void FixText()
    {
        text.color = textOriginalColor;
    }
    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }
    private IEnumerator FadeOutRoutine()
    {
        Text text = GetComponent<Text>();
        Color originalColor = text.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
    }
}
