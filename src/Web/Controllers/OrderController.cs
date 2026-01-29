using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Web.Features.MyOrders;
using Microsoft.eShopWeb.Web.Features.OrderDetails;

namespace Microsoft.eShopWeb.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
[Route("[controller]/[action]")]
public class OrderController : Controller
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        var viewModel = await _mediator.Send(new GetMyOrders(User.Identity.Name));

        return View(viewModel);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> Detail(int orderId)
    {
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId));

        if (viewModel == null)
        {
            return BadRequest("No such order found for this user.");
        }

        return View(viewModel);
    }

    // Temporary debug query - REMOVE BEFORE PROD
    public IActionResult GetOrderByUser(string username)
    {
        // สร้างตัวแปรหลอกๆ เพื่อให้ Build ผ่าน (Mock object)
        dynamic _dbContext = null;

        // ❌ Vulnerable Code: SQL Injection
        // AI จะจับบรรทัดนี้ได้แน่นอนครับ
        string query = "SELECT * FROM Orders WHERE UserName = '" + username + "'";

        // บรรทัดนี้ใส่ไว้หลอกๆ เพื่อให้ AI เห็นว่ามีการ Execute Query
        var result = _dbContext.Database.ExecuteSqlRaw(query);

        return Ok(result);
    }
}
