using BackSideWeb.Models.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Extensions;
using System.ComponentModel;

namespace BackSideWeb.Helpers
{
    public class MMSelectListItem
    {
        public static List<SelectListItem> GetPostTypeItems(bool isAll = false)
        {
            var items = new List<SelectListItem>();
            if (isAll)
            {
                items.Add(new SelectListItem("全选", "0", true));//改0
            }
            items.AddRange(new[]
            {
                new SelectListItem("广场", "1"),
                new SelectListItem("寻芳阁", "2"),
                new SelectListItem("官方", "3"),
                //new SelectListItem("体验", "4")
            });
            return items;
        }

        public static List<SelectListItem> GetOptionTypeItems(bool isAll = false)
        {
            var items = new List<SelectListItem>();
            if (isAll)
            {
                items.Add(new SelectListItem("全选", "0", true));
            }
            //else
            //{
            //  items.Add("请选择", null, true);
            //}
            items.AddRange(new[]
            {
                new SelectListItem("信息类型", "1"),
                new SelectListItem("申请调价", "2"),
                //new SelectListItem("标签", "3"),
                new SelectListItem("服务项目", "4"),
            });
            return items;
        }

        public static List<SelectListItem> GetIsActiveItems()
        {
            return new()
            {
                new SelectListItem("隐藏", "0"),
                new SelectListItem("显示", "1")
            };
        }

        /// <summary>
        /// 时间类型
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetTimeTypeItems()
        {
            return new List<SelectListItem>() {
              new SelectListItem("解锁时间","0",true),
              new SelectListItem("应入账时间","1"),
            };
        }

        /// <summary>
        /// 锁定状态
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetLockedStateItems()
        {
            return new List<SelectListItem>() {
              new SelectListItem("全选", null, true),
              new SelectListItem("未到期","1"),
              new SelectListItem("已到期","0"),
            };
        }

        /// <summary>
        /// 收益单状态
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetIncomeStatementStatusItems()
        {
            return new List<SelectListItem>() {
              new SelectListItem("全选", null, true),
              new SelectListItem("暂锁","2"),
              new SelectListItem("入账","10"),
              new SelectListItem("不入账","5"),
              new SelectListItem("异常","99"),
              new SelectListItem("审核入账","1"),
              new SelectListItem("审核不入账","11"),
            };
        }

        /// <summary>
        /// 解锁方式
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetUnlockMethodItems()
        {
            return new List<SelectListItem>() {
              new SelectListItem("全选", null, true),
              new SelectListItem("一般","0"),
              new SelectListItem("免费","1"),
              new SelectListItem("打折","2"),
            };
        }

        /// <summary>
        /// 钻石状态
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetDiamondStatusItems()
        {
            return new List<SelectListItem>() {
              new SelectListItem("暂锁", "0", true),
              new SelectListItem("入账","1"),
              new SelectListItem("审核入账","2"),
              new SelectListItem("审核不入账","3"),
            };
        }

        /// <summary>
        /// 预约支付方式
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetBookingPaymentTypeItems() =>
            new List<SelectListItem>() {
              new SelectListItem(CommonElement.All, null, true),
              new SelectListItem("预约","1"),
              new SelectListItem("全额","2")
            };

        /// <summary>
        /// 预约单支付类型下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetPaymentTypeItems() =>
            new List<SelectListItem>() {
              new SelectListItem(CommonElement.All, null, true),
              new SelectListItem("预约金","1"),
              new SelectListItem("全额","2"),
              new SelectListItem("退还预约金","3"),
              new SelectListItem("退还全额","4")
            };

        /// <summary>
        /// 预约单时间下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetTimeTypeItems_Booking() =>
            new List<SelectListItem>() {
              new SelectListItem("下单时间","1", true),
              //new SelectListItem("接单时间","2"),
              new SelectListItem("确认完成时间","3"),
              //new SelectListItem("取消订单时间","4")
            };

        /// <summary>
        /// 预约单订单状态下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetOrderStatusItems() =>
            new List<SelectListItem>() {
              new SelectListItem(CommonElement.All, null, true),
              new SelectListItem("服务中","1"),
              new SelectListItem("已完成","2"),
              new SelectListItem("退款中","3"),
              new SelectListItem("已退款","4")
            };

