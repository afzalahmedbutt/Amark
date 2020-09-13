using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain.Identity;

namespace TradingPortal.Business.interfaces
{
    public interface ICustomerManager
    {
        Task ResetCheckoutData(Customer customer, int storeId,
            bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
            bool clearRewardPoints = true, bool clearShippingMethod = true,
            bool clearPaymentMethod = true);
        void UpdateCustomer(Customer customer);
        Task UpdatePortalProducts(Customer user);
    }
}
