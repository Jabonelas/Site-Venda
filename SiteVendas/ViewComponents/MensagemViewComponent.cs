using AngleSharp.Dom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.ViewComponents;

public class MensagemViewComponent : ViewComponent
{
    private SiteVendasContext context = new SiteVendasContext();

    [HttpGet]
    [Authorize(Roles = "Admin@hotmail.com")]
    // [Authorize]
    public IViewComponentResult Invoke()
    {
        var mensagens = context.tb_mensagens.Where(x => x.mg_verificado == false).ToList();

        return View(mensagens);
    }
}