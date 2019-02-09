using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThoughtController : MonoBehaviour
{
    // Offsets are measured in pixels, pixel density
    // is assumed to be the same as of the bubble itself.
    public int marginLeft = 5;
    public int marginRight = 5;
    public int marginTop = 5;
    public int marginBottom = 5;
    public int spacingX = 5;

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

    public void SetThought(List<Sprite> icons)
    {
        ResetThought();
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
            gameObject.transform.localScale = Vector3.one;
        }

        width += (icons.Count - 1) * spacingX;

        int height = maxSpriteHeight + marginTop + marginBottom;
        Debug.unityLogger.Log(width);
        Debug.unityLogger.Log(height);
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

    public void ResetThought()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}