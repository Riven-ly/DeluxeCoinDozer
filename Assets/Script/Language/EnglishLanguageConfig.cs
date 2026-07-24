using System.Collections;
using System.Collections.Generic;

public static class EnglishLanguageConfig
{
    public static Dictionary<string, string> currentTexts = new Dictionary<string, string>()
    {
        {"Loading", "Loading"},
        {"LuckyGashapon", "Lucky Spin"},
        {"Gold", "Gold"},
        {"Ads", "AD"},
        {"PLAY", "PLAY"},
        {"Claim", " Claim"},
        {"CLAIM", " CLAIM"},
        {"ClaimX2", " Claim X2"},
        {"Claimed", "Claimed"},
        {"ClaimAll", " Claim All"},
        {"Use", "Use"},
        {"SPIN", "SPIN"},
        {"FREESPIN", "FREESPIN"},
        {"OPEN", "OPEN"},
        {"NoThanks", "NO,THANKS"},
        {"Start", "Start"},
        {"Quit", "Quit"},
        {"GO", "GO"},
        {"Complete", "Complete"},
        {"COMPLETE", "COMPLETE"},
        {"Cancel", "Cancel"},
        {"Submit", "Submit"},
        {"ACCEPT", "ACCEPT"},
        {"AdsNotReady", "The video is not ready,please try again later."},
        //-btn----
        {"TaskBtn", "Task"},
        {"SigninBtn", "Sign in"},
        {"LuckyWheelBtn", "Lucky Wheel"},
        {"ShardBtn", "Shard"},
        {"DailyChallengeBtn", "Daily Challenge"},
        {"SharkBtn", "Shake"},
        {"HugeBtn", "Huge"},
        {"GurrdBtn", "Guard"},
        {"BlastBtn", "Blast"},
        //levelUpPanel
        {"Lv", "Lv"},
        {"LV", "LV"},
        //GetGoldPanel
        {"GetGoldPanel_title", "Your gold coins have run out."},
        {"GetGoldPanel_explain", "Get more gold coins!"},
        {"GetGoldPanel_explain2", "Limited free opportunities available every day."},
        {"GetGoldPanel_freeBtnText", "FREE"},
        //签到
        {"DAY", "day"},
        {"SignIn_explain", "Tap to collect your reward "},
        //任务
        {"GameTaskPanel_btn1", "Daily"},
        {"GameTaskPanel_btn2", "Level"},
        {"GameTaskPanel_btn3", "Gashapon"},
        {"TaskType_GetGold", "Get {0} coins from the coin pusher"},
        {"TaskType_PlayAds", "Watch {0} ads"},
        {"TaskType_SpinWheel", "Spin the wheel {0} times"},
        {"TaskType_SpinGashaponMachine", "Spin the Gachapon Machine {0} times"},
        {"TaskType_LevelUp", "Reach Level {0}"},
        {"TaskinProgress", "Task in Progress"},
        //场景道具获得界面信息
        {"SceneItem_Big_Gold_Title", "Huge"},
        {"SceneItem_Big_Gold_Explain", "Drops a Huge Coin"},
        {"SceneItem_City_Wall_Title", "Guard"},
        {"SceneItem_City_Wall_Explain", "Raises side guards for 30s"},
        {"SceneItem_Gold_Explode_Title", "Blast"},
        {"SceneItem_Gold_Explode_Explain", "Drops a lot of coins"},
        {"SceneItem_Machine_Vibration_Title", "Shake"},
        {"SceneItem_Machine_Vibration_Explain", "Shakes the screen once"},
        //扭蛋机
        {"GashaponItem_Null", "Did not win"},
        {"GashaponItem_Big_Gold", "Huge Coin"},
        {"GashaponItem_City_Wall", "Wall"},
        {"GashaponItem_Diamond", "Diamond"},
        {"GashaponItem_Gold_Rain", "Coin Rain"},
        {"GashaponItem_Gold_Tower", "Coin Tower"},
        {"GashaponItem_Machine_Vibration", "Shake"},
        {"GashaponItem_Big_Gold_Rain", "Huge Coin Rain"},
        {"GashaponItem_Special_Diamond", "{0}"},
        {"GashaponItem_Special_Fragment", "Shard"},
        {"GashaponItem_Letter_A", "A"},
        {"GashaponItem_Letter_E", "E"},
        {"GashaponItem_Letter_C", "C"},
        {"GashaponItem_Letter_L", "L"},
        //网络检测
        {"RETRY", "RETRY"},
        {"NetworkTitle", "Network check failed"},
        {"NetworkStr", "Network connection lost. Please check your internet and try again."},
        //转盘
        {"DailyWheel_explain", "Chance to instantly {0} {1}50!"},
        {"DropRewardsPanel_explain", "Catch the falling rewards!"},
        //碎片
        {"ItemFragment_yindao", "Fragments you’ve obtained can be exchanged for items here!"},
        {"EmailError", "Email Error！"},
        {"AddEmail", "Add your Email"},
        {"REVISE", "REVISE"},
        {"Incomplete", "Incomplete"},
        {"SpecialFragment_1_Title", "Iphone16"},
        {"SpecialFragment_2_Title", "Galaxy Z Flip7"},
        {"SpecialFragment_3_Title", "Sony 1000xm5"},
        {"SpecialFragment_4_Title", "Quest3"},
        {"SpecialFragment_5_Title", "Ps5"},
        {"SpecialFragment_6_Title", "Ns2"},
        //困难模式
        {"DifficultMachineUI_title", "SUPER HARD"},
        {"DifficultMachineEnterPanel_Btn", "GET {0}"},
        {"DifficultMachineEnterPanel_explain", "Complete all challenge levels to win {0}!"},
        {"DifficultMachine_todayOver", "Please come again for the challenge tomorrow!"},
        {"Continue", "Continue"},
        {"DifficultMachineNextLevelPanel_explain", "of players got the {0} reward!"},
        {"DifficultMachineGameLosePanel_explain2", "Come back tomorrow to continue!"},
        {"GIVEUP", "GIVE UP"},
        {"DifficultMachineEnterYindao", "Clear the Daily Challenge to earn tons of coins and {0}!"},
        //设置
        {"TermsofService", "Terms of Service"},
        {"PrivacyPolicy", "Privacy Policy"},
        {"DifficultMachineQuitPanel_explain1", "Only once a day!"},
        {"DifficultMachineQuitPanel_explain2", "30% players won {0} today!"},
        //TxElementMain
        {"ENTER", "ENTER"},
        {"selectType_explain", "Please enter your account information"},
        {"InitCell_title", "Your current {0} is:"},
        {"InitCell_explain", "You can {0} after leveling up ({1})."},
        //---
        {"QueueUpCell_title", "Current {0} Amount"},
        {"QueueUpCell_explain", "Current queue size {0}, estimated {1} time:"},  
        {"QueueUpCell_adsBtn", "Speed Up Queue"},
        //---
        {"TaskCell_title", "{0} under review"},
        {"TaskCell_explain", "Prove your account is active within the time limit. Earn credit points by upgrading levels and watching ads."},
        {"TaskCell_coolingTimeText", "Funds return countdown: {0}"},
        {"TaskCell_btn", "Submit for review"},
        //--
        {"TxElementJinDuPanel_str1", "{0} test in progress..."},
        {"TxElementJinDuPanel_str2", "{0} test completed "},
        {"TxElementJinDuPanel_str3", "Generating {0} order..."},
        //typePanel
        {"TxElementTypeSelectPanel_title", "Choose Your {0} Method"},
        {"TxElementTypeSelectPanel_explain", "Please enter your account"},
        {"TxElementTypeSelectPanel_input1", "Please enter your {0} account"},
        {"TxElementTypeSelectPanel_input2", "Verify your {0} account"},
        {"TxElementTypeSelectPanel_Error", "Accounts are inconsistent"},
        {"TxElementTypeSelectPanel_Error2", "Incorrect accounts input"},
        //HistoryCell
        {"HistoryCell_state1", "Reviewing"},
        {"HistoryCell_state3", "Failed"},
        //FinalStep
        {"FinalStep_title", "Final Step"},
        {"FinalStep_explain", "Complete the final step to get it immediately:"},
        {"FinalStep_explain2", "Get letters through reward drop,Duplicate letters will be converted into gold coins"},
        //引导
        {"TxElement_yindao1_title", "Play & Earn Points"},
        {"TxElement_yindao1_explain", "Play More ! Earn More!"},
        {"TxElement_yindao2_title", "Convert Automatically"},
        {"TxElement_yindao2_explain", "Coins automatically convert to {0} upon {1}."},
        {"TxElement_yindao3_title", "Redeem Rapidly"},
        {"TxElement_yindao3_explain", "Redeem your rewards to your {0} account."},
        //评分
        {"EvaluationGamePanel_title1", "Are you enjoying the game?"},
        {"EvaluationGamePanel_btn1", "Not Really"},
        {"EvaluationGamePanel_btn2", "Love it!"},
        {"EvaluationGamePanel_title2", "Your 5 stars are very important to us.please give us 5 stars if you like it."},

        {"Special_Diamond_mymymy", "TW9uZXk="},//特殊钻石名字money
        {"Special_Diamond__unit", "JA=="},//特殊钻石符号$
        {"CHT", "Y2FzaCBvdXQ="},//Cash out
        {"CH", "Q2FzaA=="},//Cash 
        {"WH", "V2l0aGRyYXdhbA=="},//Withdrawal 
        {"wH", "d2l0aGRyYXdhbA=="},
        {"blc", "YmFsYW5jZQ=="},//balance
        {"Pym", "UGF5bWVudA=="},//Payment
        {"Pyg", "UGF5aW5n"},//Paying
    };
}
