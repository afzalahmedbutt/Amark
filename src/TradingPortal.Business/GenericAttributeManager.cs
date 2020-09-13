using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Constants;
using TradingPortal.Core.Domain.Common;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Helpers;

namespace TradingPortal.Business
{
    class GenericAttributeManager1 : IGenericAttributeManager1
    {
        private readonly IRepository<GenericAttribute> _genericAttributeRepository;
        public GenericAttributeManager1(IRepository<GenericAttribute> genericAttributeRepository)
        {
            _genericAttributeRepository = genericAttributeRepository;
        }

        public void DeleteAttribute(GenericAttribute attribute)
        {
            throw new NotImplementedException();
        }

        public GenericAttribute GetAttributeById(int attributeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<GenericAttribute>> GetAttributesForEntity(int entityId, string keyGroup)
        {
            var attributes = await _genericAttributeRepository.FindByAsyn(ga => ga.EntityId == entityId && ga.KeyGroup == keyGroup);
            return attributes.ToList();
        }

        

        public async Task<bool> SaveCustomerAttribute(int entityId, string key, string keyGroup, string value)
        {
            var existingAttribute = _genericAttributeRepository.Find(attr => attr.EntityId == entityId && attr.Key == key);
            if (existingAttribute == null)
            {
                var attribute = new GenericAttribute
                {
                    EntityId = entityId,
                    Key = key,
                    KeyGroup = keyGroup,
                    Value = value
                };
                _genericAttributeRepository.Add(attribute);
            }
            else
            {
                existingAttribute.Value = value;
                _genericAttributeRepository.Update(existingAttribute, entityId);
            }
            await _genericAttributeRepository.SaveAsync();
            return true;
        }



        public async Task<CustomerAttributes> GetCustomerAttributes(int entityId)
        {
            var attributes = await GetAttributesForEntity(entityId, "Customer");
            var customerAttributes = new CustomerAttributes
            {
                AmarkTPAPIKey = attributes.FirstOrDefault(a => a.Key == CustomerAttributeKey.AMARK_TPAPI_KEY)?.Value,
                AmarkTradingPartnerNumber = attributes.FirstOrDefault(a => a.Key == CustomerAttributeKey.AMARK_TRADING_PARTNER_NUMBER)?.Value,
                FirstName = attributes.FirstOrDefault(a => a.Key == CustomerAttributeKey.FIRST_NAME)?.Value,
                LastName = attributes.FirstOrDefault(a => a.Key == CustomerAttributeKey.LASTNAME)?.Value,
                Company = attributes.FirstOrDefault(a => a.Key == CustomerAttributeKey.COMPANY)?.Value,
                UseRewardPointsDuringCheckout = attributes.FirstOrDefault(a => a.Key == CustomerAttributeKey.USE_REWARD_POINTS_DURING_CHECKOUT)?.Value
            };
            return customerAttributes;
        }

        public string GetPasswordRecoveryToken(int entityId)
        {
            var attribute = _genericAttributeRepository.Find(attr => attr.EntityId == entityId && attr.Key == "PasswordRecoveryToken");
            if (attribute != null)
            {
                return attribute.Value;
            }
            else
            {
                return String.Empty;
            }
        }


        public async Task InsertAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");
            await _genericAttributeRepository.AddAsyn(attribute);
        }

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier; pass 0 if this attribute will be available for all stores</param>
        public virtual async Task SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value, int storeId = 0)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            //var keyGroup = entity.GetType().Name;
            string keyGroup = GetUnproxiedEntityType(entity).Name;
            
            var props = await GetAttributesForEntity(entity.Id, keyGroup);
            props =  props.Where(x => x.StoreId == storeId)
                .ToList();
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            string valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    DeleteAttribute(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    await UpdateAttribute(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    //insert
                    prop = new GenericAttribute()
                    {
                        EntityId = entity.Id,
                        Key = key,
                        KeyGroup = keyGroup,
                        Value = valueStr,
                        StoreId = storeId,

                    };
                    await InsertAttribute(prop);
                }
            }
        }

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier; pass 0 if this attribute will be available for all stores</param>
        public virtual async Task SaveAttribute<TPropType>(IdentityUser<int> entity, string key, TPropType value, int storeId = 0)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            //var keyGroup = entity.GetType().Name;
            string keyGroup = GetUnproxiedEntityType(entity).Name;

            var props = await GetAttributesForEntity(entity.Id, keyGroup);
            props = props.Where(x => x.StoreId == storeId)
                .ToList();
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            string valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    DeleteAttribute(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    await UpdateAttribute(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    //insert
                    prop = new GenericAttribute()
                    {
                        EntityId = entity.Id,
                        Key = key,
                        KeyGroup = keyGroup,
                        Value = valueStr,
                        StoreId = storeId,

                    };
                    await InsertAttribute(prop);
                }
            }
        }

        public async Task<TPropType> GetAttribute<TPropType>(BaseEntity entity, string key, int storeId = 0)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            string keyGroup = GetUnproxiedEntityType(entity).Name;
            var props = await GetAttributesForEntity(entity.Id, keyGroup);
            if (props == null)
                return default(TPropType);
            props = props.Where(x => x.StoreId == storeId).ToList();
            if (props.Count == 0)
                return default(TPropType);

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(prop.Value);

        }

        public async Task<TPropType> GetAttribute<TPropType>(IdentityUser<int> entity, string key, int storeId = 0)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            string keyGroup = GetUnproxiedEntityType(entity).Name;
            var props = await GetAttributesForEntity(entity.Id, keyGroup);
            if (props == null)
                return default(TPropType);
            props = props.Where(x => x.StoreId == storeId).ToList();
            if (props.Count == 0)
                return default(TPropType);

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(prop.Value);

        }


        Type GetUnproxiedEntityType(BaseEntity entity)
        {
            return _genericAttributeRepository.GetEntityClrType(entity.GetType());
        }

        Type GetUnproxiedEntityType(IdentityUser<int> entity)
        {
            return _genericAttributeRepository.GetEntityClrType(entity.GetType());
        }

        public async Task UpdateAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            await _genericAttributeRepository.UpdateAsyn(attribute,attribute.Id);

            //cache
            //_cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);
        }

        TPropType IGenericAttributeManager1.GetAttribute<TPropType>(BaseEntity entity, string key, int storeId)
        {
            throw new NotImplementedException();
        }

        TPropType IGenericAttributeManager1.GetAttribute<TPropType>(IdentityUser<int> entity, string key, int storeId)
        {
            throw new NotImplementedException();
        }
    }
}
