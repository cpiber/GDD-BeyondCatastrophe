using UnityEngine;
using UnityEngine.UI;


public class InventoryUIHLayout : HorizontalLayoutGroup
{
    [SerializeField] protected float aspectRatio = 1;

    public override void SetLayoutVertical() {
        base.SetLayoutVertical();
        
        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform child = rectChildren[i];
            var r = child.sizeDelta;
            r.x = child.sizeDelta.y * aspectRatio;
            child.sizeDelta = r;
        }
    }
}