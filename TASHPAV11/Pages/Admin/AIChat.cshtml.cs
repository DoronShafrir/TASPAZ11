using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TASHPAV11.Pages.Admin
{
    public class AIChatModel : PageModel
    {
        private readonly AIService _aiService;

        public AIChatModel(AIService aiService)
        {
            _aiService = aiService;
        }

        [BindProperty]
        public string Question { get; set; } = "";

        public string Answer { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Question))
            {
                ModelState.AddModelError(
                    nameof(Question), "Please enter a question.");

                return Page();
            }

            Answer = await _aiService.AskAsync(Question);

            return Page();
        }
    }

}

