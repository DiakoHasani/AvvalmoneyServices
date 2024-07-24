using AS.Log;
using AS.Model.UpdatePrice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.UpdatePrice
{
    public class Statics : IStatics
    {
        private readonly ILogger _logger;

        string address, text = "";
        public Statics(ILogger logger)
        {
            _logger = logger;
        }

        public StaticModel GetStatics()
        {
            try
            {
                address = Path.Combine("App_Data/Statics.json");
                text = File.ReadAllText(address);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<StaticModel>(text);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface IStatics
    {
        StaticModel GetStatics();
    }
}
