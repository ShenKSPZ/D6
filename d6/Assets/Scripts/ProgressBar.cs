using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public RectTransform Bar;
    public Image Fill;
    public bool IsShinny = false;

    /// <summary>
    /// SetFilling
    /// </summary>
    /// <param name="value">value need to be 0 to 1</param>
    public void SetFill(float value, float MaxValue)
    {
        value = value / MaxValue;
        value = Mathf.Clamp01(value);
        Fill.rectTransform.DOSizeDelta(new Vector2(Bar.rect.width * value, Fill.rectTransform.rect.height), 0.2f);
    }

    public void SetShinny()
    {
        IsShinny = false;
        StartCoroutine(Shinny());
    }

    IEnumerator Shinny()
    {
        while (true)
        {
            Fill.DOColor(Color.yellow, 0.2f);
            yield return new WaitForSeconds(0.2f);
            Fill.DOColor(DiceUI.newColor(154, 34, 92), 0.2f);
            yield return new WaitForSeconds(0.7f);
        }
    }
}
