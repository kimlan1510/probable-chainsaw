using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System;

namespace RecipeBox
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Categories> AllCategories = Categories.GetAll();
        return View ["index.cshtml", AllCategories];
      };
      Get["/newCategory"] = _ => {
        return View["Category_form.cshtml"];
      };
      Get["/categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedCategory = Categories.Find(parameters.id);
        var allRecipes = selectedCategory.GetRecipe();
        model.Add("selectedCategory", selectedCategory);
        model.Add("allRecipes", allRecipes);
        return View["category.cshtml", model];
      };
      Post["/newCategory"] = _ => {
        Categories newCategory = new Categories(Request.Form["name"]);
        newCategory.Save();
        List<Categories> AllCategories = Categories.GetAll();
        return View["index.cshtml", AllCategories];
      };
      Get["/recipes"] = _ => {
        List<Recipe> AllRecipes = Recipe.GetAll();
        return View["recipes.cshtml", AllRecipes];
      };


      Get["/recipes/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedRecipe = Recipe.Find(parameters.id);
        var recipeIngredients = selectedRecipe.GetIngredient();
        var recipeCategory = Recipe.GetCategories();
        model.Add("recipeIngredients", recipeIngredients);
        model.Add("recipeCategory", recipeCategory);
        model.Add("selectedRecipe", selectedRecipe);
        return View["recipe.cshtml", model];
      };


      Get["/recipes/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Ingredient> AllIngredients = Ingredient.GetAll();
        List<Categories> AllCategories = Categories.GetAll();
        model.Add("categories", AllCategories);
        model.Add("ingredients", AllIngredients);
        return View["recipe_form.cshtml", model];
      };
      Post["/recipes/new"] = _ => {
        Recipe newRecipe = new Recipe(Request.Form["name"], Request.Form["instructions"]);
        newRecipe.Save();
        List<Recipe> AllRecipes = Recipe.GetAll();
        string ingredients = Request.Form["ingredient"];
        string[] ingredientsArray = ingredients.Split(',');
        foreach(string ingredientId in ingredientsArray)
        {
          Ingredient foundIngredient = Ingredient.Find(int.Parse(ingredientId));
          newRecipe.AddIngredient(foundIngredient);
        }
        string category_id = Request.Form["category_id"];
        Categories foundCategory = Categories.Find(int.Parse(category_id));
        foundCategory.AddRecipe(newRecipe);
        return View["recipes.cshtml", AllRecipes];
      };
      Get["/ingredients/new"] = _ => {
        return View ["ingredient_form.cshtml"];
      };
      Post["/ingredients/new"] = _ => {
        Ingredient newIngredient = new Ingredient(Request.Form["name"]);
        newIngredient.Save();
        List<Recipe> AllRecipes = Recipe.GetAll();
        return View ["recipes.cshtml", AllRecipes];
      };


    }
  }
}
