using AS.Log;
using AS.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace AS.BL.Catches
{
    public class CryptoWithdrawCatch : ICryptoWithdrawCatch
    {
        private readonly ILogger _logger;
        private const string Key = "f3e91e09-1ec6-4895-86a1-243989829f8c";
        public CryptoWithdrawCatch(ILogger logger)
        {
            _logger = logger;
        }

        public bool AccessToSend()
        {
            try
            {
                if (HttpContext.Current.Cache[Key] != null)
                {
                    return false;
                }
                HttpContext.Current.Cache.Insert(Key, true, null, DateTime.Now.AddMinutes(ServiceKeys.ExpireCatchByMinuteCryptoWithdraw), Cache.NoSlidingExpiration);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }

    public interface ICryptoWithdrawCatch
    {
        bool AccessToSend();
    }
}
