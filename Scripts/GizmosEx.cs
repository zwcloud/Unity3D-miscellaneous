#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class GizmosEx
{
    public static bool dir = true;
    public static long factor;
    public static float Factor
    {
        get
        {
            var factor0 = System.DateTime.Now.Ticks % 1000;
            var factor1 = 1000 - factor0;
            if (factor == 0)
            {
                dir = !dir;
            }

            factor = dir ? factor0 : factor1;

            return factor / 1000f;
        }
    }

    public static float GetGizmoSize(Vector3 position)
    {
        Camera current = Camera.current;
        position = Gizmos.matrix.MultiplyPoint(position);

        float gizmoSize = 1f;
        if (current)
        {
            Transform transform = current.transform;
            Vector3 position2 = transform.position;
            float z = Vector3.Dot(position - position2, transform.TransformDirection(new Vector3(0f, 0f, 1f)));
            Vector3 a = current.WorldToScreenPoint(position2 + transform.TransformDirection(new Vector3(0f, 0f, z)));
            Vector3 b = current.WorldToScreenPoint(position2 + transform.TransformDirection(new Vector3(1f, 0f, z)));
            float magnitude = (a - b).magnitude;
            gizmoSize = 40f / Mathf.Max(magnitude, 0.0001f);
            gizmoSize = Mathf.Clamp(gizmoSize, 1, 3);
        }

        return gizmoSize;
    }

    public static void DrawArrow(Vector3 pos, Vector3 direction, bool flash, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        var orginalColor = Gizmos.color;
        if (flash)
        {
            var hsv = ColorUtility.ToHSV(orginalColor);
            Gizmos.color = ColorUtility.FromHSV(hsv[0], hsv[1], hsv[2] * Factor);
        }

        var gizmoSize = GetGizmoSize(pos);

        Gizmos.DrawRay(pos, direction * gizmoSize * 2);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction * gizmoSize * 2, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction * gizmoSize * 2, left * arrowHeadLength);

        Gizmos.color = orginalColor;

        SceneView.RepaintAll();
    }
}
#endif