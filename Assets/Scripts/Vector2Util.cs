using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public static class Vector2Util {

    // static float[] m = new float[] { 0, -1f, 1f, 0 };

    /// <summary>
    /// 点P(x,y)绕轴点C(sx,sy)顺时针旋转90°
    /// </summary>
    /// <param name="x">Px</param>
    /// <param name="y">Py</param>
    /// <param name="sx">Cx</param>
    /// <param name="sy">Cy</param>
    /// <returns>Vector2 旋转后的坐标点</returns>
    private static Vector2Int RotateClockWise (int x, int y, float sx, float sy) {
        // 绕点旋转的逻辑：先移动到中心点，然后旋转，然后反向移动
        int xx = y + Mathf.RoundToInt(-sy + sx);
        int yy = -x + Mathf.RoundToInt(sx + sy);
        return new Vector2Int ( xx,yy);
    }
    /// <summary>
    /// 点P(x,y)绕轴点C(sx,sy)顺时针旋转90°
    /// </summary>
    /// <param name="p">旋转点</param>
    /// <param name="c">中心点</param>
    /// <returns>旋转后坐标</returns>
    public static Vector2Int RotateClockWise (this Vector2Int p, Vector2 c) {
        return RotateClockWise (p.x, p.y, c.x, c.y);
    }


}