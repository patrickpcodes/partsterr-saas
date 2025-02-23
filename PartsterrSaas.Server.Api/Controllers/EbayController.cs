using Microsoft.AspNetCore.Mvc;
using PartsterrSaas.Common;

namespace PartsterrSaas.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EbayController : ControllerBase
{
    private readonly ILogger<EbayController> _logger;
    private readonly IEbayApiManager _ebayApiManager;
    public EbayController(ILogger<EbayController> logger, IEbayApiManager ebayApiManager)
    {
        _logger = logger;
        _ebayApiManager = ebayApiManager;
    }

    [HttpGet( Name = "Ebay" )]
    public async Task<IEnumerable<PartsterrOrder>> GetOrders()
    {
        var orders = await _ebayApiManager.GetAllEbayOrdersAfterDate();
        return PartsterrOrder.ConvertToPartsterrOrders(orders.Take(10).ToList());

    }
}