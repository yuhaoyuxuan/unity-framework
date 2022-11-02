using UnityEngine;
namespace Slf
{
    //==========================
    // - Author:      slf         
    // - Date:        2022/08/09 17:02:23
    // - Description: Transform扩展
    //==========================
    public static class TransformExtension
    {
        public static void SetLocalPosX(this Transform transform, float x)
        {
            transform.localPosition = new Vector3(x, transform.GetLocalPosY(), transform.GetLocalPosZ());
        }

        public static float GetLocalPosX(this Transform transform)
        {
            return transform.localPosition.x;
        }

        public static void SetLocalPosY(this Transform transform, float y)
        {
            transform.localPosition = new Vector3(transform.GetLocalPosX(), y, transform.GetLocalPosZ());
        }

        public static float GetLocalPosY(this Transform transform)
        {
            return transform.localPosition.y;
        }

        public static void SetLocalPosZ(this Transform transform, float z)
        {
            transform.localPosition = new Vector3(transform.GetLocalPosX(), transform.GetLocalPosY(), z);
        }

        public static float GetLocalPosZ(this Transform transform)
        {
            return transform.localPosition.z;
        }

        public static void SetPosX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.GetPosY(), transform.GetPosZ());
        }

        public static float GetPosX(this Transform transform)
        {
            return transform.position.x;
        }

        public static void SetPosY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.GetPosX(), y, transform.GetPosZ());
        }

        public static float GetPosY(this Transform transform)
        {
            return transform.position.y;
        }

        public static void SetPosZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.GetPosX(), transform.GetPosY(), z);
        }

        public static float GetPosZ(this Transform transform)
        {
            return transform.position.z;
        }
    }
}
