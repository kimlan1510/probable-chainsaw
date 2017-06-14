using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
  [Collection("RecipeBox")]
  public class IngredientTEst : IDisposable
  {
    public IngredientTEst()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=recipe_box_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     //Arrange, Act
     int result = Ingredient.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }


    [Fact]
    public void Test_Save_SavesToDatabase()
    {
     //Arrange
    Ingredient testIngredient = new Ingredient("tomato");

     //Act
     testIngredient.Save();
     List<Ingredient> result =Ingredient.GetAll();
     List<Ingredient> testList = new List<Ingredient>{testIngredient};

     //Assert
     Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindIngredientInDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Expandrew");
      testIngredient.Save();

      //Act
      Ingredient foundIngredient = Ingredient.Find(testIngredient.GetId());

      //Assert
      Assert.Equal(testIngredient, foundIngredient);
    }

    [Fact]
    public void Test_Update_UpdatesIngredientInDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("garlic");
      testIngredient.Save();
      string newIngredient = "tomato";
      //Act
      testIngredient.Update("tomato");
      string result =testIngredient.GetName();

      //Assert
      Assert.Equal(newIngredient, result);
    }


    public void Dispose()
    {
      Ingredient.DeleteAll();


    }


  }
}
