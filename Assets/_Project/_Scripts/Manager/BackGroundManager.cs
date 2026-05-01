using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public static BackGroundManager Instance { get; private set; }
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetUpBG(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
