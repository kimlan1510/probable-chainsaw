using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
  [Collection("RecipeBox")]
  public class RatingTest : IDisposable
  {
    public RatingTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=recipe_box_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     //Arrange, Act
     int result = Rating.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
     //Arrange
    Rating testRating = new Rating("me", 5);

     //Act
     testRating.Save();
     List<Rating> result =Rating.GetAll();
     List<Rating> testList = new List<Rating>{testRating};

     //Assert
     Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindRatingInDatabase()
    {
      //Arrange
      Rating testRating = new Rating("Expandrew", 2);
      testRating.Save();

      //Act
      Rating foundRating = Rating.Find(testRating.GetId());

      //Assert
      Assert.Equal(testRating, foundRating);
    }

    [Fact]
    public void Test_Update_UpdatesRatingInDatabase()
    {
      //Arrange
      Rating testRating = new Rating("jordan", 1);
      testRating.Save();
      int newScore = 2;
      //Act
      testRating.Update("jordan", 2);
      int result =testRating.GetScore();

      //Assert
      Assert.Equal(newScore, result);
    }

    [Fact]
    public void Test_GetRecipes_GetRecipesThatHaveTheSameRatingScore()
    {
      //Arrange
      Rating testRating1 = new Rating("I", 5);
      testRating1.Save();

      Rating testRating2 = new Rating("you", 1);
      testRating2.Save();

      Recipe testRecipe1 = new Recipe("egg", "cook it");
      testRecipe1.Save();

      Recipe testRecipe2 = new Recipe("noodle", "boil it");
      testRecipe2.Save();
      //Act
      testRecipe1.AddRating(testRating1);
      testRecipe2.AddRating(testRating2);

      List<Recipe> resultRecipe = Rating.GetRecipeWithRating(1);
      List<Recipe> testRecipe = new List<Recipe>{testRecipe2};
      //Assert
      Assert.Equal(testRecipe, resultRecipe);
    }

    [Fact]
    public void Delete_DeletesRatingAssociationsFromDatabase_RatingList()
    {
      //Arrange
      Recipe testRecipe = new Recipe("cheesy sandwich", "cheese");
      testRecipe.Save();

      Rating testRating = new Rating("kl", 1);
      testRating.Save();

      //Act
      testRating.AddRatingToRecipe(testRecipe);
      testRating.Delete();

      List<Rating> resultRecipeRating = testRecipe.GetRating();
      List<Rating> testRecipeRating = new List<Rating> {};

      //Assert
      Assert.Equal(testRecipeRating, resultRecipeRating);
    }



    public void Dispose()
    {
      Ingredient.DeleteAll();
      Rating.DeleteAll();
      Recipe.DeleteAll();
    }



  }
}
