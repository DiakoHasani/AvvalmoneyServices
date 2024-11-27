using AS.Log;
using AS.Model.General;
using AS.Model.Novinpal;
using AS.Model.Paystar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace AS.BL.Catches
{
    public class NovinpalCatch : INovinpalCatch
    {
        private readonly ILogger _logger;

        public NovinpalCatch(ILogger logger)
        {
            _logger = logger;
        }

        public NovinpalCatchModel Add(NovinpalCatchModel model)
        {
            try
            {
                HttpContext.Current.Cache.Insert(model.OrderId, model, null, DateTime.Now.AddMinutes(ServiceKeys.ExpireCatchByMinute), Cache.NoSlidingExpiration);
                return model;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public NovinpalCatchModel Get(string key)
        {
            try
            {
                return (NovinpalCatchModel)HttpContext.Current.Cache[key];
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface INovinpalCatch
    {
        NovinpalCatchModel Add(NovinpalCatchModel model);
        NovinpalCatchModel Get(string key);
    }
}
