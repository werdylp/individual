using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaneVisual : MonoBehaviour
{
    private Image image;

    public Sprite normalSprite;
    public Sprite pressedSprite;
    public Sprite missSprite;

    public float flashDuration = 0.1f;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Press()
    {
        StopAllCoroutines();
        StartCoroutine(Flash(pressedSprite));
    }

    public void Miss()
    {
        StopAllCoroutines();
        StartCoroutine(Flash(missSprite));
    }

    IEnumerator Flash(Sprite sprite)
    {
        image.sprite = sprite;
        yield return new WaitForSeconds(flashDuration);
        image.sprite = normalSprite;
    }
}
