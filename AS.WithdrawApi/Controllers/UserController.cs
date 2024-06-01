using AS.BL.Services;
using AS.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AS.WithdrawApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        public UserController(ILogger logger,
            IUserService userService, ITransactionService transactionService)
        {
            _logger = logger;
            _userService = userService;
            _transactionService = transactionService;
        }
    }
}
