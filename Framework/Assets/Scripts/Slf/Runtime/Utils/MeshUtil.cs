using UnityEngine;
using UnityEngine.UI;
using Slf;

//==========================
// - Author:      slf         
// - Date:        2022/08/26 15:10:09
// - Description: 网格工具
//==========================
public class MeshUtil
{
    /// <summary>
    /// 改变材质颜色 不会打断合批
    /// </summary>
    /// <param name="g"></param>
    public static void ChangeMaterialColor(GameObject g, Color color)
    {
        Renderer rd = GetRenderer(g);
        ChangeMaterialColor(rd, color);
    }

    /// <summary>
    /// 改变材质颜色 不会打断合批
    /// </summary>
    /// <param name="rd"></param>
    public static void ChangeMaterialColor(Renderer rd, Color color,int index = 0)
    {
        if (rd != null)
        {
            
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            if (rd.HasPropertyBlock())
            {
                rd.GetPropertyBlock(mpb,index);
                if (mpb.GetColor("_Color") == color)
                {
                    return;
                }
            }
            mpb.SetColor("_Color", color);
            rd.SetPropertyBlock(mpb, index);

            if (rd.materials.Length - 1 > index)
            {
                ChangeMaterialColor(rd, color, index+1);
            }
        }
    }


    /// <summary>
    /// 获取对象的渲染器 MeshRenderer || SkinnedMeshRenderer
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public static Renderer GetRenderer(GameObject g)
    {
        MeshRenderer mr = g.GetComponent<MeshRenderer>();
        if(mr != null)
        {
            return mr;
        }
        SkinnedMeshRenderer smr = g.GetComponent<SkinnedMeshRenderer>();
        if(smr != null)
        {
            return smr;
        }

        return default(Renderer);
    }


    /// <summary>
    /// 获取渲染器颜色
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public static Color GetMaterialColor(GameObject g)
    {
        Renderer rd = GetRenderer(g);
        if(rd != null)
        {
            if (rd.HasPropertyBlock())
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                rd.GetPropertyBlock(mpb);
                Color oldColor = mpb.GetColor("_Color");
                if (oldColor != null)
                {
                    return oldColor;
                }
                
            }
        }
        return default(Color);
    }
}
