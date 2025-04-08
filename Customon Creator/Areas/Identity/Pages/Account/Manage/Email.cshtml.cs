using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class EmailModel : PageModel {
    public IActionResult OnGet() {
        return RedirectToPage("./Index");
    }

    public IActionResult OnPost() {
        return RedirectToPage("./Index");
    }
}