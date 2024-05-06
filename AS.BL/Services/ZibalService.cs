using AS.Model.Zibal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class ZibalService : IZibalService
    {
        public async Task<ZibalCheckoutModel> CheckOut(ZibalCheckoutModel model)
        {
            return model;
        }

        public async Task<long> GetBalance()
        {
            return 100000;
        }
    }
    public interface IZibalService
    {
        Task<long> GetBalance();
        Task<ZibalCheckoutModel> CheckOut(ZibalCheckoutModel model);
    }
}
