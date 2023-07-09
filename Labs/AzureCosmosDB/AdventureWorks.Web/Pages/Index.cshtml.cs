using AdventureWorks.Context;
using AdventureWorks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorks.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IAdventureWorksProductContext _productContext;

    public IndexModel(IAdventureWorksProductContext productContext)
    {
        _productContext = productContext;
    }

    [BindProperty(SupportsGet = true)]
    public List<Model> Models { get; set; } = new List<Model>();

    public async Task OnGetAsync()
    {
        this.Models = await _productContext.GetModelsAsync();
    }
}
