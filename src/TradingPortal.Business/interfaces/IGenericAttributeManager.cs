using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingPortal.Core;
using TradingPortal.Core.Domain.Common;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Business.interfaces
{
    public interface IGenericAttributeManager1
    {
        /// <summary>
        /// Deletes an attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        void DeleteAttribute(GenericAttribute attribute);

        /// <summary>
        /// Gets an attribute
        /// </summary>
        /// <param name="attributeId">Attribute identifier</param>
        /// <returns>An attribute</returns>
        GenericAttribute GetAttributeById(int attributeId);

        /// <summary>
        /// Inserts an attribute
        /// </summary>
        /// <param name="attribute">attribute</param>
        Task InsertAttribute(GenericAttribute attribute);

        /// <summary>
        /// Updates the attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        Task UpdateAttribute(GenericAttribute attribute);

        /// <summary>
        /// Get attributes
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>Get attributes</returns>
        Task<IList<GenericAttribute>> GetAttributesForEntity(int entityId, string keyGroup);

        Task<CustomerAttributes> GetCustomerAttributes(int entityId);

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier; pass 0 if this attribute will be available for all stores</param>
        Task SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value, int storeId = 0);

        //bool SaveCustomerAttribute(int entityId, string key, string keyGroup, string value);
        Task<bool> SaveCustomerAttribute(int entityId, string key, string keyGroup, string value);

        string GetPasswordRecoveryToken(int entityId);
        Task SaveAttribute<TPropType>(IdentityUser<int> entity, string key, TPropType value, int storeId = 0);

        TPropType GetAttribute<TPropType>(BaseEntity entity,string key, int storeId = 0);
        TPropType GetAttribute<TPropType>(IdentityUser<int> entity, string key, int storeId = 0);
    }
}
