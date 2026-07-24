using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GashaponMachineCell : MonoBehaviour
{
    public Rigidbody2D rig2D;

    [Header("力的参数配置")]
    public float minForce = 3f;
    public float maxForce = 7f;
    [Tooltip("角度范围：-90°(左) ~ 90°(右)，默认无需修改")]
    public Vector2 angleRange = new Vector2(-90f, 90f);

    float isStopTimer;
    void FixedUpdate()
    {
        if (!GashaponMachine.isSpining)
            return;
        // 限制物体的最大速度
        if (rig2D.velocity.sqrMagnitude > 16) //4*4
        {
            // 保留速度方向，只限制大小
            rig2D.velocity = rig2D.velocity.normalized * 4;
            isStopTimer = 0f;
        }
        else if(rig2D.velocity.sqrMagnitude < 0.1f)//卡在角落
        {
            isStopTimer += Time.deltaTime;
            if(isStopTimer > 0.5f)
            {
                ApplyRandomAngleForce();
            }
        }
    }

    public void ApplyRandomAngleForce()
    {
        if (rig2D == null) return;

        // 1. 生成-90°~90°之间的随机角度
        float randomAngle = Random.Range(angleRange.x, angleRange.y);
        // 2. 将角度转换为弧度（Unity数学函数需用弧度计算）
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        // 3. 计算对应角度的方向向量（核心：限定在向上/向两侧范围）
        Vector2 forceDirection = new Vector2(
            Mathf.Sin(angleInRadians),  // X轴分量：-90°=-1 → 0°=0 → 90°=1
            Mathf.Cos(angleInRadians)   // Y轴分量：-90°=0 → 0°=1 → 90°=0（始终≥0，不会向下）
        );

        // 4. 生成随机大小的力
        float randomForceMagnitude = Random.Range(minForce, maxForce);
        // 5. 施加瞬时冲量
        rig2D.AddForce(forceDirection * randomForceMagnitude, ForceMode2D.Impulse);

        // 调试：打印角度和方向（可选删除）
        //Debug.Log($"施加力的角度：{randomAngle:F1}°，方向：({forceDirection.x:F2}, {forceDirection.y:F2})", this);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GashaponMachine.isSpining)
            return;
        // 只在需要的碰撞体上补力（比如地面标签为"Ground"）
        if (collision.gameObject.CompareTag("GashaponMachineBox"))
        {
            // 计算碰撞法线方向，施加一个向上的反弹力
            Vector2 bounceDir = collision.contacts[0].normal;
            Vector2 randomDir = bounceDir + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 0.2f;
            randomDir = randomDir.normalized;
            rig2D.AddForce(randomDir * Random.Range(minForce * 0.5f, maxForce * 0.5f), ForceMode2D.Impulse);
        }
    }
}
