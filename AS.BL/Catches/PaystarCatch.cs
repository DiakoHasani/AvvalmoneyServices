using AS.Log;
using AS.Model.General;
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
    public class PaystarCatch : IPaystarCatch
    {
        private readonly ILogger _logger;
        public PaystarCatch(ILogger logger)
        {
            _logger = logger;
        }

        public PaystarCatchModel Add(PaystarCatchModel model)
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

        public PaystarCatchModel Get(string key)
        {
            try
            {
                return (PaystarCatchModel) HttpContext.Current.Cache[key];
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface IPaystarCatch
    {
        PaystarCatchModel Add(PaystarCatchModel model);
        PaystarCatchModel Get(string key);
    }
}