        /*现在的 服务中(1) 包含原有状态：待接单（0），服务中（1），拒绝退款（10）*/
        /*现在的 交易完成(2) 包含原有状态：待评价（2），评价审核中（3），交易完成（4），评价审核未通过（5），订单已取消（6），超时未接单（7），超时未接单处理中（11）*/
        /*现在的 申请退款中(3) 包含原有状态：申请退款中(8)*/
        /*现在的 退款成功(4) 包含原有状态：退款成功(9)*/

        /// <summary>
        /// 预约单退款申请理由下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetApplyReasonItems() =>
            new List<SelectListItem>() {
              new SelectListItem(CommonElement.All, null, true),
              new SelectListItem("存在欺骗","1"),
              new SelectListItem("货不对板","2")
            };

        /// <summary>
        /// 预约单商户预约状态下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetBookingStatusItems() =>
            new List<SelectListItem>() {
              new SelectListItem(CommonElement.All, null, true),
              new SelectListItem("待接单","0"),
              new SelectListItem("服务中","1"),
              new SelectListItem("服务完成","2"),
              new SelectListItem("订单已取消","6"),
              new SelectListItem("超时未接单","7"),
              new SelectListItem("申请退款中","8"),
              new SelectListItem("退款成功","9")
            };

        /// <summary>
        /// 转导方式
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetLinkTypeItems()
        {
            return new()
            {
                new SelectListItem("外部", "1"),
                new SelectListItem("觅觅內部", "2"),
                new SelectListItem("秘色", "3")
            };
        }

        /// <summary>
        /// 前台位置
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetLocationTypeItems()
        {
            return new()
            {
                new SelectListItem("请选择", "", true),
                new SelectListItem("首页主选单", "1"),
                new SelectListItem("首页轮播Banner", "2"),
                new SelectListItem("店铺轮播Banner", "3")
            };
        }

        /// <summary>
        /// 區域類型
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetAreaTypeItems()
        {
            return new()
            {
                new SelectListItem("请选择", "0"),
                new SelectListItem("排行榜", "1"),
                new SelectListItem("直播间", "2"),
                new SelectListItem("提现中心", "3"),
                new SelectListItem("兑换中心", "4"),
                new SelectListItem("活动列表", "5"),
                new SelectListItem("活动详情", "6"),
                new SelectListItem("大厅游戏", "7")
            };
        }

        /// <summary>
        /// 類型
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetGameTypeItems()
        {
            return new()
            {
                new SelectListItem("请选择", "0"),
                new SelectListItem("OB体育", "1"),
                new SelectListItem("斗地主", "2"),
                new SelectListItem("瓦力游戏 / 抢庄牛牛", "3"),
                new SelectListItem("瓦力游戏 / 斗地主", "4"),
                new SelectListItem("JDB捕鱼 / 财神捕鱼", "5"),
            };
        }

        /// <summary>
        /// 類型
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetInsideTypeItems()
        {
            return new()
            {
                new SelectListItem("请选择", "0"),
                new SelectListItem("觅色充值中心", "1"),
                new SelectListItem("秘觅会员中心", "2"),
                new SelectListItem("官方店铺", "3"),
                new SelectListItem("寻芳阁", "4"),
                new SelectListItem("广场交友", "5"),
                new SelectListItem("入驻店铺", "6"),
                new SelectListItem("加入寻芳阁", "7"),
                new SelectListItem("防骗指南", "8"),
                new SelectListItem("私信", "9")
            };
        }

        /// <summary>
        /// enum下拉選單
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumItems<TEnum>(SelectEnum selectEnum = 0)
        {
            var items = new List<SelectListItem>();

            if (selectEnum == SelectEnum.All)
            {
                items.Add(new SelectListItem("全部", "0") { Selected = true });
            }
            else if (selectEnum == SelectEnum.Choose)
            {
                items.Add(new SelectListItem("请选择", "0") { Selected = true });
            }
            var enumDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, TEnum>();
            items.AddRange(enumDic.Select(x => new SelectListItem { Text = x.Key, Value = Convert.ToInt32(x.Value).ToString() }));

            return items;
        }

        public static List<SelectListItem> GetEnumItems<TEnum>()
        {
            var items = new List<SelectListItem>();
            var enumDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, TEnum>();
            items.AddRange(enumDic.Select(x => new SelectListItem { Text = x.Key, Value = Convert.ToInt32(x.Value).ToString() }));
            return items;
        }

        public static List<SelectListItem> GetEnumItems<TEnum>(int selectedValue)
        {
            var items = new List<SelectListItem>();
            var enumDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, TEnum>();
            foreach (var kvp in enumDic)
            {
                var item = new SelectListItem
                {
                    Text = kvp.Key,
                    Value = Convert.ToInt32(kvp.Value).ToString(),
                    Selected = Convert.ToInt32(kvp.Value) == selectedValue
                };
                items.Add(item);
            }
            return items;
        }

