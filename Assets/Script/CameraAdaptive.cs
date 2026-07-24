using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 宽度适配
/// </summary>
public class CameraAdaptive : MonoBehaviour
{
    private int baseWidth = 1080;
    private int baseHeight = 1920;
    private float baseVerticalFOV = 70.5f;

    private Camera mainCam;
    private float targetHorizontalFOV; // 锁定的核心：固定水平视野角

    void Awake()
    {
        mainCam = GetComponent<Camera>();
        // 核心步骤1：计算出「基准分辨率下的水平视野角」→ 永久锁定这个值，不再变化
        float baseAspect = (float)baseWidth / baseHeight;
        targetHorizontalFOV = GetHorizontalFOV(baseVerticalFOV, baseAspect);

        FixWidthExact();
    }

    /// <summary>
    /// 核心方法：100%精准固定宽度，无任何偏差
    /// </summary>
    private void FixWidthExact()
    {
        // 当前屏幕实时宽高比 (竖屏 宽/高)
        float currentAspect = (float)Screen.width / Screen.height;
        // 核心公式：根据【固定的水平视野角】+【当前宽高比】反算精准的垂直FOV
        mainCam.fieldOfView = GetVerticalFOV(targetHorizontalFOV, currentAspect);
    }

    /// <summary>
    /// 数学公式：由【垂直FOV】和【宽高比】计算【水平FOV】（精准无误差）
    /// </summary>
    private float GetHorizontalFOV(float verticalFOV, float aspectRatio)
    {
        float verticalRadians = verticalFOV * Mathf.Deg2Rad;
        float horizontalRadians = 2 * Mathf.Atan(Mathf.Tan(verticalRadians / 2) * aspectRatio);
        return horizontalRadians * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 数学公式：由【固定的水平FOV】和【当前宽高比】反算【精准垂直FOV】
    /// </summary>
    private float GetVerticalFOV(float horizontalFOV, float aspectRatio)
    {
        float horizontalRadians = horizontalFOV * Mathf.Deg2Rad;
        float verticalRadians = 2 * Mathf.Atan(Mathf.Tan(horizontalRadians / 2) / aspectRatio);
        return verticalRadians * Mathf.Rad2Deg;
    }
}
