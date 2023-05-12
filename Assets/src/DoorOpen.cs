using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite OpenSprite;
    public Sprite ClosedSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.sprite = OpenSprite;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.sprite = ClosedSprite;
    }
}
