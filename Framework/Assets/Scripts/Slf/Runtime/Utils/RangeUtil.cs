using UnityEngine;
using UnityEngine.UI;
using Slf;

//==========================
// - Author:      slf         
// - Date:        2022/08/23 13:33:25
// - Description: 范围检测
//==========================
public class RangeUtil 
{
    /// <summary>
    /// 圆形检测
    /// 物体位置与目标位置的距离是否小于圆半径
    /// </summary>
    /// <param name="t1">物体</param>
    /// <param name="t2">目标</param>
    /// <param name="radius">半径</param>
    /// <returns></returns>
    public static bool Circle(Transform t1, Transform t2 , float radius) {
        float dist = Vector3.Distance(t1.position, t2.position);
        bool boo = dist <= radius;
        return boo;
    }
    /// <summary>
    /// 绘制攻击区域
    /// </summary>
    public static void DrawCircleArea(Transform t, float radius)
    {
        int segments = 100;
        float deltaAngle = 360f / segments;
        Vector3 forward = t.forward;

        Vector3[] vertices = new Vector3[segments];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, deltaAngle * i, 0f) * forward * radius + t.position;
            vertices[i] = pos;
        }
        int trianglesAmount = segments - 2;
        int[] triangles = new int[trianglesAmount * 3];
        for (int i = 0; i < trianglesAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }
        GameObject go = new GameObject("AttackArea");
        go.transform.position = new Vector3(0, 0.1f, 0);
        go.transform.SetParent(t);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        mr.material.shader = Shader.Find("Unlit/Color");
        mr.material.color = Color.red;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mf.mesh = mesh;
    }





    /// <summary>
    /// 扇形区域检测
    /// 物体位置与目标位置的距离是否小于扇形半径
    /// 物体与目标的夹角是否小于扇形角度的二分之一
    /// </summary>
    /// <param name="t1">物体</param>
    /// <param name="t2">目标</param>
    /// <param name="distance">距离</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    public static bool Sector(Transform t1, Transform t2, float distance, float angle)
    {
        float dist = Vector3.Distance(t1.position, t2.position);
        float ang = Vector3.Angle(t1.forward, t2.position-t1.position);
        bool boo = dist <= distance && ang<=angle/2;
        return boo;
    }
    /// <summary>
    /// 绘制扇形区域
    /// </summary>
    public static void DrawSectorArea(Transform t, float distance, float angle)
    {
        int segments = 100;
        float deltaAngle = angle / segments;
        Vector3 forward = t.forward;

        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = t.position;
        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angle / 2 + deltaAngle * (i - 1), 0f) * forward * distance + t.position;
            vertices[i] = pos;
        }
        int trianglesAmount = segments;
        int[] triangles = new int[segments * 3];
        for (int i = 0; i < trianglesAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }

        GameObject go = new GameObject("AttackArea");
        go.transform.position = new Vector3(0, 0.1f, 0);
        go.transform.SetParent(t);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        mr.material.shader = Shader.Find("Unlit/Color");
        mr.material.color = Color.red;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mf.mesh = mesh;
    }


    /// <summary>
    /// 矩形区域检测
    /// 物体与目标是否小于等于90度
    /// 物体位置到目标位置的向量在发起检测物体前方向向量上的投影是否小于矩形的长度
    /// 物体位置到目标位置的向量在发起检测物体右方向向量上的投影是否小于矩形的一半宽度
    /// </summary>
    /// <param name="t1">物体</param>
    /// <param name="t2">目标</param>
    /// <param name="distance">距离</param>
    /// <param name="width">宽度</param>
    /// <returns></returns>
    public static bool Rectangle(Transform t1, Transform t2, float distance, float width)
    {
        float dot = Vector3.Dot(t2.position - t1.position, t1.forward);
        float projectionDist_forward = Vector3.Project(t2.position - t1.position, t1.forward).magnitude;
        float projectionDist_right = Vector3.Project(t2.position - t1.position, t1.right.normalized).magnitude;
        bool boo = dot >= 0 && projectionDist_forward <= distance && (projectionDist_right <= width / 2);
        return boo;
    }
    /// <summary>
    /// 绘制矩形区域
    /// </summary>
    public static void DrawRectangleArea(Transform t, float distance, float width)
    {
        Vector3[] vertices = new Vector3[4];
        vertices[0] = t.position + t.right * width / 2;
        vertices[1] = t.position + t.right * width / 2 + t.forward * distance;
        vertices[2] = t.position - t.right * width / 2 + t.forward * distance;
        vertices[3] = t.position - t.right * width / 2;
        int trianglesAmount = 2;
        int[] triangles = new int[trianglesAmount * 3];
        for (int i = 0; i < trianglesAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 2;
            triangles[3 * i + 2] = i + 1;
        }

        GameObject go = new GameObject("AttackArea");
        go.transform.position = new Vector3(0, 0.1f, 0);
        go.transform.SetParent(t);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        mr.material.shader = Shader.Find("Unlit/Color");
        mr.material.color = Color.red;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mf.mesh = mesh;
    }

}
