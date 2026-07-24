
using System.ComponentModel;

// 创建一个 partial 类 SROptions
public partial class SROptions
{
    // 定义一个可在面板中调整的数字选项

   // [Category("游戏设置"), DisplayName("金币倍率")]
   // public float CoinMultiplier { get; set; } = 1.0f;


    // 定义一个可点击的按钮方法
    [Category("基本功能"), DisplayName("增加100金币")]
    public void Add100Coins()
    {
        EventManager.Instance.TriggerEvent(GameEvent.GetGold, 100);
    }

    [Category("基本功能"), DisplayName("增加10.55特殊钻石")]
    public void Add100Diamond()
    {
        EventManager.Instance.TriggerEvent(GameEvent.GetDiamond, 10.55f);
    }

    [Category("基本功能"), DisplayName("增加100经验")]
    public void AddLvEx()
    {
        GameManager.Instance.playerInfo.AddExperience(100);
        UIManager.Instance.playInfoUI.levelUI.AddLvEx();
    }
    //------------
    [Category("其他事件"), DisplayName("临时增加当前时间10小时(递增)")]
    public void AddHours()
    {
        GameManager.Instance.addDebug_hours += 10;
    }

    [Category("其他事件"), DisplayName("增加10次广告次数")]
    public void AddPlayAds()
    {
        EventManager.Instance.TriggerEvent(GameEvent.PlayAds, 10);
    }
    //------------扭蛋机器-------------------------------------------------------------------------
    [Category("扭蛋机器"), DisplayName("正常随机")]
    public void GashaponMachineSpin()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin();
    }
    [Category("扭蛋机器"), DisplayName("空奖")]
    public void GashaponMachineSpin1()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.NULL);
    }
    [Category("扭蛋机器"), DisplayName("巨大金币")]
    public void GashaponMachineSpin2()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Big_Gold);
    }
    [Category("扭蛋机器"), DisplayName("护墙")]
    public void GashaponMachineSpin3()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.City_Wall);
    }
    [Category("扭蛋机器"), DisplayName("钻石")]
    public void GashaponMachineSpin4()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Diamond);
    }
    [Category("扭蛋机器"), DisplayName("金币雨")]
    public void GashaponMachineSpin5()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Gold_Rain);
    }
    [Category("扭蛋机器"), DisplayName("金币塔")]
    public void GashaponMachineSpin6()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Gold_Tower);
    }
    [Category("扭蛋机器"), DisplayName("震动")]
    public void GashaponMachineSpin7()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Machine_Vibration);
    }
    [Category("扭蛋机器"), DisplayName("巨大金币雨")]
    public void GashaponMachineSpin8()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Big_Gold_Rain);
    }
    [Category("扭蛋机器"), DisplayName("钻石2")]
    public void GashaponMachineSpin9()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Special_Diamond);
    }
    [Category("扭蛋机器"), DisplayName("碎片")]
    public void GashaponMachineSpin10()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Special_Fragment);
    }
    [Category("扭蛋机器"), DisplayName("字母E")]
    public void GashaponMachineSpin11()
    {
        GameManager.Instance.curMachine.gashaponMachine.StartSpin(GashaponItemType.Letter_E);
    }
}