using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    #region Sys
    public class BussCache
    {
        private string unique = "";
        public string Unique
        {
            get
            {
                return unique;
            }
            set
            {
                unique = value;
            }
        }
    }

    public class BussParam
    {
        public string GetUnique()
        {
            string needMd5 = "";
            string md5S = "";
            foreach (FieldInfo f in this.GetType().GetFields())
            {
                needMd5 += f.Name;
                needMd5 += f.GetValue(this).ToString();
            }
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(needMd5));
                var strResult = BitConverter.ToString(result);
                md5S = strResult.Replace("-", "");
            }
            return md5S;
        }
    }

    public class SessionUser
    {
        public string openid;
        public string checkPhone;
        public string checkCode;
        public string userType;
        public string userId;
    }

    public class SmsCodeRes
    {
        public int error_code;
        public string reason;
    }

    public class WsPayState
    {
        public string userId;
        public string scanCode;
    }

    public class ExchangeRes
    {
        public string reason;
        public ExchangeResult result;
        public int error_code;
    }
    public class ExchangeResult
    {
        public string update;
        public List<string[]> list;
    }

    public enum ScanCodeType
    {
        Shop,
        User,
        Null,
    }

    #endregion

    #region Params

    public class LoginParam
    {
        public string code;
    }

    public class UserRegParam
    {
        public string avatarUrl;
        public string city;
        public string country;
        public string gender;
        public string language;
        public string nickName;
        public string province;
        public string userCode;
    }

    public class GetOnlineShopDataParam : BussParam
    {
        public string shopId;
    }

    public class GetOfflineShopDataParam : BussParam
    {
        public string shopId;
    }
    public class GetHomePageParam : BussParam
    {
        public string shopId;
    }
    public class GetTradeParam : BussParam
    {
        public string shopId;
    }
    #endregion

    #region DaoObjs

    public class User
    {
        public string userName;
        public string userId;
        public string openid;
        public string userImg;
        public string phone;
        public string userType;
        public string scanCode;
        public string sex;
    }

    public class OnlineData : BussCache
    {
        public Shops shops;
        public PartSales partSales;
        public Proportion proportion;
        public SalesTrendData salesTrendData;
        public OrderTrendData orderTrendData;
        public BestSellerGoodsData bestSellerGoodsData;
        public LowSellerGoodsData lowSellerGoodsData;
        public AccountsReceivableTRateData accountsReceivableTRateData;
    }

    public class OfflineData : BussCache
    {
        public Shops shops;
        public PartSales partSales;
        public Proportion proportion;
        public SalesTrendData salesTrendData;
        public OrderTrendData orderTrendData;
        public BestSellerGoodsData bestSellerGoodsData;
        public LowSellerGoodsData lowSellerGoodsData;
        public AccountsReceivableTRateData accountsReceivableTRateData;
        public MarketingRateData marketingRateData;
        public StockTRateData stockTRateData;

    }
    public class HomePageData : BussCache
    {
        public PartSalesHP partSales;
        public SalesTrendDataHP salesTrendData;
        public SalesShareDataHP SalesShareData;
    }
    public class TradeData : BussCache
    {
        public Shops shops;
        public PartSales partSales;
        public SalesTrendDataHP salesTrendData;
    }
    public class Shops
    {
        public List<Shop> list = new List<Shop>();
    }

    public class Shop
    {
        public string shopId;
        public string shopName;
    }

    public class MonthGroups
    {
        public List<PartSalesMonth> list = new List<PartSalesMonth>();
    }

    public class PartSales
    {
        public string dailyAverage;
        public PartSalesDay partSalesDay;
        public MonthGroups monthGroups;
    }

    public class PartSalesDay
    {
        public string upOrDown;
        public string rate;
        public string actualAmount;
        public string supplyAmount;
        public string orderNum;
    }

    public class PartSalesMonth
    {
        public string month;
        public string monthDisplay;
        public string upOrDown;
        public string rate;
        public string actualAmount;
        public string supplyAmount;
        public string orderNum;
    }

    public class Proportion
    {
        public List<ProportionLegend> proportionLegend;
        public List<ProportionValues> proportionValues;
    }

    public class ProportionLegend
    {
        public string shopName;
        public string percentDisplay;
    }

    public class ProportionValues
    {
        public string shopName;
        public double percent;
        public string constValue;
    }

    public class SalesTrendData
    {
        public List<DaySalesData> list = new List<DaySalesData>();
    }

    public class DaySalesData
    {
        public double money;
        public string day;
        public string title;
    }

    public class OrderTrendData
    {
        public List<DayOrderData> list = new List<DayOrderData>();
    }

    public class DayOrderData
    {
        public double count;
        public string day;
        public string title;
    }

    public class BestSellerGoodsData
    {
        public List<SellerGoodsData> list = new List<SellerGoodsData>();
    }

    public class SellerGoodsData
    {
        public string brand;
        public double count;
    }

    public class LowSellerGoodsData
    {
        public List<SellerGoodsData> list = new List<SellerGoodsData>();
    }

    public class AccountsReceivableTRateData
    {
        public List<AccountsReceivableTMonthData> list = new List<AccountsReceivableTMonthData>();
    }

    public class AccountsReceivableTMonthData
    {
        public double rate;
        public string title;
    }

    public class MarketingRateData
    {
        public List<MarketingDayData> list = new List<MarketingDayData>();
    }

    public class MarketingDayData
    {
        public double rate;
        public string title;
    }

    public class StockTRateData
    {
        public List<StockTMonthData> list = new List<StockTMonthData>();
    }

    public class StockTMonthData
    {
        public double rate;
        public string title;
    }
    
    public class PartSalesHP
    {
        public string sales="0";
        public string platformProfit="0";
    }
    public class SalesTrendDataHP
    {
        public string status = "0";
        public List<SalesTrend> salesTrends= new List<SalesTrend>();
    }
    public class SalesTrend
    {
        public string month;
        public string type;
        public double value;
    }
    public class SalesShareDataHP
    {
        public List<SalesShareMap> map =new List<SalesShareMap>();
        public List<SalesShareData> data = new List<SalesShareData>();
    }
    public class SalesShareMap
    {
        public string rate;
        public string title;
    }
    public class SalesShareData
    {
        public string name;
        public double percent;
        public string a;
    }
    #endregion
}
