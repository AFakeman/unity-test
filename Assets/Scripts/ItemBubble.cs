using System.Collections;
using System.Collections.Generic;
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

    public void RenderSprites(List<Sprite> icons)
    {
        ResetSprites();
        if (icons == null || icons.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        
        int width = marginLeft + marginRight;
        int maxSpriteHeight = 0;

        for (int i = 0; i < icons.Count; ++i)
        {
            var icon = icons[i];
            width += (int) icon.rect.width;
            if (icon.rect.height  > maxSpriteHeight)
            {
                maxSpriteHeight = (int) icon.rect.height;
            }

            var gameObject = new GameObject("Icon" + i.ToString());
            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = icon;
            gameObject.transform.parent = transform;
            var rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sortingLayerName = sortingLayerName;
            rend.sortingOrder = sortingOrder;

            gameObject.transform.localScale = Vector3.one;
            gameObject.layer = 5;
        }

        width += (icons.Count - 1) * spacingX;

        int height = maxSpriteHeight + marginTop + marginBottom;
        _bubble_sr.size = new Vector2(PixelsToUnits(width), PixelsToUnits(height));

        float xPos = - (width / 2.0f) + marginLeft;
        float yPos = marginBottom + maxSpriteHeight / 2.0f - height / 2.0f;

        foreach (Transform trans in transform)
        {
            var spriteWidth = trans.GetComponent<SpriteRenderer>().sprite.rect.width;
            trans.localPosition = new Vector3(
                PixelsToUnits(xPos + spriteWidth / 2), 
                PixelsToUnits(yPos),
                -0.1f
            );
            xPos += (int) spriteWidth + spacingX;
        }

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