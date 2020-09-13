using MTSWebApi;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TradingPortal.Business.interfaces;

namespace TradingPortal.Business
{
    public class ExportManager : IExportManager
    {
        public void ExportOpenPositionsToXlsx(Stream stream, List<OpenPositionItem> openpositions)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("openpositions");
                //Create Headers and format them 
                var properties = new string[]
                {
                    "TradeDate",
                    "ValueDate",
                    "Ticket#",
                    //"YourConfirm#",
                    "YouHave",
                    "ProductDescription",
                    "Quantity",
                    "ProductBalance",
                    "CashBalance",
                    "ShippingStatus",
                };
                for (int i = 0; i < properties.Length; i++)
                {
                    if (i == 2 || i == 3 || i == 4)
                    {
                        worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }


                int row = 2;
                foreach (var p in openpositions)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = p.dtTrade.ToShortDateString();
                    col++;

                    worksheet.Cells[row, col].Value = p.dtValue.ToShortDateString();
                    col++;

                    worksheet.Cells[row, col].Value = p.sTicketNo;
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    col++;

                    //worksheet.Cells[row, col].Value = p.sTPConfirmNo;
                    //worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    //col++;

                    worksheet.Cells[row, col].Value = p.sTradeType;
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    col++;

                    worksheet.Cells[row, col].Value = p.sProductDescription;
                    col++;

                    worksheet.Cells[row, col].Value = p.iQuantity;
                    col++;

                    worksheet.Cells[row, col].Value = p.decProductBalance;
                    col++;

                    worksheet.Cells[row, col].Value = string.Format("{0}{1}", p.sCurrencySymbol, p.decCashBalance);
                    col++;

                    if (p.sTrackingNumbers[0].Length > 1)
                    {
                        string sTrackingNumbers = "";

                        foreach (string s in p.sTrackingNumbers)
                        {
                            //sTrackingNumbers = sTrackingNumbers + string.Format("http://www.fedex.com/Tracking?action=track&language=english&cntry_code=us&initial=x&mps=y&tracknumbers={0} ", p.sTrackingNumbers);
                            //sTrackingNumbers = sTrackingNumbers + string.Format("{0} ", p.sTrackingNumbers) ;
                            sTrackingNumbers = sTrackingNumbers + string.Format("{0} ", p.sTrackingNumbers).Substring(string.Format("{0} ", p.sTrackingNumbers).LastIndexOf('=') + 1);
                        }
                        worksheet.Cells[row, col].Value = sTrackingNumbers;

                    }
                    else
                    {
                        if (p.sReceived != "")
                        {
                            worksheet.Cells[row, col].Value = "Received";
                        }
                        else
                        {
                            if (p.sShipping != "")
                            {
                                worksheet.Cells[row, col].Value = p.sShipping;
                            }
                        }
                    }
                    col++;

                    row++;
                }

                // save the new spreadsheet
                xlPackage.Save();
            }
         }

        /// <summary>
        /// Export tradinghistory to XLSX
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="products">Products</param>
        public virtual void ExportTradingHistoryToXlsx(Stream stream, List<TradingHistoryItem> tradinghistory)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("tradinghistory");
                //Create Headers and format them 
                var properties = new string[]
                {
                    "TradeDate",
                    "ValueDate",
                    "Ticket#",
                    //"YourConfirm#",
                    "YouHave",
                    "ProductDescription",
                    "Quantity",
                    "Price",
                    "Total Amount",
                    "ShippingStatus",
                };
                for (int i = 0; i < properties.Length; i++)
                {
                    if (i == 2 || i == 3 || i == 4)
                    {
                        worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }


                int row = 2;
                foreach (var p in tradinghistory)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = p.dtTrade.ToShortDateString();
                    col++;

                    worksheet.Cells[row, col].Value = p.dtValue.ToShortDateString();
                    col++;

                    worksheet.Cells[row, col].Value = p.sTicketNo;
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    col++;

                    //worksheet.Cells[row, col].Value = p.sTPConfirmNo;
                    //worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    //col++;

                    worksheet.Cells[row, col].Value = p.sTradeType;
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    col++;

                    worksheet.Cells[row, col].Value = p.sProductDescription;
                    col++;

                    worksheet.Cells[row, col].Value = p.iQuantity;
                    col++;

                    worksheet.Cells[row, col].Value = string.Format("{0}{1}", p.sCurrencySymbol, p.decPrice);
                    col++;

                    worksheet.Cells[row, col].Value = string.Format("{0}{1}", p.sCurrencySymbol, p.decTotalAmount);
                    col++;

                    if (p.sTrackingNumbers[0].Length > 1)
                    {
                        string sTrackingNumbers = "";

                        foreach (string s in p.sTrackingNumbers)
                        {
                            //sTrackingNumbers = sTrackingNumbers + string.Format("http://www.fedex.com/Tracking?action=track&language=english&cntry_code=us&initial=x&mps=y&tracknumbers={0} ", p.sTrackingNumbers);
                            //sTrackingNumbers = sTrackingNumbers + string.Format("{0} ", p.sTrackingNumbers);
                            sTrackingNumbers = sTrackingNumbers + string.Format("{0} ", p.sTrackingNumbers).Substring(string.Format("{0} ", p.sTrackingNumbers).LastIndexOf('=') + 1);
                        }
                        worksheet.Cells[row, col].Value = sTrackingNumbers;

                    }
                    else
                    {
                        if (p.sReceived != "")
                        {
                            worksheet.Cells[row, col].Value = "Received";
                        }
                        else
                        {
                            if (p.sShipping != "")
                            {
                                worksheet.Cells[row, col].Value = p.sShipping;
                            }
                        }
                    }
                    col++;

                    row++;
                }

                // save the new spreadsheet
                xlPackage.Save();
            }
        }
    }
}
