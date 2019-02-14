using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ItemBubble : MonoBehaviour
{
    // Offsets are measured in pixels, pixel density
    // is assumed to be the same as of the bubble itself.
    public int marginLeft = 4;
    public int marginRight = 3;
    public int marginTop = 3;
    public int marginBottom = 4;
    public int spacingX = 2;
    public string sortingLayerName = "UI";
    public int sortingOrder = 1;

    public enum Style
    {
        Solid,
        Semitransparent,
    }

    public struct StyledIcon
    {
        public Style Style;
        public Sprite Sprite;
    }

    private SpriteRenderer _bubble_sr;

    // Start is called before the first frame update.
    private void Start()
    {
    }

    private void Awake()
    {
        _bubble_sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private float PixelsToUnits(float p)
    {
        return p / _bubble_sr.sprite.pixelsPerUnit;
    }

    public void RenderSprites(IEnumerable<Sprite> icons)
    {
        var styledIcons = icons.Select(
            sprite => new StyledIcon()
            {
                Sprite = sprite, Style = Style.Solid
            }
        );
        RenderSprites(styledIcons);
    }

    public void RenderSprites(IEnumerable<StyledIcon> icons)
    {
        ResetSprites();
        if (icons == null || icons.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        var newSize = BubbleSizeForIcons(icons);
        _bubble_sr.size = newSize;

        float xPos = - (newSize.x * _bubble_sr.sprite.pixelsPerUnit / 2.0f) + marginLeft;
        float yPos = (marginBottom - marginTop) / 2.0f;

        var i = 0;
        foreach (var icon in icons)
        {
            var gameObjectForIcon = CreateGameObjectForIcon(icon, "Icon" + i);
            gameObjectForIcon.transform.localPosition = new Vector3(
                PixelsToUnits(xPos + icon.Sprite.rect.width / 2),
                PixelsToUnits(yPos),
                -0.1f
            );

            xPos += (int) icon.Sprite.rect.width + spacingX;
            ++i;
        }
    }

    private GameObject CreateGameObjectForIcon(StyledIcon icon, String name)
    {
        var gameObjectForIcon = new GameObject(name);
        gameObjectForIcon.transform.parent = transform;
        gameObjectForIcon.transform.localScale = Vector3.one;
        gameObjectForIcon.layer = 5;

        var spriteRenderer = gameObjectForIcon.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = icon.Sprite;
        spriteRenderer.sortingLayerName = sortingLayerName;
        spriteRenderer.sortingOrder = sortingOrder;

        switch(icon.Style)
        {
            case Style.Semitransparent:
                spriteRenderer.color = new Color(1f, 1f, 1f, .5f);
            break;
        }

        return gameObjectForIcon;
    }

    private Vector2 BubbleSizeForIcons(List<StyledIcon> icons)
    {
        int width = marginLeft + marginRight;
        width += (icons.Count - 1) * spacingX;
        int maxSpriteHeight = 0;

        foreach (var icon in icons)
        {
            var spriteRect = icon.Sprite.rect;
            width += (int) spriteRect.width;
            if (spriteRect.height  > maxSpriteHeight)
            {
                maxSpriteHeight = (int) spriteRect.height;
            }
        }

        int height = maxSpriteHeight + marginTop + marginBottom;
        return new Vector2(PixelsToUnits(width), PixelsToUnits(height));

    }

    public void ResetSprites()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
         {
             Transform child = transform.GetChild(i);
             child.parent = null;
             GameObject.Destroy(child.gameObject);
         }
    }
}
