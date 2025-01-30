using AS.Log;
using AS.Model.General;
using AS.Model.Paystar;
using AS.Model.Sepal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace AS.BL.Catches
{
    public class SepalCatch : ISepalCatch
    {
        private readonly ILogger _logger;
        public SepalCatch(ILogger logger)
        {
            _logger = logger;
        }

        public SepalCatchModel Add(SepalCatchModel model)
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

        public SepalCatchModel Get(string key)
        {
            try
            {
                return (SepalCatchModel)HttpContext.Current.Cache[key];
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface ISepalCatch
    {
        SepalCatchModel Add(SepalCatchModel model);
        SepalCatchModel Get(string key);
    }
}
