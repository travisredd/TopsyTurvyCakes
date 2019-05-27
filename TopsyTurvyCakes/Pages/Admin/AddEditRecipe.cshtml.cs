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

        [BindProperty]
        public Microsoft.AspNetCore.Http.IFormFile newImage { get; set; }

        [BindProperty]
        public Microsoft.AspNetCore.Http.IFormFile Image { get; set; }

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
            //Recipe.Id = Id.GetValueOrDefault();
            var newRecipe = recipesService.Find(Id.GetValueOrDefault()) ?? new Recipe();
            newRecipe.Description = Recipe.Description;
            newRecipe.Directions = Recipe.Directions;
            newRecipe.Ingredients = Recipe.Ingredients;
            newRecipe.SetImage(Image);

            await recipesService.SaveAsync(newRecipe);
            //call the recipe page
            return RedirectToPage("/Recipe", new { id = newRecipe.Id });
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }
    }
}