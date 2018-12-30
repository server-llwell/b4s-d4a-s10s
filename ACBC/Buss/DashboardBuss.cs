using ACBC.Common;
using ACBC.Dao;
using Newtonsoft.Json;

namespace ACBC.Buss
{
    public class DashboardBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.DashboardApi;
        }

        public object Do_GetOnlineShopData(BaseApi baseApi)
        {
            GetOnlineShopDataParam getOnlineShopDataParam = JsonConvert.DeserializeObject<GetOnlineShopDataParam>(baseApi.param.ToString());
            if (getOnlineShopDataParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            OnlineData onlineData = Utils.GetCache<OnlineData>(getOnlineShopDataParam);

            if (onlineData == null)
            {
                string shopId = getOnlineShopDataParam.shopId;
                DashboardDao dashboardDao = new DashboardDao();
                Shops shops = dashboardDao.OnlineGetShops();

                PartSales partSales = new PartSales
                {
                    dailyAverage = dashboardDao.OnlineGetDailyAverage(shopId),
                    partSalesDay = dashboardDao.OnlineGetPartSalesDay(shopId),
                    monthGroups = dashboardDao.OnlineGetMonthGroups(shopId)
                };

                Proportion proportion = new Proportion
                {
                    proportionLegend = dashboardDao.OnlineGetProportionLegend(),
                    proportionValues = dashboardDao.OnlineGetProportionValues()
                };

                SalesTrendData salesTrendData = new SalesTrendData
                {
                    list = dashboardDao.OnlineGetSalesTrendList(shopId)
                };

                OrderTrendData orderTrendData = new OrderTrendData
                {
                    list = dashboardDao.OnlineGetOrderTrendList(shopId)
                };

                BestSellerGoodsData bestSellerGoodsData = new BestSellerGoodsData
                {
                    list = dashboardDao.OnlineGetBestSellerGoodsList(shopId)
                };

                LowSellerGoodsData lowSellerGoodsData = new LowSellerGoodsData
                {
                    list = dashboardDao.OnlineGetLowSellerGoodsList(shopId)
                };

                AccountsReceivableTRateData accountsReceivableTRateData = new AccountsReceivableTRateData
                {
                    list = dashboardDao.OnlineGetAccountsReceivableTRateList(shopId)
                };

                onlineData = new OnlineData
                {
                    accountsReceivableTRateData = accountsReceivableTRateData,
                    bestSellerGoodsData = bestSellerGoodsData,
                    lowSellerGoodsData = lowSellerGoodsData,
                    orderTrendData = orderTrendData,
                    partSales = partSales,
                    proportion = proportion,
                    salesTrendData = salesTrendData,
                    shops = shops
                };
                onlineData.Unique = getOnlineShopDataParam.GetUnique();
                Utils.SetCache(onlineData, 0, 1, 0);
            }

            return onlineData;
        }

        public object Do_GetOfflineShopData(BaseApi baseApi)
        {
            GetOfflineShopDataParam getOfflineShopDataParam = JsonConvert.DeserializeObject<GetOfflineShopDataParam>(baseApi.param.ToString());
            if (getOfflineShopDataParam == null)
            {
                throw new ApiException(CodeMessage.InvalidParam, "InvalidParam");
            }

            OfflineData offlineData = Utils.GetCache<OfflineData>(getOfflineShopDataParam);

            if (offlineData == null)
            {
                DashboardDao dashboardDao = new DashboardDao();
                Shops shops = dashboardDao.OfflineGetShops();

                string shopId = getOfflineShopDataParam.shopId;

                PartSales partSales = new PartSales
                {
                    dailyAverage = dashboardDao.OfflineGetDailyAverage(shopId),
                    partSalesDay = dashboardDao.OfflineGetPartSalesDay(shopId),
                    monthGroups = dashboardDao.OfflineGetMonthGroups(shopId)
                };

                Proportion proportion = new Proportion
                {
                    proportionLegend = dashboardDao.OfflineGetProportionLegend(),
                    proportionValues = dashboardDao.OfflineGetProportionValues()
                };

                SalesTrendData salesTrendData = new SalesTrendData
                {
                    list = dashboardDao.OfflineGetSalesTrendList(shopId)
                };

                OrderTrendData orderTrendData = new OrderTrendData
                {
                    list = dashboardDao.OfflineGetOrderTrendList(shopId)
                };

                BestSellerGoodsData bestSellerGoodsData = new BestSellerGoodsData
                {
                    list = dashboardDao.OfflineGetBestSellerGoodsList(shopId)
                };

                LowSellerGoodsData lowSellerGoodsData = new LowSellerGoodsData
                {
                    list = dashboardDao.OfflineGetLowSellerGoodsList(shopId)
                };

                AccountsReceivableTRateData accountsReceivableTRateData = new AccountsReceivableTRateData
                {
                    list = dashboardDao.OnlineGetAccountsReceivableTRateList(shopId)
                };

                MarketingRateData marketingRateData = new MarketingRateData
                {
                    list = dashboardDao.OfflineGetMarketingDayList(shopId)
                };

                StockTRateData stockTRateData = new StockTRateData
                {
                    list = dashboardDao.OfflineGetStockTMonthList(shopId)
                };
                offlineData = new OfflineData();
                offlineData.accountsReceivableTRateData = accountsReceivableTRateData;
                offlineData.bestSellerGoodsData = bestSellerGoodsData;
                offlineData.lowSellerGoodsData = lowSellerGoodsData;
                offlineData.orderTrendData = orderTrendData;
                offlineData.partSales = partSales;
                offlineData.proportion = proportion;
                offlineData.salesTrendData = salesTrendData;
                offlineData.shops = shops;
                offlineData.marketingRateData = marketingRateData;
                offlineData.stockTRateData = stockTRateData;
                offlineData.Unique = getOfflineShopDataParam.GetUnique();
                Utils.SetCache(offlineData, 0, 1, 0);
            }

            return offlineData;
        }
    }
}
