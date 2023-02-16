using UnityEngine;
using UnityEngine.UI;

/**
 * Based on https://answers.unity.com/questions/1653199/how-to-make-gridlayout-dramatically-resize-content.html
 */

public class InventoryUIGrid : GridLayoutGroup, ISerializationCallbackReceiver
{
    [SerializeField] protected float aspectRatio = 1;
    [SerializeField] protected InventoryUIGrid[] others = new InventoryUIGrid[] {};

    public Vector2 calculateCellSize() {
        float width = rectTransform.rect.width;
        float useableWidth = width - this.padding.horizontal - (m_ConstraintCount - 1) * this.spacing.x;
        float cellWidth = useableWidth / m_ConstraintCount;
        float cellHeight = cellWidth * this.aspectRatio;
        int minRows = minRows = Mathf.CeilToInt(rectChildren.Count / (float)m_ConstraintCount - 0.001f);
        float height = rectTransform.rect.height;
        float useableHeight = height - this.padding.vertical - (minRows - 1) * this.spacing.y;
        cellHeight = Mathf.Min(cellHeight, useableHeight / minRows);
        return new Vector2(cellWidth, cellHeight);
    }

    public override void SetLayoutVertical() {
        this.cellSize = calculateCellSize();
        foreach (var other in others) {
            this.cellSize = Vector2.Min(this.cellSize, other.calculateCellSize());
        }

        base.SetLayoutVertical();
    }

    public void OnAfterDeserialize() {
    }

    public void OnBeforeSerialize() {
        this.m_CellSize = Vector2.zero;
    }
}