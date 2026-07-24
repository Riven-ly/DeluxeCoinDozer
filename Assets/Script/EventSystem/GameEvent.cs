
// 事件类型枚举
public enum GameEvent
{
    NULL,
    PlayAds,
    LevelUp,
    /// <summary>
    /// 创建机台道具，固定消耗player金币数量 1
    /// </summary>
    CreatMachineItem,
    /// <summary>
    /// 推进机台道具，参数是int（奖励金币数量）
    /// </summary>
    GetMachineItemReward,
    GetMachineItemReward_SpecialFragment,
    GetMachineItemReward_SpecialDiamond,
    //开始扭蛋
    SpinGachapon,
    SpinWheel,
    GetGold,
    GetDiamond,
    //签到通知红点更新
    SignIn,
    //通知保存任务数据
    SaveGameTask,
    //更新任务红点
    UpdateTaskRedDot,
    //使用场景道具
    UseSceneItem,
    GetSceneItem,
    Open_City_Wall_Btn,
    Hide__City_Wall_Btn,
    DailyWheel,
    Daily_DifficultMachine,
    DifficultMachineRecordDiamond,
}