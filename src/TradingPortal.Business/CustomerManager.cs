using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using MTSWebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Constants;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.Domain.Shipping;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure;
using TradingPortal.Infrastructure.DatabaseContexts;
using TradingPortal.Infrastructure.Services.Interfaces;
using WindowsServiceEndPoint;

namespace TradingPortal.Business
{
    public class CustomerManager  :ICustomerManager
    {
        //private readonly UserManager<Customer> _userManager;
        //private readonly IRepository<Customer> _customerRepository;
        private readonly IStoreCommandRepository _storeCommandRepository;
        private readonly IGenericAttributeManager _genericAttributeManager;
        private readonly ApplicationDbContext _context;
        //private readonly ICurrentUser _currentUser;
        private readonly IConfiguration _config;
        //private DbCommand cmd;
        private MTS_Api _mtsApi = null;

        public CustomerManager(ApplicationDbContext context, IGenericAttributeManager genericAttributeManager,IConfiguration config, IStoreCommandRepository storeCommandRepository)
        {
            _context = context;
            _genericAttributeManager = genericAttributeManager;
            _config = config;
            _storeCommandRepository = storeCommandRepository;
        }

        

        ///// <summary>
        ///// Reset data required for checkout
        ///// </summary>
        ///// <param name="customer">Customer</param>
        ///// <param name="storeId">Store identifier</param>
        ///// <param name="clearCouponCodes">A value indicating whether to clear coupon code</param>
        ///// <param name="clearCheckoutAttributes">A value indicating whether to clear selected checkout attributes</param>
        ///// <param name="clearRewardPoints">A value indicating whether to clear "Use reward points" flag</param>
        ///// <param name="clearShippingMethod">A value indicating whether to clear selected shipping method</param>
        ///// <param name="clearPaymentMethod">A value indicating whether to clear selected payment method</param>
        public virtual async Task ResetCheckoutData(Customer customer, int storeId,
            bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
            bool clearRewardPoints = true, bool clearShippingMethod = true,
            bool clearPaymentMethod = true)
        {
            if (customer == null)
                throw new ArgumentNullException();

            //clear entered coupon codes
            if (clearCouponCodes)
            {
                await _genericAttributeManager.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.DiscountCouponCode, null);
                await _genericAttributeManager.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.GiftCardCouponCodes, null);
            }

            //clear checkout attributes
            if (clearCheckoutAttributes)
            {
                await _genericAttributeManager.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.CheckoutAttributes, null);
            }

            //clear reward points flag
            if (clearRewardPoints)
            {
                await _genericAttributeManager.SaveAttribute<bool>(customer, SystemCustomerAttributeNames.UseRewardPointsDuringCheckout, false, storeId);
            }

            //clear selected shipping method
            if (clearShippingMethod)
            {
                await _genericAttributeManager.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.SelectedShippingOption, null, storeId);
                await _genericAttributeManager.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.OfferedShippingOptions, null, storeId);
            }

            //clear selected payment method
            if (clearPaymentMethod)
            {
                await _genericAttributeManager.SaveAttribute<string>(customer, SystemCustomerAttributeNames.SelectedPaymentMethod, null, storeId);
            }

            UpdateCustomer(customer);
        }

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //_context.SaveChanges();
            //await _userManager.UpdateAsync(customer);
            
                       
        }


        public async Task UpdatePortalProducts(Customer user)
        {
            try
            {
                CustomerAttributes customerAttributes = _genericAttributeManager.GetCustomerAttributes(user.Id).Result;
                var serviceEndPoint = _config.GetSection("ConnectionStrings:WebAPI_URL").Value;
                _mtsApi = new WindowsServiceEndPoint.MTS_Api(serviceEndPoint, customerAttributes.AmarkTPAPIKey, customerAttributes.AmarkTradingPartnerNumber, user.Email);
                var productInfo = await _mtsApi.GetPortalProducts();
                if (productInfo.objProductList[0].sRequestStatus != "Declined")
                {
                    var productsList = GetProductsList(productInfo.objProductList);
                    

                    UpdateProducts(productsList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
            


        public void UpdateProducts(List<SqlDataRecord> productMetaDataList)
        {
            Task.Run(() =>
            {
                var result = _storeCommandRepository.CreateStoreProcedureCommand("[dbo].[spUpdateAllPortalProducts]")
                        .AddParameter("@PortalProducts", productMetaDataList, SqlDbType.Structured, "UT_PortalProduct")
                        //.AddParameter("@returnVal",null,SqlDbType.Int,ParameterDirection.Output)
                        .ExecuteNonQuery();
            });
        }

        private List<SqlDataRecord> GetProductsList(ProductItem[] productsList)
        {
            List<SqlDataRecord> datatable = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaData = new SqlMetaData[4];
            sqlMetaData[0] = new SqlMetaData("Name", SqlDbType.NVarChar,2000);
            sqlMetaData[1] = new SqlMetaData("ShortDescription", SqlDbType.NVarChar,2000);
            sqlMetaData[2] = new SqlMetaData("Weight", SqlDbType.Decimal);
            sqlMetaData[3] = new SqlMetaData("CommodityID", SqlDbType.Int);
            SqlDataRecord row = new SqlDataRecord(sqlMetaData);

            foreach (var product in productsList)
            {
                row = new SqlDataRecord(sqlMetaData);
                row.SetValues(new object[] { product.sProductCode, product.sProductDesc, product.decProductOzconv, product.iCommodityCode });
                datatable.Add(row);
            }
            return datatable;
        }





        //public MTS_Api MtsApi
        //{
        //    get
        //    {
        //        if (_mtsApi == null)
        //        {
        //            CustomerAttributes customerAttributes = _genericAttributeManager.GetCustomerAttributes(_currentUser.User.Id).Result;
        //            var serviceEndPoint = _config.GetSection("ConnectionStrings:WebAPI_URL").Value;
        //            _mtsApi = new WindowsServiceEndPoint.MTS_Api(serviceEndPoint, customerAttributes.AmarkTPAPIKey, customerAttributes.AmarkTradingPartnerNumber, _currentUser.User.Email);
        //            return _mtsApi;
        //        }
        //        else
        //        {
        //            return _mtsApi;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        //public virtual Customer GetCustomerById(int customerId)
        //{
        //    if (customerId == 0)
        //        return null;

        //    return _userManager.GetUserAsync(customerId);
        //}


    }
}
