using ACBC.Buss;
using Com.ACBC.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Dao
{
    public class DashboardDao
    {
        /// <summary>
        /// 获取在线店铺code
        /// </summary>
        /// <returns></returns>
        public Shops OnlineGetShops()
        {
            Shops shops = new Shops();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_SHOPID_BY_ONLINE);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                Shop shop1 = new Shop
                {
                    shopId = "",
                    shopName = "全部店铺",
                };
                shops.list.Add(shop1);
                foreach (DataRow dr in dt.Rows)
                {
                    Shop shop = new Shop
                    {
                        shopId = dr["USERCODE"].ToString(),
                        shopName = dr["USERNAME"].ToString(),
                    };
                    shops.list.Add(shop);
                }
            }
            return shops;
        }
        /// <summary>
        /// 日客单价
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public string OnlineGetDailyAverage(string shopId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_DAILYAVERAGE_BY_SHOPID, shopId,"1");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count == 1)
            {
                return dt.Rows[0]["DAILY_AVERAGE"].ToString();
            }

            return null;
        }
        /// <summary>
        /// 昨日零售统计
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public PartSalesDay OnlineGetPartSalesDay(string shopId)
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-1).ToString("yyyy-MM-dd");
            string lastWeekYesterday = dtime.AddDays(-8).ToString("yyyy-MM-dd");
            string lastWeekToday = dtime.AddDays(-7).ToString("yyyy-MM-dd");
            PartSalesDay partSalesDay = null;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_SHOPID_DATE, shopId, yesterday, today, "1");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count == 1)
            {
                string actualAmount = dt.Rows[0]["ACTUAL_AMOUNT"].ToString();
                string orderNum = "0";
                string rate = "0";
                string supplyAmount = dt.Rows[0]["SUPPLY_AMOUNT"].ToString();
                string upOrDown = "0";
                //处理比率
                StringBuilder builder1 = new StringBuilder();
                builder1.AppendFormat(DashboardSqls.SELECT_RATE_BY_SHOPID_DATE, shopId, yesterday, today, lastWeekYesterday, lastWeekToday, "1");
                string sql1 = builder1.ToString();
                DataTable dt1 = DatabaseOperationWeb.ExecuteSelectDS(sql1, "T").Tables[0];
                if (dt1 != null && dt1.Rows.Count == 1)
                {
                    rate = dt1.Rows[0]["RATE"].ToString();
                    try
                    {
                        if (Convert.ToDouble(rate) < 0)
                        {
                            upOrDown = "1";
                            rate = (0 - Convert.ToDouble(rate)).ToString();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                //处理订单数
                StringBuilder builder2 = new StringBuilder();
                builder2.AppendFormat(DashboardSqls.SELECT_ORDERNUM_BY_SHOPID_DATE, shopId, yesterday, today, "1");
                string sql2 = builder2.ToString();
                DataTable dt2 = DatabaseOperationWeb.ExecuteSelectDS(sql2, "T").Tables[0];
                if (dt2 != null && dt2.Rows.Count == 1)
                {
                    orderNum = dt2.Rows[0]["ORDER_NUM"].ToString();
                }
                if (actualAmount == "")
                {
                    actualAmount = "0";
                }
                if (supplyAmount == "")
                {
                    supplyAmount = "0";
                }
                if (rate == "")
                {
                    rate = "0";
                }
                if (orderNum == "")
                {
                    orderNum = "0";
                }
                partSalesDay = new PartSalesDay
                {
                    actualAmount = actualAmount,           //日销售额
                    orderNum = orderNum,                   //订单数
                    rate = rate,                            //同比
                    supplyAmount = supplyAmount,           //应收账款
                    upOrDown = upOrDown,                  //上升还是下降
                };
            }

            return partSalesDay;
        }
        /// <summary>
        /// 月零售统计
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public MonthGroups OnlineGetMonthGroups(string shopId)
        {
            MonthGroups monthGroups = new MonthGroups();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_SHOPID_MONTH, shopId, "1");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                //查询订单数
                StringBuilder builder1 = new StringBuilder();
                builder1.AppendFormat(DashboardSqls.SELECT_ORDERNUM_BY_SHOPID_MONTH, shopId, "1");
                string sql1 = builder1.ToString();
                DataTable dt1 = DatabaseOperationWeb.ExecuteSelectDS(sql1, "T").Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PartSalesMonth partSalesMonth = new PartSalesMonth
                    {
                        actualAmount = dt.Rows[i]["ACTUAL_AMOUNT"].ToString(),
                        month = dt.Rows[i]["MONTH"].ToString(),
                        monthDisplay = "",
                        orderNum = "",
                        rate = "0",
                        supplyAmount = dt.Rows[i]["SUPPLY_AMOUNT"].ToString(),
                        upOrDown = "0",
                    };
                    //处理月份起末
                    DateTime dtime = Convert.ToDateTime(dt.Rows[i]["MONTH"].ToString() + "-01");
                    string beginDate = dtime.ToString("yyyy-MM-dd");
                    string endDate = dtime.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                    partSalesMonth.monthDisplay = beginDate + "~" + endDate;
                    //处理订单数
                    DataRow[] drs = dt1.Select("month='"+ dt.Rows[i]["MONTH"].ToString() + "'");
                    if (drs.Length>0)
                    {
                        partSalesMonth.orderNum = drs[0]["ORDER_NUM"].ToString();
                    }
                    //处理比率
                    
                    string lastYearMonth = dtime.AddYears(-1).ToString("yyyy-MM");
                    DataRow[] drs1 = dt.Select("month='" + lastYearMonth + "'");
                    if (drs1.Length > 0)
                    {
                        double d = 0;

                        try
                        {
                            double month1 = Convert.ToDouble(dt.Rows[i]["ACTUAL_AMOUNT"]);
                            double month2 = Convert.ToDouble(drs1[0]["ACTUAL_AMOUNT"]);
                            d = Math.Round((month1-month2 )/month2*100,2);
                        }
                        catch (Exception)
                        {
                        }
                        if (d<0)
                        {
                            d = 0 - d;
                            partSalesMonth.upOrDown = "1";
                        }
                        partSalesMonth.rate = d.ToString();
                    }
                    monthGroups.list.Add(partSalesMonth);
                    if (i >= 12)
                    {
                        break;
                    }
                }
            }
            return monthGroups;
        }

        public List<ProportionLegend> OnlineGetProportionLegend()
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-1).ToString("yyyy-MM-dd");
            List<ProportionLegend> proportionLegendList = new List<ProportionLegend>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_ONLINESHOP_YESTERDAY, yesterday, today, yesterday, today);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count >0)
            {
                double total = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        total += Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                total = Math.Round(total, 2);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double percent = 0;
                    try
                    {
                        percent = Math.Round(Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString()) / total * 100, 2);
                    }
                    catch (Exception)
                    {

                    }
                    ProportionLegend proportionLegend = new ProportionLegend
                    {
                        percentDisplay = percent.ToString()+"%",
                        shopName = dt.Rows[i]["USERNAME"].ToString(),
                    };
                    proportionLegendList.Add(proportionLegend);
                }


            }

            return proportionLegendList;
        }

        public List<ProportionValues> OnlineGetProportionValues()
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-1).ToString("yyyy-MM-dd");
            List<ProportionValues> proportionValuesList = new List<ProportionValues>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_ONLINESHOP_YESTERDAY, yesterday, today, yesterday, today);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                double total = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        total += Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }
                total = Math.Round(total, 2);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double percent = 0;
                    try
                    {
                        percent = Math.Round(Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString()) / total, 4);
                    }
                    catch (Exception)
                    {

                    }
                    ProportionValues proportionValues = new ProportionValues
                    {
                        constValue = "1",
                        shopName = dt.Rows[i]["USERNAME"].ToString(),
                        percent = percent,
                    };
                    proportionValuesList.Add(proportionValues);
                }
                
            }

            return proportionValuesList;
        }

        public List<DaySalesData> OnlineGetSalesTrendList(string shopId)
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-7).ToString("yyyy-MM-dd");
            List<DaySalesData> list = new List<DaySalesData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_SEVENAMOUNT_BY_SHOPID, shopId, yesterday,today, "1");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime tempDay = dtime.AddDays(list.Count - 7);
                    while (tempDay.ToString("yyyy-MM-dd") != dt.Rows[i]["DAY"].ToString())
                    {
                        DaySalesData daySalesData = new DaySalesData
                        {
                            day = (list.Count + 1).ToString(),
                            money = 0,
                            title = tempDay.ToString("yyyy-MM-dd"),
                        };
                        list.Add(daySalesData);
                        tempDay = tempDay.AddDays(1);
                    }
                    DaySalesData daySalesData1 = new DaySalesData
                    {
                        day = (list.Count+1).ToString(),
                        money = Convert.ToDouble( dt.Rows[i]["MONEY"].ToString()),
                        title = dt.Rows[i]["DAY"].ToString(),
                    };
                    list.Add(daySalesData1);
                }
            }
            return list;
        }

        public List<DayOrderData> OnlineGetOrderTrendList(string shopId)
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-7).ToString("yyyy-MM-dd");
            List<DayOrderData> list = new List<DayOrderData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_SEVENAMOUNT_BY_SHOPID, shopId, yesterday, today, "1");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime tempDay = dtime.AddDays(list.Count - 7);
                    while (tempDay.ToString("yyyy-MM-dd") != dt.Rows[i]["DAY"].ToString())
                    {
                        DayOrderData dayOrderData = new DayOrderData
                        {
                            day = (list.Count + 1).ToString(),
                            count = 0,
                            title = tempDay.ToString("yyyy-MM-dd"),
                        };
                        list.Add(dayOrderData);
                        tempDay = tempDay.AddDays(1);
                    }
                    DayOrderData dayOrderData1 = new DayOrderData
                    {
                        day = (list.Count + 1).ToString(),
                        count =Convert.ToDouble( dt.Rows[i]["ORDERNUM"].ToString()),
                        title = dt.Rows[i]["DAY"].ToString(),
                    };
                    list.Add(dayOrderData1);
                }
            }
            return list;
        }

        public List<SellerGoodsData> OnlineGetBestSellerGoodsList(string shopId)
        {
            List<SellerGoodsData> list = new List<SellerGoodsData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_BEST_GOODS_BY_SHOPID, shopId, "1");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = dt.Rows.Count-1; i >=0; i--)
                {
                    SellerGoodsData sellerGoodsData = new SellerGoodsData
                    {
                        brand = dt.Rows[i]["GOODSNAME"].ToString(),
                        count = Convert.ToDouble(dt.Rows[i]["COUNT"]),
                    };
                    list.Add(sellerGoodsData);
                }
            }
            return list;
        }

        public List<SellerGoodsData> OnlineGetLowSellerGoodsList(string shopId)
        {
            List<SellerGoodsData> list = new List<SellerGoodsData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_LOW_GOODS_BY_SHOPID, shopId, "1");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    SellerGoodsData sellerGoodsData = new SellerGoodsData
                    {
                        brand = dt.Rows[i]["GOODSNAME"].ToString(),
                        count = Convert.ToDouble(dt.Rows[i]["COUNT"]),
                    };
                    list.Add(sellerGoodsData);
                }
            }
            return list;
        }

        public List<AccountsReceivableTMonthData> OnlineGetAccountsReceivableTRateList(string shopId)
        {
            List<AccountsReceivableTMonthData> list = new List<AccountsReceivableTMonthData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_ACCOUNTS_RECEIVABLE_TRATE_BY_SHOPID, shopId);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AccountsReceivableTMonthData accountsReceivableTMonthData = new AccountsReceivableTMonthData
                    {
                        rate = Convert.ToDouble(dr["ACCOUNTRATIO"]),
                        title = dr["ACCOUNTMONTH"].ToString(),
                    };
                    list.Add(accountsReceivableTMonthData);
                }
            }
            return list;
        }



        public Shops OfflineGetShops()
        {

            Shops shops = new Shops();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_SHOPID_BY_OFFLINE);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                Shop shop1 = new Shop
                {
                    shopId = "",
                    shopName = "全部店铺",
                };
                shops.list.Add(shop1);
                foreach (DataRow dr in dt.Rows)
                {
                    Shop shop = new Shop
                    {
                        shopId = dr["USERCODE"].ToString(),
                        shopName = dr["USERNAME"].ToString(),
                    };
                    shops.list.Add(shop);
                }
            }
            return shops;
        }

        public string OfflineGetDailyAverage(string shopId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_DAILYAVERAGE_BY_SHOPID, shopId, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count == 1)
            {
                return dt.Rows[0]["DAILY_AVERAGE"].ToString();
            }

            return null;
        }

        public PartSalesDay OfflineGetPartSalesDay(string shopId)
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-1).ToString("yyyy-MM-dd");
            string lastWeekYesterday = dtime.AddDays(-8).ToString("yyyy-MM-dd");
            string lastWeekToday = dtime.AddDays(-7).ToString("yyyy-MM-dd");
            PartSalesDay partSalesDay = null;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_SHOPID_DATE, shopId, yesterday, today, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count == 1)
            {
                string actualAmount = dt.Rows[0]["ACTUAL_AMOUNT"].ToString();
                string orderNum = "0";
                string rate = "0";
                string supplyAmount = dt.Rows[0]["SUPPLY_AMOUNT"].ToString();
                string upOrDown = "0";
                //处理比率
                StringBuilder builder1 = new StringBuilder();
                builder1.AppendFormat(DashboardSqls.SELECT_RATE_BY_SHOPID_DATE, shopId, yesterday, today, lastWeekYesterday, lastWeekToday, "2");
                string sql1 = builder1.ToString();
                DataTable dt1 = DatabaseOperationWeb.ExecuteSelectDS(sql1, "T").Tables[0];
                if (dt1 != null && dt1.Rows.Count == 1)
                {
                    rate = dt1.Rows[0]["RATE"].ToString();
                    try
                    {
                        if (Convert.ToDouble(rate) < 0)
                        {
                            upOrDown = "1";
                            rate = (0 - Convert.ToDouble(rate)).ToString();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                //处理订单数
                StringBuilder builder2 = new StringBuilder();
                builder2.AppendFormat(DashboardSqls.SELECT_ORDERNUM_BY_SHOPID_DATE, shopId, yesterday, today, "2");
                string sql2 = builder2.ToString();
                DataTable dt2 = DatabaseOperationWeb.ExecuteSelectDS(sql2, "T").Tables[0];
                if (dt2 != null && dt2.Rows.Count == 1)
                {
                    orderNum = dt2.Rows[0]["ORDER_NUM"].ToString();
                }
                if (actualAmount == "")
                {
                    actualAmount = "0";
                }
                if (supplyAmount == "")
                {
                    supplyAmount = "0";
                }
                if (rate == "")
                {
                    rate = "0";
                }
                if (orderNum == "")
                {
                    orderNum = "0";
                }
                partSalesDay = new PartSalesDay
                {
                    actualAmount = actualAmount,           //日销售额
                    orderNum = orderNum,                   //订单数
                    rate = rate,                            //同比
                    supplyAmount = supplyAmount,           //应收账款
                    upOrDown = upOrDown,                  //上升还是下降
                };
            }

            return partSalesDay;
        }

        public MonthGroups OfflineGetMonthGroups(string shopId)
        {
            MonthGroups monthGroups = new MonthGroups();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_SHOPID_MONTH, shopId, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                //查询订单数
                StringBuilder builder1 = new StringBuilder();
                builder1.AppendFormat(DashboardSqls.SELECT_ORDERNUM_BY_SHOPID_MONTH, shopId, "2");
                string sql1 = builder1.ToString();
                DataTable dt1 = DatabaseOperationWeb.ExecuteSelectDS(sql1, "T").Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PartSalesMonth partSalesMonth = new PartSalesMonth
                    {
                        actualAmount = dt.Rows[i]["ACTUAL_AMOUNT"].ToString(),
                        month = dt.Rows[i]["MONTH"].ToString(),
                        monthDisplay = "",
                        orderNum = "",
                        rate = "0",
                        supplyAmount = dt.Rows[i]["SUPPLY_AMOUNT"].ToString(),
                        upOrDown = "0",
                    };
                    //处理月份起末
                    DateTime dtime = Convert.ToDateTime(dt.Rows[i]["MONTH"].ToString() + "-01");
                    string beginDate = dtime.ToString("yyyy-MM-dd");
                    string endDate = dtime.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                    partSalesMonth.monthDisplay = beginDate + "~" + endDate;
                    //处理订单数
                    DataRow[] drs = dt1.Select("month='" + dt.Rows[i]["MONTH"].ToString() + "'");
                    if (drs.Length > 0)
                    {
                        partSalesMonth.orderNum = drs[0]["ORDER_NUM"].ToString();
                    }
                    //处理比率

                    string lastYearMonth = dtime.AddYears(-1).ToString("yyyy-MM");
                    DataRow[] drs1 = dt.Select("month='" + lastYearMonth + "'");
                    if (drs1.Length > 0)
                    {
                        double d = 0;

                        try
                        {
                            double month1 = Convert.ToDouble(dt.Rows[i]["ACTUAL_AMOUNT"]);
                            double month2 = Convert.ToDouble(drs1[0]["ACTUAL_AMOUNT"]);
                            d = Math.Round((month1 - month2) / month2 * 100, 2);
                        }
                        catch (Exception)
                        {
                        }
                        if (d < 0)
                        {
                            d = 0 - d;
                            partSalesMonth.upOrDown = "1";
                        }
                        partSalesMonth.rate = d.ToString();
                    }
                    monthGroups.list.Add(partSalesMonth);
                    if (i >= 12)
                    {
                        break;
                    }
                }
            }
            return monthGroups;
        }

        public List<ProportionLegend> OfflineGetProportionLegend()
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-1).ToString("yyyy-MM-dd");
            List<ProportionLegend> proportionLegendList = new List<ProportionLegend>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_OFFLINESHOP_YESTERDAY, yesterday, today);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                double total = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        total += Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }
                total = Math.Round(total, 2);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double percent = 0;
                    try
                    {
                        percent = Math.Round(Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString()) / total * 100, 2);
                    }
                    catch (Exception)
                    {

                    }
                    ProportionLegend proportionLegend = new ProportionLegend
                    {
                        percentDisplay = percent.ToString() + "%",
                        shopName = dt.Rows[i]["USERNAME"].ToString(),
                    };
                    proportionLegendList.Add(proportionLegend);
                }


            }

            return proportionLegendList;
        }

        public List<ProportionValues> OfflineGetProportionValues()
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-1).ToString("yyyy-MM-dd");
            List<ProportionValues> proportionValuesList = new List<ProportionValues>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_AMOUNT_BY_OFFLINESHOP_YESTERDAY, yesterday, today);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                double total = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        total += Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }
                total = Math.Round(total, 2);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double percent = 0;
                    try
                    {
                        percent = Math.Round(Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString()) / total, 4);
                    }
                    catch (Exception)
                    {

                    }
                    ProportionValues proportionValues = new ProportionValues
                    {
                        constValue = "1",
                        shopName = dt.Rows[i]["USERNAME"].ToString(),
                        percent = percent,
                    };
                    proportionValuesList.Add(proportionValues);
                }

            }

            return proportionValuesList;
        }

        public List<DaySalesData> OfflineGetSalesTrendList(string shopId)
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-7).ToString("yyyy-MM-dd");
            List<DaySalesData> list = new List<DaySalesData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_SEVENAMOUNT_BY_SHOPID, shopId, yesterday, today, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime tempDay = dtime.AddDays(list.Count - 7);
                    while (tempDay.ToString("yyyy-MM-dd") != dt.Rows[i]["DAY"].ToString())
                    {
                        DaySalesData daySalesData = new DaySalesData
                        {
                            day = (list.Count + 1).ToString(),
                            money = 0,
                            title = tempDay.ToString("yyyy-MM-dd"),
                        };
                        list.Add(daySalesData);
                        tempDay = tempDay.AddDays(1);
                    }
                    DaySalesData daySalesData1 = new DaySalesData
                    {
                        day = (list.Count + 1).ToString(),
                        money =Convert.ToDouble( dt.Rows[i]["MONEY"].ToString()),
                        title = dt.Rows[i]["DAY"].ToString(),
                    };
                    list.Add(daySalesData1);
                }
            }
            return list;
        }

        public List<DayOrderData> OfflineGetOrderTrendList(string shopId)
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddDays(-7).ToString("yyyy-MM-dd");
            List<DayOrderData> list = new List<DayOrderData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_SEVENAMOUNT_BY_SHOPID, shopId, yesterday, today, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime tempDay = dtime.AddDays(list.Count - 7);
                    while (tempDay.ToString("yyyy-MM-dd") != dt.Rows[i]["DAY"].ToString())
                    {
                        DayOrderData dayOrderData = new DayOrderData
                        {
                            day = (list.Count + 1).ToString(),
                            count = 0,
                            title = tempDay.ToString("yyyy-MM-dd"),
                        };
                        list.Add(dayOrderData);
                        tempDay = tempDay.AddDays(1);
                    }
                    DayOrderData dayOrderData1 = new DayOrderData
                    {
                        day = (list.Count + 1).ToString(),
                        count =Convert.ToDouble( dt.Rows[i]["ORDERNUM"].ToString()),
                        title = dt.Rows[i]["DAY"].ToString(),
                    };
                    list.Add(dayOrderData1);
                }
            }
            return list;
        }

        public List<SellerGoodsData> OfflineGetBestSellerGoodsList(string shopId)
        {
            List<SellerGoodsData> list = new List<SellerGoodsData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_BEST_GOODS_BY_SHOPID, shopId, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    SellerGoodsData sellerGoodsData = new SellerGoodsData
                    {
                        brand = dt.Rows[i]["GOODSNAME"].ToString(),
                        count = Convert.ToDouble(dt.Rows[i]["COUNT"]),
                    };
                    list.Add(sellerGoodsData);
                }
            }
            return list;
        }

        public List<SellerGoodsData> OfflineGetLowSellerGoodsList(string shopId)
        {
            List<SellerGoodsData> list = new List<SellerGoodsData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_LOW_GOODS_BY_SHOPID, shopId, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    SellerGoodsData sellerGoodsData = new SellerGoodsData
                    {
                        brand = dt.Rows[i]["GOODSNAME"].ToString(),
                        count = Convert.ToDouble(dt.Rows[i]["COUNT"]),
                    };
                    list.Add(sellerGoodsData);
                }
            }
            return list;
        }

        public List<AccountsReceivableTMonthData> OfflineGetAccountsReceivableTRateList(string shopId)
        {
            List<AccountsReceivableTMonthData> list = new List<AccountsReceivableTMonthData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_ACCOUNTS_RECEIVABLE_TRATE_BY_SHOPID, shopId);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AccountsReceivableTMonthData accountsReceivableTMonthData = new AccountsReceivableTMonthData
                    {
                        rate = Convert.ToDouble(dr["ACCOUNTRATIO"]),
                        title = dr["ACCOUNTMONTH"].ToString(),
                    };
                    list.Add(accountsReceivableTMonthData);
                }
            }
            return list;
        }

        public List<MarketingDayData> OfflineGetMarketingDayList(string shopId)
        {
            List<MarketingDayData> list = new List<MarketingDayData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_MARKETING_RATE_BY_SHOPID_DAY, shopId,shopId, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MarketingDayData marketingDayData = new MarketingDayData
                    {
                        rate = Convert.ToDouble(dr["RATE"]),
                        title = dr["DATE1"].ToString(),
                    };
                    list.Add(marketingDayData);
                }
            }
            return list;
        }

        public List<StockTMonthData> OfflineGetStockTMonthList(string shopId)
        {
            DateTime dtime = DateTime.Now;
            string today = dtime.ToString("yyyy-MM-dd");
            string yesterday = dtime.AddMonths(-1).ToString("yyyy-MM-dd");
            List<StockTMonthData> list = new List<StockTMonthData>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(DashboardSqls.SELECT_STOCK_BY_SHOPID_DAY, shopId,shopId, yesterday,today, "2");
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StockTMonthData stockTMonthData = new StockTMonthData
                    {
                        rate = Convert.ToDouble(dr["RATE"]),
                        title = dr["TITLE"].ToString(),
                    };
                    list.Add(stockTMonthData);
                }
            }
            return list;
        }

        public class DashboardSqls
        {
            public const string SELECT_SHOPID_BY_ONLINE = "SELECT USERCODE,USERNAME FROM T_USER_LIST WHERE ifOnline='1' ";
            public const string SELECT_SHOPID_BY_OFFLINE = "SELECT USERCODE,USERNAME FROM T_USER_LIST WHERE ifOnline='0' ";
            public const string SELECT_DAILYAVERAGE_BY_SHOPID = "SELECT ROUND( SUM(TRADEAMOUNT)/COUNT(*),2) as DAILY_AVERAGE " +
                                                                "FROM T_ORDER_LIST WHERE ('{0}'='' or PURCHASERCODE = '{0}' ) AND APITYPE='{1}' ";
            public const string SELECT_AMOUNT_BY_SHOPID_DATE = "SELECT SUM(G.SKUUNITPRICE*G.QUANTITY) as ACTUAL_AMOUNT,SUM(G.PURCHASEPRICE*G.QUANTITY) as SUPPLY_AMOUNT " +
                "FROM T_ORDER_LIST O,T_ORDER_GOODS G " +
                "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND ('{0}'='' or O.PURCHASERCODE = '{0}')  AND APITYPE='{3}' " +
                               "AND TRADETIME BETWEEN STR_TO_DATE('{1}', '%Y-%m-%d') AND STR_TO_DATE('{2}', '%Y-%m-%d')";
            public const string SELECT_RATE_BY_SHOPID_DATE = "SELECT ROUND((A.S-B.S)/B.S*100,2) as RATE  " +
                "FROM (SELECT SUM(G.SKUUNITPRICE*G.QUANTITY) S FROM T_ORDER_LIST O,T_ORDER_GOODS G " +
                      "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND ('{0}'='' or O.PURCHASERCODE = '{0}') " +
                            "AND TRADETIME BETWEEN STR_TO_DATE('{1}', '%Y-%m-%d') AND STR_TO_DATE('{2}', '%Y-%m-%d')  AND APITYPE='{5}' ) A, " +
                     "(SELECT SUM(G.SKUUNITPRICE* G.QUANTITY) S FROM T_ORDER_LIST O, T_ORDER_GOODS G " +
                      "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND O.PURCHASERCODE = 'WXCCAIGOU' " +
                            "AND TRADETIME BETWEEN STR_TO_DATE('{3}', '%Y-%m-%d') AND STR_TO_DATE('{4}', '%Y-%m-%d')  AND APITYPE='{5}' ) B";
            public const string SELECT_ORDERNUM_BY_SHOPID_DATE = "SELECT COUNT(*) as ORDER_NUM FROM T_ORDER_LIST O " +
                "WHERE O.MERCHANTORDERID  AND ('{0}'='' or O.PURCHASERCODE = '{0}') " +
                "AND TRADETIME BETWEEN STR_TO_DATE('{1}', '%Y-%m-%d') AND STR_TO_DATE('{2}', '%Y-%m-%d') AND APITYPE='{3}' ";
            public const string SELECT_AMOUNT_BY_SHOPID_MONTH = "SELECT DATE_FORMAT(TRADETIME,'%Y-%m') MONTH,SUM(G.SKUUNITPRICE*G.QUANTITY) ACTUAL_AMOUNT,SUM(G.PURCHASEPRICE) SUPPLY_AMOUNT " +
                "FROM T_ORDER_LIST O,T_ORDER_GOODS G  " +
                "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND ('{0}'='' or O.PURCHASERCODE = '{0}') AND APITYPE='{1}'  GROUP BY DATE_FORMAT(TRADETIME,'%Y-%m') ORDER BY MONTH DESC";
            public const string SELECT_ORDERNUM_BY_SHOPID_MONTH = "SELECT DATE_FORMAT(TRADETIME,'%Y-%m') as MONTH,COUNT(*)as ORDER_NUM " +
                "FROM T_ORDER_LIST O " +
                "WHERE  ('{0}'='' or O.PURCHASERCODE = '{0}') AND APITYPE='{1}'  GROUP BY DATE_FORMAT(TRADETIME,'%Y-%m') ";
            public const string SELECT_AMOUNT_BY_ONLINESHOP_YESTERDAY = "SELECT USERNAME,SUM(G.SKUUNITPRICE*G.QUANTITY) " +
                "FROM T_ORDER_LIST O,T_ORDER_GOODS G ,T_USER_LIST U  " +
                "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND O.PURCHASERCODE = U.USERCODE " +
                      "AND TRADETIME BETWEEN STR_TO_DATE('{0}', '%Y-%m-%d') AND STR_TO_DATE('{1}', '%Y-%m-%d') " +
                      "AND APITYPE = 1 AND U.USERTYPE= '2' " +
                "GROUP BY PURCHASERCODE " +
                "UNION  " +
                "SELECT 'BBC' AS USERNAME, SUM(G.SKUUNITPRICE* G.QUANTITY) as AMOUNT " +
                "FROM T_ORDER_LIST O,T_ORDER_GOODS G, T_USER_LIST U " +
                "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND O.PURCHASERCODE = U.USERCODE " +
                      "AND TRADETIME BETWEEN STR_TO_DATE('{2}', '%Y-%m-%d') AND STR_TO_DATE('{3}', '%Y-%m-%d') " +
                      "AND APITYPE = 1 AND U.USERTYPE= '3'";
            public const string SELECT_AMOUNT_BY_OFFLINESHOP_YESTERDAY = "SELECT USERNAME,SUM(G.SKUUNITPRICE*G.QUANTITY) " +
                "FROM T_ORDER_LIST O,T_ORDER_GOODS G ,T_USER_LIST U  " +
                "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND O.PURCHASERCODE = U.USERCODE " +
                      "AND TRADETIME BETWEEN STR_TO_DATE('{0}', '%Y-%m-%d') AND STR_TO_DATE('{1}', '%Y-%m-%d') " +
                      "AND APITYPE = 2 AND U.USERTYPE= '2' " +
                "GROUP BY PURCHASERCODE ";
            public const string SELECT_SEVENAMOUNT_BY_SHOPID = "SELECT DATE_FORMAT(TRADETIME,'%Y-%m-%d') as DAY,COUNT(*) AS ORDERNUM, SUM(O.TRADEAMOUNT) AS MONEY " +
                "FROM T_ORDER_LIST O " +
                "WHERE ('{0}'='' or PURCHASERCODE = '{0}') AND APITYPE='{3}'  AND TRADETIME BETWEEN STR_TO_DATE('{1}', '%Y-%m-%d') AND STR_TO_DATE('{2}', '%Y-%m-%d') " +
                "GROUP BY DATE_FORMAT(TRADETIME,'%Y-%m-%d') " +
                "ORDER BY DATE_FORMAT(TRADETIME,'%Y-%m-%d') ASC ";
            public const string SELECT_BEST_GOODS_BY_SHOPID= "SELECT G.BARCODE,G.GOODSNAME,SUM(G.QUANTITY) AS COUNT " +
                "FROM T_ORDER_LIST O,T_ORDER_GOODS G  " +
                "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND ('{0}'='' or O.PURCHASERCODE = '{0}')  AND APITYPE='{1}' " +
                "GROUP BY G.BARCODE " +
                "ORDER BY SUM(G.QUANTITY) DESC   LIMIT 10";
            public const string SELECT_LOW_GOODS_BY_SHOPID= "SELECT G.BARCODE,G.GOODSNAME,SUM(G.QUANTITY) AS COUNT " +
                "FROM T_ORDER_LIST O,T_ORDER_GOODS G  " +
                "WHERE O.MERCHANTORDERID = G.MERCHANTORDERID AND ('{0}'='' or O.PURCHASERCODE = '{0}')  AND APITYPE='{1}' " +
                "GROUP BY G.BARCODE " +
                "ORDER BY SUM(G.QUANTITY) ASC LIMIT 10";
            public const string SELECT_ACCOUNTS_RECEIVABLE_TRATE_BY_SHOPID = "SELECT * FROM T_ACCOUNT_MONTH_RECEIVABLE WHERE ('{0}'='' or USERCODE = '{0}') ";
            public const string SELECT_MARKETING_RATE_BY_SHOPID_DAY = "SELECT  DATE1,ROUND(COUNT(*)/(SELECT COUNT(*) FROM T_GOODS_DISTRIBUTOR_PRICE WHERE ('{0}'='' or USERCODE = '{0}'))*100,2)  AS RATE " +
                "FROM (SELECT DATE_FORMAT(TRADETIME,'%Y-%m-%d') DATE1,BARCODE FROM T_ORDER_LIST O, T_ORDER_GOODS G WHERE  O.MERCHANTORDERID = G.MERCHANTORDERID AND ('{1}'='' or O.PURCHASERCODE = '{1}') AND APITYPE='{2}'  GROUP BY DATE_FORMAT(TRADETIME,'%Y-%m-%d') ,BARCODE) A " +
                "GROUP BY DATE1 ORDER BY DATE1 DESC  LIMIT 10";
            public const string SELECT_STOCK_BY_SHOPID_DAY = "SELECT DATE_FORMAT(TRADETIME,'%Y-%m-%d') AS TITLE ,SUM(G.QUANTITY),ROUND(SUM(G.QUANTITY)/( (SELECT SUM(PNUM) FROM T_GOODS_DISTRIBUTOR_PRICE WHERE ('{0}'='' or USERCODE ='{0}') ) /DATE_FORMAT(NOW(),'%d')),2) AS RATE " +
                "FROM T_ORDER_LIST O ,T_ORDER_GOODS G " +
                "WHERE O.MERCHANTORDERID= G.MERCHANTORDERID AND  TRADETIME BETWEEN STR_TO_DATE('{2}', '%Y-%m-%d') AND STR_TO_DATE('{3}', '%Y-%m-%d') AND ('{1}'='' or PURCHASERCODE = '{1}')  AND APITYPE='{4}' " +
                "GROUP BY DATE_FORMAT(TRADETIME,'%Y-%m-%d') ORDER BY DATE_FORMAT(TRADETIME,'%Y-%m-%d') DESC LIMIT 10";

        }
    }
}
