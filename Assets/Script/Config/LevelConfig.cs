using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig
{
    public List<LevelConfigData> configs = new List<LevelConfigData>()
    {
        new LevelConfigData(0,100,0,0f),// index = 0 null
        new LevelConfigData(1,5,10,10f),
        new LevelConfigData(2,65,10,10f),
        new LevelConfigData(3,80,10,10f),
        new LevelConfigData(4,95,10,10f),
        new LevelConfigData(5,110,15,10f),
        new LevelConfigData(6,130,15,10f),
        new LevelConfigData(7,150,15,10f),
        new LevelConfigData(8,175,15,10f),
        new LevelConfigData(9,200,15,10f),
        new LevelConfigData(10,225,15,10f),
        new LevelConfigData(11,255,20,5f),
        new LevelConfigData(12,285,20,5f),
        new LevelConfigData(13,315,20,5f),
        new LevelConfigData(14,350,20,5f),
        new LevelConfigData(15,385,20,5f),
        new LevelConfigData(16,420,30,5f),
        new LevelConfigData(17,460,30,5f),
        new LevelConfigData(18,500,30,5f),
        new LevelConfigData(19,545,30,5f),
        new LevelConfigData(20,590,30,5f),
        new LevelConfigData(21,635,40,5f),
        new LevelConfigData(22,685,40,5f),
        new LevelConfigData(23,735,40,5f),
        new LevelConfigData(24,790,40,5f),
        new LevelConfigData(25,845,40,5f),
        new LevelConfigData(26,905,50,5f),
        new LevelConfigData(27,965,50,5f),
        new LevelConfigData(28,1030,50,5f),
        new LevelConfigData(29,1100,50,5f),
        new LevelConfigData(30,1150,50,5f),
        new LevelConfigData(31,1200,50,5f),
        new LevelConfigData(32,1250,50,5f),
        new LevelConfigData(33,1300,50,5f),
        new LevelConfigData(34,1350,50,5f),
        new LevelConfigData(35,1400,50,5f),
        new LevelConfigData(36,1450,50,5f),
        new LevelConfigData(37,1500,50,5f),
        new LevelConfigData(38,1550,50,5f),
        new LevelConfigData(39,1600,50,5f),
        new LevelConfigData(40,1650,50,5f),
        new LevelConfigData(41,1700,60,5f),
        new LevelConfigData(42,1750,60,5f),
        new LevelConfigData(43,1800,60,5f),
        new LevelConfigData(44,1850,60,5f),
        new LevelConfigData(45,1900,60,5f),
        new LevelConfigData(46,1950,60,5f),
        new LevelConfigData(47,2000,60,5f),
        new LevelConfigData(48,2050,60,5f),
        new LevelConfigData(49,2100,60,5f),
        new LevelConfigData(50,2150,60,5f),
        new LevelConfigData(51,2200,70,5f),
        new LevelConfigData(52,2250,70,5f),
        new LevelConfigData(53,2300,70,5f),
        new LevelConfigData(54,2350,70,5f),
        new LevelConfigData(55,2400,70,5f),
        new LevelConfigData(56,2450,70,5f),
        new LevelConfigData(57,2500,70,5f),
        new LevelConfigData(58,2550,70,5f),
        new LevelConfigData(59,2600,70,5f),
        new LevelConfigData(60,2650,70,5f),
        new LevelConfigData(61,2750,80,5f),
        new LevelConfigData(62,2850,80,5f),
        new LevelConfigData(63,2950,80,5f),
        new LevelConfigData(64,3050,80,5f),
        new LevelConfigData(65,3150,80,5f),
        new LevelConfigData(66,3250,80,5f),
        new LevelConfigData(67,3350,80,5f),
        new LevelConfigData(68,3450,80,5f),
        new LevelConfigData(69,3550,80,5f),
        new LevelConfigData(70,3650,80,5f),
        new LevelConfigData(71,3750,90,5f),
        new LevelConfigData(72,3850,90,5f),
        new LevelConfigData(73,3950,90,5f),
        new LevelConfigData(74,4050,90,5f),
        new LevelConfigData(75,4150,90,5f),
        new LevelConfigData(76,4250,90,5f),
        new LevelConfigData(77,4350,90,5f),
        new LevelConfigData(78,4450,90,5f),
        new LevelConfigData(79,4550,90,5f),
        new LevelConfigData(80,4650,90,5f),
        new LevelConfigData(81,4750,90,5f),
        new LevelConfigData(82,4850,90,5f),
        new LevelConfigData(83,4950,90,5f),
        new LevelConfigData(84,5050,90,5f),
        new LevelConfigData(85,5150,90,5f),
        new LevelConfigData(86,5250,90,5f),
        new LevelConfigData(87,5350,90,5f),
        new LevelConfigData(88,5450,90,5f),
        new LevelConfigData(89,5550,90,5f),
        new LevelConfigData(90,5650,90,5f),
        new LevelConfigData(91,5850,100,5f),
        new LevelConfigData(92,6050,100,5f),
        new LevelConfigData(93,6250,100,5f),
        new LevelConfigData(94,6450,100,5f),
        new LevelConfigData(95,6650,100,5f),
        new LevelConfigData(96,6850,100,5f),
        new LevelConfigData(97,7050,100,5f),
        new LevelConfigData(98,7250,100,5f),
        new LevelConfigData(99,7450,100,5f),
        new LevelConfigData(100,7650,100,5f),
        new LevelConfigData(101,7850,120,5f),
        new LevelConfigData(102,8050,120,5f),
        new LevelConfigData(103,8250,120,5f),
        new LevelConfigData(104,8450,120,5f),
        new LevelConfigData(105,8650,120,5f),
        new LevelConfigData(106,8850,120,5f),
        new LevelConfigData(107,9050,120,5f),
        new LevelConfigData(108,9250,120,5f),
        new LevelConfigData(109,9450,120,5f),
        new LevelConfigData(110,9650,120,5f),
        new LevelConfigData(111,9850,120,5f),
        new LevelConfigData(112,10050,120,5f),
        new LevelConfigData(113,10250,120,5f),
        new LevelConfigData(114,10450,120,5f),
        new LevelConfigData(115,10650,120,5f),
        new LevelConfigData(116,10850,120,5f),
        new LevelConfigData(117,11050,120,5f),
        new LevelConfigData(118,11250,120,5f),
        new LevelConfigData(119,11450,120,5f),
        new LevelConfigData(120,11650,120,5f),
        new LevelConfigData(121,11850,120,5f),
        new LevelConfigData(122,12050,120,5f),
        new LevelConfigData(123,12250,120,5f),
        new LevelConfigData(124,12450,120,5f),
        new LevelConfigData(125,12650,120,5f),
        new LevelConfigData(126,12850,120,5f),
        new LevelConfigData(127,13050,120,5f),
        new LevelConfigData(128,13250,120,5f),
        new LevelConfigData(129,13450,120,5f),
        new LevelConfigData(130,13650,120,5f),
        new LevelConfigData(131,13850,120,5f),
        new LevelConfigData(132,14050,120,5f),
        new LevelConfigData(133,14250,120,5f),
        new LevelConfigData(134,14450,120,5f),
        new LevelConfigData(135,14650,120,5f),
        new LevelConfigData(136,14850,120,5f),
        new LevelConfigData(137,15050,120,5f),
        new LevelConfigData(138,15250,120,5f),
        new LevelConfigData(139,15450,120,5f),
        new LevelConfigData(140,15650,120,5f),
        new LevelConfigData(141,15850,120,5f),
        new LevelConfigData(142,16050,120,5f),
        new LevelConfigData(143,16250,120,5f),
        new LevelConfigData(144,16450,120,5f),
        new LevelConfigData(145,16650,120,5f),
        new LevelConfigData(146,16850,120,5f),
        new LevelConfigData(147,17050,120,5f),
        new LevelConfigData(148,17250,120,5f),
        new LevelConfigData(149,17450,120,5f),
        new LevelConfigData(150,999999,120,5f)
    };


    // 根据等级 获取对应配置
    public LevelConfigData GetLevelData(int level)
    {
        int _index = level;
        if (_index < 1) _index = 1;
        if (_index > 150) _index = 150;
        return configs[_index];
    }
}
public class LevelConfigData
{
    public int level;
    public int levelExperience;
    public int rewardGold;
    public float rewardDiamond;

    public LevelConfigData(int _lv,int _lvEx, int _goldCnt, float _diamondCnt)
    {
        level = _lv;
        levelExperience = _lvEx;
        rewardGold = _goldCnt;
        rewardDiamond = _diamondCnt;
    }
}
