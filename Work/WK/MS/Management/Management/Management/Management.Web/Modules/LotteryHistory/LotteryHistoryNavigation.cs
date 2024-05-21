using Serenity.Navigation;
using MyPages = Management.LotteryHistory.Pages;

[assembly: NavigationMenu(int.MaxValue, "LotteryHistory", icon: "fa-list")]
[assembly: NavigationLink(int.MaxValue, "LotteryHistory/Lottery Num", typeof(MyPages.LotteryNumController), icon: "fa-history")]