
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.ViewModels;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation;
using System;
using MTSWebApi;
using System.IO;

namespace TradingPortal.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class OpenPositionsController : BaseController
    {
        //private readonly IHttpContextAccessor _context;
        private readonly IRequestForOrderManager _requestForOrderManager;
        private readonly IExportManager _exportManager;

        public OpenPositionsController(
            IHttpContextAccessor context,
            IRequestForOrderManager requestForOrderManager,
            IExportManager exportManager) : base(context)
        {
            _requestForOrderManager = requestForOrderManager;
            _exportManager = exportManager;
        }


        [HttpGet("openpositionsselect")]
        public async Task<List<OpenModel>> OpenPositionsSelect()
        {
            var openPosition = await _requestForOrderManager.GetOpenPositions();
            var gridModel = new GridOpenModel();
            var query = (from op in openPosition.objOpenPositionList
                         select new OpenModel
                         {
                             iOrder_Hdr_ID = op.iOrder_Hdr_ID,
                             sdtTrade = op.dtTrade.ToShortDateString(),
                             sdtValue = op.dtValue.ToShortDateString(),
                             sTicketNo = op.sTicketNo,
                             sTPConfirmNo = op.sTPConfirmNo,
                             sTradeType = op.sTradeType,
                             sProductDescription = op.sProductDescription,
                             iQuantity = op.iQuantity.ToString(),
                             decProductBalance = op.decProductBalance,
                             decCashBalance = op.decCashBalance,
                             sTrackingNumbers = op.sTrackingNumbers,
                             sReceived = op.sReceived,
                             sShipping = op.sShipping,
                             sCurrencySymbol = op.sCurrencySymbol
                         }).ToList();

            return query;
        }

        [HttpGet("tradinghistoryselect")]
        public async Task<GridOpenModel> TradingHistorySelect(OpenPositionsModel model)
        {
            //model.PageSize = 10000;
            var tradingHistory = await _requestForOrderManager.GetTradingHistory(model.StartDate ?? DateTime.Now.AddMonths(-1),
                            model.EndDate ?? DateTime.Now, model.Page,model.PageSize);

            var gridModel = new GridOpenModel();
            gridModel.OpenModels = (from th in tradingHistory.objTradingHistoryList
                         select new OpenModel
                         {
                             iOrder_Hdr_ID = th.iOrder_Hdr_ID,
                             sdtTrade = th.dtTrade.ToShortDateString(),
                             sdtValue = th.dtValue.ToShortDateString(),
                             sTicketNo = th.sTicketNo,
                             sTPConfirmNo = th.sTPConfirmNo,
                             sTradeType = th.sTradeType,
                             sProductDescription = th.sProductDescription,
                             iQuantity = th.iQuantity.ToString(),
                             decPrice = th.decPrice,
                             decTotalAmount = th.decTotalAmount,
                             sTrackingNumbers = th.sTrackingNumbers,
                             sReceived = th.sReceived,
                             sShipping = th.sShipping,
                             sCurrencySymbol = th.sCurrencySymbol
                         }).ToList();
            if (tradingHistory.objTradingHistoryList.Count() != 0)
            {
                gridModel.Total = tradingHistory.objTradingHistoryList.First().iTotalRecords;
            }
            return gridModel;
        }

        [HttpGet("updateordershippinginfo")]
        public async Task<UpdateShippingInfoResult> UpdateOrderShippingInfo(int iRequestID, int iOrderHdrID, string sShippingName1, string sShippingName2, string sShippingAddress1, string sShippingAddress2, string sShippingCity, string sShippingState, string sShippingZipCode, string sShippingCountry, string sShippingPhoneNumber, string rqs)
        {
            bool blnretval = false;
            string sZip;
            try
            {
                if (sShippingCountry == "US" || sShippingCountry == "USA" || sShippingCountry == "United States")
                {
                    if (sShippingState.Length > 2)
                    {
                        sShippingState = sShippingState.Substring(0, 2);
                    }
                }
                sZip = sShippingZipCode;
                if (sShippingZipCode.Length > 5)
                {
                    sShippingZipCode = sShippingZipCode.Substring(0, 5);
                }

                if (await _requestForOrderManager.AddressValid(sShippingCountry, sShippingCity, sShippingState, sShippingZipCode))
                {
                    blnretval = await _requestForOrderManager.UpdateOrderShippingInfo(iRequestID, iOrderHdrID, sShippingName1, sShippingName2, sShippingAddress1, sShippingAddress2, sShippingCity, sShippingState, sZip, sShippingCountry, sShippingPhoneNumber ?? "");
                }
            }
            catch (Exception ex)
            {
                blnretval = false;
            }

            return new UpdateShippingInfoResult
            {
                IsValid = blnretval,
                QValue = rqs
            };
        }

        [HttpGet("getordershippingitem")]
        public async Task<ShippingInfo> GetOrderShippingItem(int iOrderHdrID)
        {
            ShipToAddressItem address = new ShipToAddressItem();
            ShippingInfo shippingInfo = null;
            try
            {
                address = await _requestForOrderManager.GetOrderShippingItem(-1, iOrderHdrID);
                shippingInfo = new ShippingInfo
                {
                    iOrderHdrID = iOrderHdrID,
                    sShippingName1 = address.sShip_To_Name1,
                    sShippingName2 = address.sShip_To_Name2,
                    sShippingAddress1 = address.sShip_To_Address1,
                    sShippingAddress2 = address.sShip_To_Address2,
                    sShippingState = address.sShip_To_State,
                    sShippingCity = address.sShip_To_City,
                    sShippingCountry = address.sShip_To_Country,
                    sShippingZipCode = address.sShip_To_Zip,
                    sShippingPhoneNumber = address.sShip_To_Phone ?? "",
                    rqs = "0"
                };
            }
            catch (Exception ex)
            {
            }
            return shippingInfo;
        }

        [Route("ExportExcelAll")]
        public async Task<ActionResult> ExportExcelAll()
        {
            try
            {
                OpenPositionInfo openPositions = await _requestForOrderManager.GetOpenPositions();

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportOpenPositionsToXlsx(stream, openPositions.objOpenPositionList.ToList());
                    bytes = stream.ToArray();
                }
                return File(bytes, "text/xls", "openpositions.xlsx");
            }
            catch (Exception exc)
            {
                throw exc;
                //ErrorNotification(exc);
                //return RedirectToAction("List");
            }

        }

        [Route("ExportExcelSelected")]
        public async Task<ActionResult> ExportExcelSelected(DateTime StartDate, DateTime EndDate, int Page, int PageSize)
        {
            try
            {
                PageSize = 2000;
                TradingHistoryInfo tradingHistory = await _requestForOrderManager.GetTradingHistory(StartDate != null ? StartDate : DateTime.Now.AddMonths(-1),
                EndDate != null ? EndDate : DateTime.Now, Page, PageSize);

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportTradingHistoryToXlsx(stream, tradingHistory.objTradingHistoryList.ToList());
                    bytes = stream.ToArray();
                }
                return File(bytes, "text/xls", "tradinghistory.xlsx");
            }
            catch (Exception exc)
            {
                throw exc;
                //ErrorNotification(exc);
                //return RedirectToAction("List");
            }
        }

    }

    
}




