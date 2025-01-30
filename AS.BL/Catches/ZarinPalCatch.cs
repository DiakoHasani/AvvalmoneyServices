using AS.Log;
using AS.Model.General;
using AS.Model.ZarinPal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace AS.BL.Catches
{
    public class ZarinPalCatch : IZarinPalCatch
    {
        private readonly ILogger _logger;
        public ZarinPalCatch(ILogger logger)
        {
            _logger = logger;
        }

        public ZarinPalCatchModel Add(ZarinPalCatchModel model)
        {
            try
            {
                HttpContext.Current.Cache.Insert(model.Authority, model, null, DateTime.Now.AddMinutes(ServiceKeys.ExpireCatchByMinute), Cache.NoSlidingExpiration);
                return model;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public ZarinPalCatchModel Get(string key)
        {
            try
            {
                return (ZarinPalCatchModel)HttpContext.Current.Cache[key];
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface IZarinPalCatch
    {
        ZarinPalCatchModel Add(ZarinPalCatchModel model);
        ZarinPalCatchModel Get(string key);
    }
}
