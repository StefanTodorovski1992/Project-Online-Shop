using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActiveOrdersController : Controller
    {
       private readonly IActiveOrdersRepository _activeOrdersRepo;
       public ActiveOrdersController(IActiveOrdersRepository activeOrdersRepo)
       {
           _activeOrdersRepo = activeOrdersRepo;
       }
       public async Task<IActionResult> ActiveOrders()
       {
           var orders = await _activeOrdersRepo.ActiveOrders();
           return View(orders);
       }
    }
}
