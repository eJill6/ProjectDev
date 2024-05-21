using Serenity.Navigation;
using MyPages = Management.ProductManagement.Pages;

[assembly: NavigationMenu(5000, "ProductManagement", icon: "fa-list-alt")]
[assembly: NavigationLink(5001, "ProductManagement/Frontside Menu", typeof(MyPages.FrontsideMenuController), icon: "fa-gamepad")]