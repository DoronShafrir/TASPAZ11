using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TASHPAV11.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
        //Ai PArt - not Active yet

        private readonly AIService _aiService;
        C#
public CoursesModel(AIService aiService)
        {
            _aiService = aiService;
        }
        public async Task<IActionResult> OnPostExplainAsync()
        {
            string answer = await _aiService.AskAsync(
            "Explain Newton's second law to a high school student."
            );
            return Content(answer);
        }
    }
}
