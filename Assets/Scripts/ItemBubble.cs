using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ItemBubble : UIBubble
{
    // This is the sprite renderer priority so items would be
    // rendered on top of the background
    public int sortingOrder = 1;
    public string sortingLayerName = "UI";

    // Spacing in pixels between items
    public int spacingX = 2;

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

    // Start is called before the first frame update.
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // Default way to render sprites is with the solid style.
    // See the next method comment to find out the constratints.
    public void RenderSprites(IEnumerable<Sprite> icons)
    {
        if (icons == null)
        {
            RenderSprites((IEnumerable<StyledIcon>) null);
        }
        var styledIcons = icons.Select(
            sprite => new StyledIcon()
            {
                Sprite = sprite, Style = Style.Solid
            }
        );
        RenderSprites(styledIcons);
    }

    // Renders the provided collection of icons into the rect.
    // They are assumed to have the same size (probably not enforced),
    // and same pixel-to-unit ratio (enforced).
    public void RenderSprites(IEnumerable<StyledIcon> icons)
    {
        Reset();
        if (icons == null || !icons.Any())
        {
            // We hide the object if there is nothing to display.
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        var iconContainer = new GameObject("IconContainer");
        var contentSize = ContentSizeForIcons(icons);

        float xPos = - (UnitsToPixels(contentSize.x) / 2.0f);
        float yPos = 0;

        var i = 0;
        foreach (var icon in icons)
        {
            var gameObjectForIcon = CreateGameObjectForIcon(
                iconContainer.transform, icon, "Icon" + i);

            gameObjectForIcon.transform.localPosition = new Vector3(
                PixelsToUnits(xPos + icon.Sprite.rect.width / 2),
                PixelsToUnits(yPos),
                -0.1f
            );

            xPos += (int) icon.Sprite.rect.width + spacingX;
            ++i;
        }

        SetContents(iconContainer, contentSize, true);
    }

    private GameObject CreateGameObjectForIcon(Transform parent, StyledIcon icon, String name)
    {
        var gameObjectForIcon = new GameObject(name);
        gameObjectForIcon.transform.parent = transform;
        gameObjectForIcon.transform.localScale = Vector3.one;
        gameObjectForIcon.layer = 5; // UI

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

    // Computes the size of rect occupied by icons in pixels.
    // Assumes that they have the same pixel size as the bubble itself.
    private Vector2 ContentSizeForIcons(IEnumerable<StyledIcon> icons)
    {
        // If there are N items, there are only N - 1 spacings.
        int width = -spacingX;
        int height = 0;

        foreach (var icon in icons)
        {
            var spriteRect = icon.Sprite.rect;
            width += (int) spriteRect.width + spacingX;

            // I am not quite sure why this max thingy is needed, to be honest.
            // Probably as not to hard-code any sprite size, but if we draw
            // a 16x16 sprite and a 32x32 together it'll look fuck ugly.
            if (spriteRect.height > height)
            {
                height = (int) spriteRect.height;
            }
        }

        return new Vector2(PixelsToUnits(width), PixelsToUnits(height));

    }
}
