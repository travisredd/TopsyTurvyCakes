using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages.Admin
{
    public class AddEditRecipeModel : PageModel
    {
        [FromRoute]
        public long? Id { get; set; }
        //determine if this is a new recipe
        public bool IsNewRecipe
        {
            get { return Id == null; }
        }
        [BindProperty]
        //instantiate a new copy of the recipe - create a new recipe
        public Recipe Recipe { get; set; }

        public IRecipesService recipesService { get; set; }
        
        //has connection to the recipeService
        public AddEditRecipeModel(IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }

        //this is an HttpGet -  to fill out the form
        [HttpGet]
        public async Task OnGetAsync()
        {
            //if null use right side, if not null use the left side.
            Recipe = recipesService.Find(Id.GetValueOrDefault()) ?? new Recipe();
        }

        //this is an HttpPost -  return async tast of IActionResult
        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Recipe.Id = Id.GetValueOrDefault();
            await recipesService.SaveAsync(Recipe);
            //call the recipe page
            return RedirectToPage("/Recipe", new { id = Recipe.Id });
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }
    }
}