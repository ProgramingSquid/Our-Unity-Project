using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapToItem : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListItem;
    public VerticalLayoutGroup layout;
    [Space(30)]
    public float affectVelocity = 150;
    public float snapForce;
    float snapSpeed;
    bool isSnapped;
    public float itemAmount { get; private set; }
    public int currentItem { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isSnapped = false;   
    }

    // Update is called once per frame
    void Update()
    {
        itemAmount = contentPanel.localPosition.y / (sampleListItem.rect.height + layout.spacing);
        currentItem = Mathf.RoundToInt(itemAmount);

        if (itemAmount != currentItem) { isSnapped = false; }
            if (scrollRect.velocity.magnitude < affectVelocity && !isSnapped)
        {
            scrollRect.velocity = Vector2.zero;
            snapSpeed += snapForce * Time.deltaTime;
            contentPanel.localPosition = new Vector3(contentPanel.localPosition.x,
            Mathf.MoveTowards(contentPanel.localPosition.y, currentItem * (sampleListItem.rect.height + layout.spacing), snapSpeed),
            contentPanel.localPosition.z);

            if(itemAmount == currentItem)
            {
                isSnapped = true;
            }
            else { isSnapped = false; }
        }
        if(scrollRect.velocity.magnitude > affectVelocity)
        {
            isSnapped = false;
            snapSpeed = 0;
        }
    }
}
