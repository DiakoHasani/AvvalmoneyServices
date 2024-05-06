using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class MenuNotificationService : IMenuNotificationService
    {
        private readonly IMenuNotificationRepository _menuNotificationRepository;
        public MenuNotificationService(IMenuNotificationRepository menuNotificationRepository)
        {
            _menuNotificationRepository = menuNotificationRepository;
        }

        public async Task<bool> PushTransaction()
        {
            var menu = _menuNotificationRepository.GetAll().FirstOrDefault();
            menu.Men_Transaction++;
            _menuNotificationRepository.Update(menu);
            return await _menuNotificationRepository.SaveChangeAsync() > 0;
        }
    }
    public interface IMenuNotificationService
    {
        Task<bool> PushTransaction();
    }
}
