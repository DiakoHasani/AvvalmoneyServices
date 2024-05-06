using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class MenuNotificationRepository : BaseRepository<MenuNotification>, IMenuNotificationRepository
    {
        public MenuNotificationRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IMenuNotificationRepository : IBaseRepository<MenuNotification>
    {
    }
}
