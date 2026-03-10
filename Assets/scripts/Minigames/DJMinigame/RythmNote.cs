using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum NoteDirection
{
    Left,
    Down,
    Up,
    Right,

}

public class RhythmNote : MonoBehaviour
{
    public DJMinigame minigame;

    public float speed = 300f;
    public bool wasHit = false;

    public Image image;

    public Sprite leftSprite;
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite rightSprite;

    public NoteDirection direction;


    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        UpdateSprite();
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (!wasHit && rect.position.y < minigame.hitLine.position.y - minigame.hitTolerance)
        {
            minigame.NoteMissed(this);
            Destroy(gameObject);
        }
    }

    void UpdateSprite()
    {
        switch (direction)
        {
            case NoteDirection.Left:
                image.sprite = leftSprite;
                break;

            case NoteDirection.Down:
                image.sprite = downSprite;
                break;

            case NoteDirection.Up:
                image.sprite = upSprite;
                break;

            case NoteDirection.Right:
                image.sprite = rightSprite;
                break;
        }
    }
}