using System;
using System.Collections.Generic;
using System.Text;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using System.Linq;
using TradingPortal.Business.interfaces;

namespace TradingPortal.Business
{
    public class CheckoutAttributeManager : ICheckoutAttributeManager
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CHECKOUTATTRIBUTES_ALL_KEY = "Nop.checkoutattribute.all";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute ID
        /// </remarks>
        private const string CHECKOUTATTRIBUTES_BY_ID_KEY = "Nop.checkoutattribute.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute ID
        /// </remarks>
        private const string CHECKOUTATTRIBUTEVALUES_ALL_KEY = "Nop.checkoutattributevalue.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute value ID
        /// </remarks>
        private const string CHECKOUTATTRIBUTEVALUES_BY_ID_KEY = "Nop.checkoutattributevalue.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CHECKOUTATTRIBUTES_PATTERN_KEY = "Nop.checkoutattribute.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CHECKOUTATTRIBUTEVALUES_PATTERN_KEY = "Nop.checkoutattributevalue.";
        #endregion

        #region Fields

        private readonly IRepository<CheckoutAttribute> _checkoutAttributeRepository;
        private readonly IRepository<CheckoutAttributeValue> _checkoutAttributeValueRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="checkoutAttributeRepository">Checkout attribute repository</param>
        /// <param name="checkoutAttributeValueRepository">Checkout attribute value repository</param>
        /// <param name="eventPublisher">Event published</param>
        public CheckoutAttributeManager(
            IRepository<CheckoutAttribute> checkoutAttributeRepository,
            IRepository<CheckoutAttributeValue> checkoutAttributeValueRepository
            )
        {
            _checkoutAttributeRepository = checkoutAttributeRepository;
            _checkoutAttributeValueRepository = checkoutAttributeValueRepository;

        }

        #endregion

        //#region Methods

        #region Checkout attributes

        /// <summary>
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void DeleteCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Delete(checkoutAttribute);

            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            //_eventPublisher.EntityDeleted(checkoutAttribute);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <returns>Checkout attribute collection</returns>
        public virtual IList<CheckoutAttribute> GetAllCheckoutAttributes()
        {
            var query = from ca in _checkoutAttributeRepository.GetAll()
                        orderby ca.DisplayOrder
                        select ca;
            var checkoutAttributes = query.ToList();
            return checkoutAttributes;
            //return _cacheManager.Get(CHECKOUTATTRIBUTES_ALL_KEY, () =>
            //{
            //    var query = from ca in _checkoutAttributeRepository.Table
            //                orderby ca.DisplayOrder
            //                select ca;
            //    var checkoutAttributes = query.ToList();
            //    return checkoutAttributes;
            //});
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <returns>Checkout attribute</returns>
        public virtual CheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId)
        {
            if (checkoutAttributeId == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTES_BY_ID_KEY, checkoutAttributeId);
            return _checkoutAttributeRepository.GetById(checkoutAttributeId);
            //return _cacheManager.Get(key, () => { return _checkoutAttributeRepository.GetById(checkoutAttributeId); });
        }

        /// <summary>
        /// Inserts a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void InsertCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Add(checkoutAttribute);

            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            //_eventPublisher.EntityInserted(checkoutAttribute);
        }

        /// <summary>
        /// Updates the checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void UpdateCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Update(checkoutAttribute, checkoutAttribute.Id);

            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            //_eventPublisher.EntityUpdated(checkoutAttribute);
        }

        /// <summary>
        /// Updates the checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void UpdateCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException("checkoutAttributeValue");

            _checkoutAttributeValueRepository.Update(checkoutAttributeValue, checkoutAttributeValue.Id);

            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            //_eventPublisher.EntityUpdated(checkoutAttributeValue);
        }

        /// <summary>
        /// Inserts a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void InsertCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException("checkoutAttributeValue");

            _checkoutAttributeValueRepository.Update(checkoutAttributeValue, checkoutAttributeValue.Id);

            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            //_cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);

            ////event notification
            //_eventPublisher.EntityInserted(checkoutAttributeValue);
        }

        public void DeleteCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            throw new NotImplementedException();
        }

        public IList<CheckoutAttributeValue> GetCheckoutAttributeValues(int checkoutAttributeId)
        {
            throw new NotImplementedException();
        }

        public CheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId)
        {
            string key = string.Format(CHECKOUTATTRIBUTEVALUES_BY_ID_KEY, checkoutAttributeValueId);
            return _checkoutAttributeValueRepository.Find(key);
        }


        #endregion

        //#region Checkout variant attribute values

    }
}

