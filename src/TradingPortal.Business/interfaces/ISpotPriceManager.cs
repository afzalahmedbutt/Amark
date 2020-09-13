using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Business.interfaces
{
    public interface ISpotPriceManager
    {
        Task<SpotPricePreviewViewModel> SpotPricePreview(string VsWhich = "NY");
        Task<UpdateSpotsViewModel> UpdateSpots(DateTime LastDate, string VsWhich = "NY", string IsAfterHours = "NO");
    }
}
