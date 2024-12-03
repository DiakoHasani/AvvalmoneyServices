using AS.Log;
using AS.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace AS.BL.Catches
{
    public class LifeLogBotWithdrawCatch : ILifeLogBotWithdrawCatch
    {
        private readonly ILogger _logger;
        private const string Key = "0bf471d5-3287-41bd-88d2-4c89502bb1c6";

        public LifeLogBotWithdrawCatch(ILogger logger)
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
                HttpContext.Current.Cache.Insert(Key, true, null, DateTime.Now.AddMinutes(ServiceKeys.ExpireCatchByMinuteLifeLogBot), Cache.NoSlidingExpiration);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }
    public interface ILifeLogBotWithdrawCatch
    {
        bool AccessToSend();
    }
}
