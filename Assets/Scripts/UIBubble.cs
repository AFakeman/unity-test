using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBubble : MonoBehaviour
{
    // Offsets are measured in pixels.
    public int marginLeft = 4;
    public int marginRight = 3;
    public int marginTop = 3;
    public int marginBottom = 4;

    private SpriteRenderer _bubble_sr;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        _bubble_sr = GetComponent<SpriteRenderer>();
    }

    protected float PixelsToUnits(float p)
    {
        return p / _bubble_sr.sprite.pixelsPerUnit;
    }

    protected float UnitsToPixels(float p)
    {
        return p * _bubble_sr.sprite.pixelsPerUnit;
    }

    // Sets the root to the center of the canvas, and resizes for given size.
    // If the parameter unpack is set to true, root object is considered
    // just a container, and all the children will be transferred to
    // UIBubble, destroying the root object. Used to reduce clutter.
    public void SetContents(GameObject root, Vector2 contentSize, bool unpack=false)
    {
        root.transform.parent = transform;
        root.transform.localPosition = GetCenterForContent();
        _bubble_sr.size = BubbleSizeForContents(contentSize);
        if (unpack) {
            foreach (Transform obj in root.transform) {
                obj.parent = transform;
            }
            root.transform.parent = null;
            GameObject.Destroy(root);
        }
    }

    // Returns the center of the canvas as a local position in units.
    private Vector2 GetCenterForContent()
    {
        var size = _bubble_sr.size;
        float x = (marginRight - marginLeft) / 2.0f;
        float y = (marginBottom - marginTop) / 2.0f;
        return new Vector2(x, y);
    }

    // Bubble size (in units) for contents including margins.
    // Contents are assumed to be in units.
    private Vector2 BubbleSizeForContents(Vector2 contentSize)
    {
        float width = PixelsToUnits(marginLeft + marginRight) + contentSize.x;
        float height = PixelsToUnits(marginTop + marginBottom) + contentSize.y;

        return new Vector2(width, height);
    }

    public void Reset()
    {
        // for loop instead of foreach since we are destroying children,
        // and no guarantees on iteration are available in this case.
        for (int i = transform.childCount - 1; i >= 0; --i)
         {
             Transform child = transform.GetChild(i);
             child.parent = null;
             GameObject.Destroy(child.gameObject);
         }
    }
}