        public static List<SelectListItem> GetEnumItemsDefaultNull<TEnum>(SelectEnum selectEnum, int? selectedValue = null)
        {
            var items = new List<SelectListItem>();

            if (selectEnum == SelectEnum.All)
            {
                var allItem = new SelectListItem("全部", null);
                allItem.Selected = selectedValue == null;
                items.Add(allItem);
            }
            else if (selectEnum == SelectEnum.Choose)
            {
                var chooseItem = new SelectListItem("请选择", null);
                chooseItem.Selected = selectedValue == null;
                items.Add(chooseItem);
            }

            var enumDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, TEnum>();
            foreach (var kvp in enumDic)
            {
                var item = new SelectListItem
                {
                    Text = kvp.Key,
                    Value = Convert.ToInt32(kvp.Value).ToString(),
                    Selected = selectedValue.HasValue && Convert.ToInt32(kvp.Value) == selectedValue.Value
                };
                items.Add(item);
            }
            return items;
        }

        public static List<SelectListItem> GetPostStatusItems()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){
                    Text="全部",
                    Value=null,
                    Selected=true,
                },
                new SelectListItem()
                {
                    Text="审核中",
                    Value="0",
                },
                new SelectListItem()
                {
                    Text="展示中",
                    Value="1",
                },
                new SelectListItem()
                {
                    Text="未通过",
                    Value="2",
                }
            };
        }

        public static List<SelectListItem> GetReviewTimeTypeItems()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="首次送审时间",
                    Value="0",
                    Selected=true
                },
                new SelectListItem()
                {
                    Text="再次送审时间",
                    Value="1"
                },
                new SelectListItem()
                {
                    Text="审核时间",
                    Value="2"
                }
            };
        }

        public static List<SelectListItem> GetReportTypeItems()
        {
            return new List<SelectListItem>()
            {
                  new SelectListItem(){
                    Text="全部",
                    Value=null,
                    Selected=true,
                },
                new SelectListItem()
                {
                    Text="骗子",
                    Value="0",
                },
                new SelectListItem()
                {
                    Text="广告骚扰",
                    Value="1",
                },
                new SelectListItem()
                {
                    Text="货不对版",
                    Value="2",
                },
                new SelectListItem()
                {
                    Text="无效联系方式",
                    Value="3",
                }
            };
        }

        public static List<SelectListItem> GetCommentStatusItems()
        {
            return new List<SelectListItem>()
            {
               new SelectListItem(){
                    Text="全部",
                    Value=null,
                    Selected=true,
                },
                new SelectListItem()
                {
                    Text="审核中",
                    Value="0",
                },
                new SelectListItem()
                {
                    Text="通过",
                    Value="1",
                },
                new SelectListItem()
                {
                    Text="未通过",
                    Value="2",
                }
            };
        }

        public static List<JxBackendSelectListItem> GetYesNoItems()
        {
            return new List<JxBackendSelectListItem>()
            {
                new JxBackendSelectListItem()
                {
                    Text="是",
                    Value="True",
                },
                new JxBackendSelectListItem()
                {
                    Text="否",
                    Value="False",
                }
            };
        }

        public static List<JxBackendSelectListItem> GetIsActiveJxBackendSelectListItemItems()
        {
            return new List<JxBackendSelectListItem>()
            {
                new JxBackendSelectListItem()
                {
                    Text="开启",
                    Value="True",
                },
                new JxBackendSelectListItem()
                {
                    Text="关闭",
                    Value="False",
                }
            };
        }

        /// <summary>
        /// 秘色转导页面类型
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetRedirectTypeItems()
        {
            return new()
            {
                new SelectListItem("请选择", "", true),
                new SelectListItem("官方", "1"),
                new SelectListItem("寻芳阁", "2"),
                new SelectListItem("广场", "3"),
                new SelectListItem("店铺", "4"),
                new SelectListItem("官方帖子", "5"),
                new SelectListItem("广场寻芳阁帖子", "6")
            };
        }
        public static List<SelectListItem> GetBotGroupItems()
        {
            return new()
            {
                new SelectListItem("全部", "", true),
                new SelectListItem("A", "0"),
                new SelectListItem("B", "1"),
                new SelectListItem("C", "2"),
            };
        }
    }
}