using MTSWebApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MTSWebApi.MTS_APISoapClient;
//using MTSWebApi.MTS_APISoapClient;

namespace WindowsServiceEndPoint
{
    public class MTS_Api
    {
        string serviceEndPoint = String.Empty;
        string sTPAPIKey = String.Empty;
        string sTradingPartnerID = String.Empty;
        string sEmailAddress = String.Empty;

        public MTS_Api(string _serviceEndPoint,
            string _sAPIKey,
            string _sTradingPartnerID,
            string _sEmailAddress)
        {
            serviceEndPoint = _serviceEndPoint;
            sTPAPIKey = _sAPIKey;
            sTradingPartnerID = _sTradingPartnerID;
            sEmailAddress = _sEmailAddress;
        }
        public async Task<ProductInfo> GetPortalProducts()
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            //client.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://localhost:49502/xml/MTSWebAPI.asmx");
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            RetrievePortalProductsResponse response = null;
            response = await client.RetrievePortalProductsAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress);
            return response.Body.RetrievePortalProductsResult;
        }

        public async Task<ProductsPricingInfo> GetAMarkWebProductsPricingToBuy(decimal decGoldTotal, decimal decSilverTotal, decimal decPlatinumTotal, decimal decPalladiumTotal)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response =  await client.RetrieveWebProductsPricingAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress, "S", decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);
            return response.Body.RetrieveWebProductsPricingResult;
            
        }

        public async Task<ProductsDiscountsInfo> GetAMarkWebProductsDiscounts()
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.RetrieveWebProductsDiscountsAsync(sTPAPIKey,sTradingPartnerID,sEmailAddress);
            return response.Body.RetrieveWebProductsDiscountsResult;
        }

        public async Task<CustomerCommodityLimitsInfo> GetAMarkWebTradingLimits(string sOrderType)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.GetCustomerCommodityLimitsAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress,sOrderType);
            return response.Body.GetCustomerCommodityLimitsResult;
        }

        public async Task<ProductsPricingInfo> GetAMarkWebProductsPricingToSell(decimal decGoldTotal, decimal decSilverTotal, decimal decPlatinumTotal, decimal decPalladiumTotal)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.RetrieveWebProductsPricingAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress, "B", decGoldTotal, decSilverTotal, decPlatinumTotal, decPalladiumTotal);
            return response.Body.RetrieveWebProductsPricingResult;
        }

        public async Task<ProductsQuoteInfo> RequestAmarkOnlineQuote(string sOrderType, List<ProductQuoteItem> lstProductQuoteItems, bool bHFIFlag, string sShippingType, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber, string sTP_Confirm_No, string sSpecialInstructions)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.RequestOnlineQuoteAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress, sOrderType, lstProductQuoteItems.ToArray(), bHFIFlag, sShippingType, sShippingName1, sShippingName2, sShippingAddress1, sShippingAddress2, sShippingCity, sShippingState, sShippingZipCode, sShippingCountry, sShippingPhoneNumber, sTP_Confirm_No, sSpecialInstructions);
            return response.Body.RequestOnlineQuoteResult;
        }

        public async Task<bool> AddressValid(string sCountry, string sCity, string sState, string sZip)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.IsAddressValidAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress, sCountry, sCity, sState, sZip);
            return response.Body.IsAddressValidResult;
        }

        public async Task<RequestOnlineTradeInfo> RequestAmarkOnlineTrade(string sOnline_Request_Type, string sQuoteKey, bool bHFIFlag, string sShippingType, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber, string sTP_Confirm_No)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.RequestOnlineTradeAsync(sOnline_Request_Type, sTPAPIKey, sTradingPartnerID, sEmailAddress, sQuoteKey, bHFIFlag, sShippingType, sShippingName1, sShippingName2, sShippingAddress1, sShippingAddress2, sShippingCity, sShippingState, sShippingZipCode, sShippingCountry, sShippingPhoneNumber, sTP_Confirm_No);
            return response.Body.RequestOnlineTradeResult;
        }

        public async Task<OpenPositionInfo> RetrieveOpenPositions()
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.RetrieveOpenPositionsAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress);
            return response.Body.RetrieveOpenPositionsResult;
        }

        public async Task<TradingHistoryInfo> RetrieveTradingHistory(DateTime dtBegin, DateTime dtEnd, int iPageNumber, int iPageSize)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.RetrieveTradingHistoryAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress, dtBegin, dtEnd, iPageNumber, iPageSize);
            return response.Body.RetrieveTradingHistoryResult;
        }

        public async Task<bool> UpdateOrderShippingInfoWithOrderHdrID(string sRequestID, string sOrderHdrID, string sShippingType, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber, string sUserID)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.UpdateOrderShippingInfoWithOrderHdrIDAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress, sRequestID, sOrderHdrID, sShippingType, sShippingName1, sShippingName2, sShippingAddress1, sShippingAddress2, sShippingCity, sShippingState, sShippingZipCode, sShippingCountry, sShippingPhoneNumber, sUserID);
            return response.Body.UpdateOrderShippingInfoWithOrderHdrIDResult;
        }

        public async Task<ShipToAddressItem> GetOrderShippingItem(int iRequestID, int iOrderHdrID)
        {
            MTS_APISoapClient client = new MTS_APISoapClient(EndpointConfiguration.MTS_APISoap);
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceEndPoint);
            client.Endpoint.EndpointBehaviors.Add(new SOAPEndpointBehaviour());
            var response = await client.GetShipToAddressAsync(sTPAPIKey, sTradingPartnerID, sEmailAddress, iRequestID.ToString(), iOrderHdrID.ToString());
            return response.Body.GetShipToAddressResult;
        }
    }
}
