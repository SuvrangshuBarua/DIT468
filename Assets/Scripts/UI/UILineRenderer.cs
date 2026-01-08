using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    [SerializeField] private float _lineWidth;
    [SerializeField] private bool _center;
    private bool _isVisible = true;
    private List<(Vector2, Vector2)> _lineSegments = new List<(Vector2, Vector2)>();

    public void DrawLine(Vector2 start, Vector2 end)
    {
        _lineSegments.Add((start, end));
        if (_isVisible)
            SetVerticesDirty();
    }

    public void ClearLines()
    {
        _lineSegments.Clear();
        SetVerticesDirty();
    }

    public void SetVisible(bool visible)
    {
        _isVisible = visible;
        canvasRenderer.SetAlpha(visible ? 1f : 0f);
        if (visible)
            SetVerticesDirty();
    }
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        if(_lineSegments.Count == 0) return;

        for (int i = 0; i < _lineSegments.Count; i++)
        {
            var segment = _lineSegments[i];
            CreateLineSegment(vh, segment.Item1, segment.Item2);

            int index = i * 5;

            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);
        }
    }
    

    private void CreateLineSegment(VertexHelper vh, Vector3 point1, Vector3 point2)
    {
        Vector3 offset = _center ? (rectTransform.sizeDelta / 2) : Vector2.zero;
        
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Quaternion point1Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point1, point2) + 90);
        vertex.position = point1Rotation * new Vector3(-_lineWidth / 2, 0);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);
        vertex.position = point1Rotation * new Vector3(_lineWidth / 2, 0);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);


        Quaternion point2Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point2, point1) - 90);
        vertex.position = point2Rotation * new Vector3(-_lineWidth / 2, 0);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);
        vertex.position = point2Rotation * new Vector3(_lineWidth / 2, 0);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);

        vertex.position = point2 - offset;
        vh.AddVert(vertex);
    }

    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI));
    }
    
}
