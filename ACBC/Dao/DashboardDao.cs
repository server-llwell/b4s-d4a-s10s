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
        //public Shops OnlineGetShops()
        //{
        //    Shops shops = new Shops();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls.SELECT_SHOPID_BY_ONLINE);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            Shop shop = new Shop
        //            {
        //                shopId = dr["SHOP_ID"].ToString(),
        //                shopName = dr["SHOP_NAME"].ToString(),
        //            };
        //            shops.list.Add(shop);
        //        }
        //    }
        //    return shops;
        //}

        //public string OnlineGetDailyAverage(string shopId)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null && dt.Rows.Count == 1)
        //    {
        //        return dt.Rows[0]["DAILY_AVERAGE"].ToString();
        //    }

        //    return null;
        //}

        //public PartSalesDay OnlineGetPartSalesDay(string shopId)
        //{
        //    PartSalesDay partSalesDay = null;
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if(dt != null && dt.Rows.Count == 1)
        //    {
        //        partSalesDay = new PartSalesDay
        //        {
        //            actualAmount = dt.Rows[0]["ACTUAL_AMOUNT"].ToString(),
        //            orderNum = dt.Rows[0]["ORDER_NUM"].ToString(),
        //            rate = dt.Rows[0]["RATE"].ToString(),
        //            supplyAmount = dt.Rows[0]["SUPPLY_AMOUNT"].ToString(),
        //            upOrDown = dt.Rows[0]["UP_OR_DOWN"].ToString(),
        //        };
        //    }

        //    return partSalesDay;
        //}

        //public MonthGroups OnlineGetMonthGroups(string shopId)
        //{
        //    MonthGroups monthGroups = new MonthGroups();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            PartSalesMonth partSalesMonth = new PartSalesMonth
        //            {
        //                actualAmount = dr["ACTUAL_AMOUNT"].ToString(),
        //                month = dr["MONTH"].ToString(),
        //                monthDisplay = dr["MONTH_DISPLAY"].ToString(),
        //                orderNum = dr["ORDER_NUM"].ToString(),
        //                rate = dr["RATE"].ToString(),
        //                supplyAmount = dr["SUPPLY_AMOUNT"].ToString(),
        //                upOrDown = dr["UP_OR_DOWN"].ToString(),
        //            };
        //            monthGroups.list.Add(partSalesMonth);
        //        }
        //    }
        //    return monthGroups;
        //}

        //public ProportionLegend OnlineGetProportionLegend()
        //{
        //    ProportionLegend proportionLegend = null;
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls.);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null && dt.Rows.Count == 1)
        //    {
        //        proportionLegend = new ProportionLegend
        //        {
        //            percentDisplay = dt.Rows[0]["PERCENT_DISPLAY"].ToString(),
        //            shopName = dt.Rows[0]["SHOP_NAME"].ToString(),
        //        };
        //    }

        //    return proportionLegend;
        //}

        //public ProportionValues OnlineGetProportionValues()
        //{
        //    ProportionValues proportionValues = null;
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls.);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null && dt.Rows.Count == 1)
        //    {
        //        proportionValues = new ProportionValues
        //        {
        //            constValue = dt.Rows[0]["CONST_VALUE"].ToString(),
        //            shopName = dt.Rows[0]["SHOP_NAME"].ToString(),
        //            percent = Convert.ToInt32(dt.Rows[0]["PERCENT"]),
        //        };
        //    }

        //    return proportionValues;
        //}

        //public List<DaySalesData> OnlineGetSalesTrendList(string shopId)
        //{
        //    List<DaySalesData> list = new List<DaySalesData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            DaySalesData daySalesData = new DaySalesData
        //            {
        //                day = dr["DAY"].ToString(),
        //                money = dr["MONEY"].ToString(),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(daySalesData);
        //        }
        //    }
        //    return list;
        //}

        //public List<DayOrderData> OnlineGetOrderTrendList(string shopId)
        //{
        //    List<DayOrderData> list = new List<DayOrderData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            DayOrderData dayOrderData = new DayOrderData
        //            {
        //                day = dr["DAY"].ToString(),
        //                count = dr["COUNT"].ToString(),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(dayOrderData);
        //        }
        //    }
        //    return list;
        //}

        //public List<SellerGoodsData> OnlineGetBestSellerGoodsList(string shopId)
        //{
        //    List<SellerGoodsData> list = new List<SellerGoodsData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            SellerGoodsData sellerGoodsData = new SellerGoodsData
        //            {
        //                brand = dr["BRAND"].ToString(),
        //                count = dr["COUNT"].ToString(),
        //            };
        //            list.Add(sellerGoodsData);
        //        }
        //    }
        //    return list;
        //}

        //public List<SellerGoodsData> OnlineGetLowSellerGoodsList(string shopId)
        //{
        //    List<SellerGoodsData> list = new List<SellerGoodsData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            SellerGoodsData sellerGoodsData = new SellerGoodsData
        //            {
        //                brand = dr["BRAND"].ToString(),
        //                count = dr["COUNT"].ToString(),
        //            };
        //            list.Add(sellerGoodsData);
        //        }
        //    }
        //    return list;
        //}

        //public List<AccountsReceivableTMonthData> OnlineGetAccountsReceivableTRateList(string shopId)
        //{
        //    List<AccountsReceivableTMonthData> list = new List<AccountsReceivableTMonthData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            AccountsReceivableTMonthData accountsReceivableTMonthData = new AccountsReceivableTMonthData
        //            {
        //                rate = Convert.ToDouble(dr["RATE"]),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(accountsReceivableTMonthData);
        //        }
        //    }
        //    return list;
        //}



        //public Shops OfflineGetShops()
        //{
        //    Shops shops = new Shops();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls.);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            Shop shop = new Shop
        //            {
        //                shopId = dr["SHOP_ID"].ToString(),
        //                shopName = dr["SHOP_NAME"].ToString(),
        //            };
        //            shops.list.Add(shop);
        //        }
        //    }
        //    return shops;
        //}

        //public string OfflineGetDailyAverage(string shopId)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null && dt.Rows.Count == 1)
        //    {
        //        return dt.Rows[0]["DAILY_AVERAGE"].ToString();
        //    }

        //    return null;
        //}

        //public PartSalesDay OfflineGetPartSalesDay(string shopId)
        //{
        //    PartSalesDay partSalesDay = null;
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null && dt.Rows.Count == 1)
        //    {
        //        partSalesDay = new PartSalesDay
        //        {
        //            actualAmount = dt.Rows[0]["ACTUAL_AMOUNT"].ToString(),
        //            orderNum = dt.Rows[0]["ORDER_NUM"].ToString(),
        //            rate = dt.Rows[0]["RATE"].ToString(),
        //            supplyAmount = dt.Rows[0]["SUPPLY_AMOUNT"].ToString(),
        //            upOrDown = dt.Rows[0]["UP_OR_DOWN"].ToString(),
        //        };
        //    }

        //    return partSalesDay;
        //}

        //public MonthGroups OfflineGetMonthGroups(string shopId)
        //{
        //    MonthGroups monthGroups = new MonthGroups();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            PartSalesMonth partSalesMonth = new PartSalesMonth
        //            {
        //                actualAmount = dr["ACTUAL_AMOUNT"].ToString(),
        //                month = dr["MONTH"].ToString(),
        //                monthDisplay = dr["MONTH_DISPLAY"].ToString(),
        //                orderNum = dr["ORDER_NUM"].ToString(),
        //                rate = dr["RATE"].ToString(),
        //                supplyAmount = dr["SUPPLY_AMOUNT"].ToString(),
        //                upOrDown = dr["UP_OR_DOWN"].ToString(),
        //            };
        //            monthGroups.list.Add(partSalesMonth);
        //        }
        //    }
        //    return monthGroups;
        //}

        //public ProportionLegend OfflineGetProportionLegend()
        //{
        //    ProportionLegend proportionLegend = null;
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls.);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null && dt.Rows.Count == 1)
        //    {
        //        proportionLegend = new ProportionLegend
        //        {
        //            percentDisplay = dt.Rows[0]["PERCENT_DISPLAY"].ToString(),
        //            shopName = dt.Rows[0]["SHOP_NAME"].ToString(),
        //        };
        //    }

        //    return proportionLegend;
        //}

        //public ProportionValues OfflineGetProportionValues()
        //{
        //    ProportionValues proportionValues = null;
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls.);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null && dt.Rows.Count == 1)
        //    {
        //        proportionValues = new ProportionValues
        //        {
        //            constValue = dt.Rows[0]["CONST_VALUE"].ToString(),
        //            shopName = dt.Rows[0]["SHOP_NAME"].ToString(),
        //            percent = Convert.ToInt32(dt.Rows[0]["PERCENT"]),
        //        };
        //    }

        //    return proportionValues;
        //}

        //public List<DaySalesData> OfflineGetSalesTrendList(string shopId)
        //{
        //    List<DaySalesData> list = new List<DaySalesData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            DaySalesData daySalesData = new DaySalesData
        //            {
        //                day = dr["DAY"].ToString(),
        //                money = dr["MONEY"].ToString(),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(daySalesData);
        //        }
        //    }
        //    return list;
        //}

        //public List<DayOrderData> OfflineGetOrderTrendList(string shopId)
        //{
        //    List<DayOrderData> list = new List<DayOrderData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            DayOrderData dayOrderData = new DayOrderData
        //            {
        //                day = dr["DAY"].ToString(),
        //                count = dr["COUNT"].ToString(),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(dayOrderData);
        //        }
        //    }
        //    return list;
        //}

        //public List<SellerGoodsData> OfflineGetBestSellerGoodsList(string shopId)
        //{
        //    List<SellerGoodsData> list = new List<SellerGoodsData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            SellerGoodsData sellerGoodsData = new SellerGoodsData
        //            {
        //                brand = dr["BRAND"].ToString(),
        //                count = dr["COUNT"].ToString(),
        //            };
        //            list.Add(sellerGoodsData);
        //        }
        //    }
        //    return list;
        //}

        //public List<SellerGoodsData> OfflineGetLowSellerGoodsList(string shopId)
        //{
        //    List<SellerGoodsData> list = new List<SellerGoodsData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            SellerGoodsData sellerGoodsData = new SellerGoodsData
        //            {
        //                brand = dr["BRAND"].ToString(),
        //                count = dr["COUNT"].ToString(),
        //            };
        //            list.Add(sellerGoodsData);
        //        }
        //    }
        //    return list;
        //}

        //public List<AccountsReceivableTMonthData> OfflineGetAccountsReceivableTRateList(string shopId)
        //{
        //    List<AccountsReceivableTMonthData> list = new List<AccountsReceivableTMonthData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            AccountsReceivableTMonthData accountsReceivableTMonthData = new AccountsReceivableTMonthData
        //            {
        //                rate = Convert.ToDouble(dr["RATE"]),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(accountsReceivableTMonthData);
        //        }
        //    }
        //    return list;
        //}

        //public List<MarketingDayData> OfflineGetMarketingDayList(string shopId)
        //{
        //    List<MarketingDayData> list = new List<MarketingDayData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            MarketingDayData marketingDayData = new MarketingDayData
        //            {
        //                rate = Convert.ToDouble(dr["RATE"]),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(marketingDayData);
        //        }
        //    }
        //    return list;
        //}

        //public List<StockTMonthData> OfflineGetStockTMonthList(string shopId)
        //{
        //    List<StockTMonthData> list = new List<StockTMonthData>();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat(DashboardSqls., shopId);
        //    string sql = builder.ToString();
        //    DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            StockTMonthData stockTMonthData = new StockTMonthData
        //            {
        //                rate = Convert.ToDouble(dr["RATE"]),
        //                title = dr["TITLE"].ToString(),
        //            };
        //            list.Add(stockTMonthData);
        //        }
        //    }
        //    return list;
        //}

        public class DashboardSqls
        {
            public const string SELECT_SHOPID_BY_ONLINE = "SELECT USERCODE,USERNAME FROM T_USER_LIST WHERE USERTYPE=3";
        }
    }
}
