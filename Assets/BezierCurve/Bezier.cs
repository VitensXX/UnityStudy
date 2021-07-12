using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bezier : MonoBehaviour
{
    public Slider slider;
    public GameObject Ball;
    public LineRenderer lineRenderer; 

    Vector3 A = new Vector3(-10, 10, 0);
    Vector3 B = new Vector3(0, 0, 0);
    Vector3 C = new Vector3(10, 10, 0);
    Vector3 D = new Vector3(20, -10, 0);

    Dictionary<int, LineRenderer> _lineCache = new Dictionary<int, LineRenderer>();

    List<Vector3> points = new List<Vector3>();
    void Start()
    {
        points.Add(A);
        points.Add(B);
        points.Add(C);
        points.Add(D);
        DrawLine(points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        Ball.transform.position = BezierCurve(slider.value, points);
    }

    Vector3 LerpPoint(Vector3 A, Vector3 B, float lerp)
    {
        float x = Mathf.Lerp(A.x, B.x, lerp);
        float y = Mathf.Lerp(A.y, B.y, lerp);
        float z = Mathf.Lerp(A.z, B.z, lerp);

        return new Vector3(x, y, z);
    }

    Vector3 BezierCurve(float lerp, List<Vector3> points)
    {
        if(points.Count == 1)
        {
            return points[0];
        }

        List<Vector3> p = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            p.Add(LerpPoint(points[i], points[i + 1], lerp));
        }

        //画线操作
        if (p.Count >= 2)
        {
            DrawLine(p);
        }

        return BezierCurve(lerp, p);
    }

    void DrawLine(List<Vector3> points)
    {
        int count = points.Count;
        LineRenderer line;
        _lineCache.TryGetValue(count, out line);
        if (line == null)
        {
            line = new GameObject("line", typeof(LineRenderer)).GetComponent<LineRenderer>();
            line.startWidth = 0.1f;
            _lineCache.Add(count, line);
        }
        line.positionCount = count;
        for (int i = 0; i < count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }

    void Recovery()
    {

    }
}
