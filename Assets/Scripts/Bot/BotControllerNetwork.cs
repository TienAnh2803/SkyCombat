using UnityEngine;
using Fusion;

public class BotControllerNetwork : NetworkBehaviour
{
    BotController botController;

    private void Awake()
    {
        botController = GetComponent<BotController>();
    }

    public override void FixedUpdateNetwork()
    {
        BotHpAndFindTarget();
        if (botController.target == null) return;
        if (botController.hp >= botController.maxHp / 2)
        {
            botController.ChaseTarget();
        }
        else
        {
            botController.FleeFromTarget();
        }
    }

    private void BotHpAndFindTarget()
    {
        if (Time.time - botController.lastTargetUpdateTime > botController.targetUpdateInterval)
        {
            botController.FindTarget();
            botController.lastTargetUpdateTime = Time.time;
        }

        // Cập nhật HP mỗi 3 giây
        if (Time.time - botController.lastHpUpdateTime > botController.hpUpdateInterval)
        {
            botController.hp = Random.Range(0, 10);
            botController.lastHpUpdateTime = Time.time;
        }
    }
}
