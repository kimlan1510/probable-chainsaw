using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
  [Collection("RecipeBox")]
  public class RecipeTest : IDisposable
  {
    public RecipeTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=recipe_box_test; Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_RecipeEmptyAtFirst()
    {
      //Arrange, Act
      int result = Recipe.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_SaveRecipeToDatabase()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Chicken Lasagna", "make lasagna");
      testRecipe.Save();

      //Act
      List<Recipe> result = Recipe.GetAll();
      List<Recipe> testList = new List<Recipe>{testRecipe};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsRecipeInDatabase()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Turkey Lasagna", "throw away chicken lasagna");
      testRecipe.Save();
      //Act
      Recipe foundRecipe = Recipe.Find(testRecipe.GetId());
      //Assert
      Assert.Equal(testRecipe, foundRecipe);
    }

    [Fact]
    public void Test_Update_UpdatesRecipeInDatabase()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Turkey and Jelly Sandwich", "put on bread");
      testRecipe.Save();
      string newName = "Dumpster food";
      //Act
      testRecipe.Update("Dumpster food", "put on bread");
      string result =testRecipe.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_AddIngredient_AddsIngredientToRecipe()
    {
      //Arrange
      Recipe testRecipe = new Recipe("cheesy grits", "eat them up");
      testRecipe.Save();

      Ingredient testIngredient = new Ingredient("hominy");
      testIngredient.Save();

      Ingredient testIngredient2 = new Ingredient("polenta");
      testIngredient2.Save();

      //Act
      testRecipe.AddIngredient(testIngredient);
      testRecipe.AddIngredient(testIngredient2);

      List<Ingredient> result = testRecipe.GetIngredient();
      List<Ingredient> testList = new List<Ingredient>{testIngredient, testIngredient2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesRecipeAssociationsFromDatabase_RecipeList()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("cheese");
      testIngredient.Save();

      Recipe testRecipe = new Recipe("cheesy grits", "eat em up");
      testRecipe.Save();

      //Act
      testRecipe.AddIngredient(testIngredient);
      testRecipe.Delete();

      List<Recipe> resultIngredientRecipe = testIngredient.GetRecipe();
      List<Recipe> testIngredientRecipe = new List<Recipe> {};

      //Assert
      Assert.Equal(testIngredientRecipe, resultIngredientRecipe);
    }

    public void Dispose()
    {
      Ingredient.DeleteAll();
      Recipe.DeleteAll();
    }
  }
}
