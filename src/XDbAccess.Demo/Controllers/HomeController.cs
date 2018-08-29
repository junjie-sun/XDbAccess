using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XDbAccess.Demo.Models;
using XDbAccess.Demo.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace XDbAccess.Demo.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _UserService;

        private IOrderService _OrderService;

        private IProductService _ProductService;

        public HomeController(IUserService userService,
            IOrderService orderService,
            IProductService productService)
        {
            _UserService = userService;
            _OrderService = orderService;
            _ProductService = productService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        #region 新增演示

        public IActionResult InsertDemo()
        {
            return View(new User()
            {
                Name = "Jacky",
                Birthday = new DateTime(1996, 8, 3),
                Description = "演示用户"
            });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            await _UserService.AddUserAsync(user);

            string message = $"注册用户成功，用户ID={user.Id}";

            return View("Result", message);
        }

        #endregion

        #region 查询演示

        public async Task<IActionResult> QueryDemo(string name)
        {
            var list = await _UserService.AllUsersAsync(name);

            return View(list);
        }

        #endregion

        #region 修改演示

        public async Task<IActionResult> ModifyDemo(int id)
        {
            var user = await _UserService.GetUserAsync(id);

            if (user == null)
            {
                return View("Index");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyUser(User user)
        {
            var r = await _UserService.ModifyUserAsync(user);

            string message =r > 0 ? $"修改用户成功，用户ID={user.Id}" : $"修改用户失败，用户ID={user.Id}";

            return View("Result", message);
        }

        #endregion

        #region 删除演示

        [HttpPost]
        public async Task<IActionResult> RemoveUser(int id)
        {
            var r = await _UserService.RemoveUserAsync(id);

            if (r > 0)
            {
                return Json(new { status = 0, message = $"删除用户成功，用户ID={id}" });
            }

            return Json(new { status = -1, message = $"删除用户失败，用户ID={id}" });
        }

        #endregion

        #region 事务演示

        /// <summary>
        /// 单层事务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SingleTransDemo()
        {
            try
            {
                await _UserService.SingleTransDemoAsync();
                return Json(new { status = 0, message = "SingleTransDemo Success." });
            }
            catch(Exception ex)
            {
                return Json(new { status = -1, message = $"SingleTransDemo Fail. Detail:{ex.Message}" });
            }
        }

        /// <summary>
        /// 单层开启2个事务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SingleTwoTransDemo()
        {
            try
            {
                await _UserService.SingleTwoTransDemoAsync();
                return Json(new { status = 0, message = "SingleTwoTransDemo Success." });
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, message = $"SingleTwoTransDemo Fail. Detail:{ex.Message}" });
            }
        }

        /// <summary>
        /// 双层开启2个事务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DoubleTwoTransDemo()
        {
            try
            {
                await _UserService.DoubleTwoTransDemoAsync();
                return Json(new { status = 0, message = "DoubleTwoTransDemo Success." });
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, message = $"DoubleTwoTransDemo Fail. Detail:{ex.Message}" });
            }
        }

        /// <summary>
        /// 双层开启2个事务内层事务回滚
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DoubleTwoTransNestRollbackDemo()
        {
            try
            {
                await _UserService.DoubleTwoTransNestRollbackDemoAsync();
                return Json(new { status = 0, message = "DoubleTwoTransNestRollbackDemo Success." });
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, message = $"DoubleTwoTransNestRollbackDemo Fail. Detail:{ex.Message}" });
            }
        }

        /// <summary>
        /// 双层开启2个事务外层事务回滚
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DoubleTwoTransRootRollbackDemo()
        {
            try
            {
                await _UserService.DoubleTwoTransRootRollbackDemoAsync();
                return Json(new { status = 0, message = "DoubleTwoTransRootRollbackDemo Success." });
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, message = $"DoubleTwoTransRootRollbackDemo Fail. Detail:{ex.Message}" });
            }
        }

        #endregion

        #region 扩展接口演示

        [HttpPost]
        public async Task<IActionResult> RegisterUserEntity(User user)
        {
            await _UserService.AddUserEntityAsync(user);

            string message = $"注册用户成功，用户ID={user.Id}";

            return View("Result", message);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyEntityUser(User user)
        {
            var r = await _UserService.ModifyUserEntityAsync(user);

            string message = r > 0 ? $"修改用户成功，用户ID={user.Id}" : $"修改用户失败，用户ID={user.Id}";

            return View("Result", message);
        }

        public async Task<IActionResult> PagedQueryDemo(string name, int pageIndex = 0, int pageSize = 5, int total = 0)
        {
            var result = await _UserService.PagedQueryDemoAsync(name, pageIndex, pageSize);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = result.PageIndex == 0 ? result.Total : total;
            return View(result.Data);
        }

        #endregion

        #region 多库演示

        public async Task<IActionResult> DoubleDbDemo()
        {
            Product product1 = new Product()
            {
                Name = "产品1"
            };
            Product product2 = new Product()
            {
                Name = "产品2"
            };
            await _ProductService.AddProductAsync(product1);
            await _ProductService.AddProductAsync(product2);

            User user = new User()
            {
                Name = "用户1",
                Birthday = new DateTime(1990, 5, 10)
            };
            await _UserService.AddUserAsync(user);

            await _OrderService.AddOrderAsync(new Order()
            {
                UserId = user.Id,
                CreateTime = DateTime.Now,
                Total = 100
            }, new List<int>() { product1.Id, product2.Id });

            var orderList = await _OrderService.GetAllOrdersAsync();

            return View(orderList);
        }

        #endregion
    }
}
