using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
  [Collection("RecipeBox")]
  public class CategoriesTest : IDisposable
  {
    public CategoriesTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=recipe_box_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     //Arrange, Act
     int result = Categories.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
     //Arrange
    Categories testCategories = new Categories("italian");

     //Act
     testCategories.Save();
     List<Categories> result =Categories.GetAll();
     List<Categories> testList = new List<Categories>{testCategories};

     //Assert
     Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindCategoriesInDatabase()
    {
      //Arrange
      Categories testCategories = new Categories("italian");
      testCategories.Save();

      //Act
      Categories foundCategories = Categories.Find(testCategories.GetId());

      //Assert
      Assert.Equal(testCategories, foundCategories);
    }

    [Fact]
    public void Test_Update_UpdatesCategoriesInDatabase()
    {
      //Arrange
      Categories testCategories = new Categories("italian");
      testCategories.Save();
      string newCategories = "meditarian";
      //Act
      testCategories.Update("meditarian");
      string result =testCategories.GetName();

      //Assert
      Assert.Equal(newCategories, result);
    }

    [Fact]
    public void Test_GetRecipe_GetRecipesThatIsInTheSameCategory()
    {
      //Arrange
      Recipe testRecipe1 = new Recipe("egg", "cook it");
      testRecipe1.Save();

      Recipe testRecipe2 = new Recipe("noodle", "boil it");
      testRecipe2.Save();

      Categories testCategories = new Categories("italian");
      testCategories.Save();

      //Act
      testRecipe1.AddCategories(testCategories);
      testRecipe2.AddCategories(testCategories);

      List<Recipe> resultRecipe = testCategories.GetRecipe();
      List<Recipe> testRecipe = new List<Recipe>{testRecipe1, testRecipe2};
      //Assert
      Assert.Equal(testRecipe, resultRecipe);
    }

    [Fact]
    public void Delete_DeletesCategoriesAssociationsFromDatabase_CategoriesList()
    {
      //Arrange
      Recipe testRecipe = new Recipe("cheesy sandwich", "cheese");
      testRecipe.Save();

      Categories testCategories = new Categories("meditarian");
      testCategories.Save();

      //Act
      testCategories.AddRecipe(testRecipe);
      testCategories.Delete();

      List<Categories> resultRecipeCategories = testRecipe.GetCategories();
      List<Categories> testRecipeCategories = new List<Categories> {};

      //Assert
      Assert.Equal(testRecipeCategories, resultRecipeCategories);
    }

    public void Dispose()
    {
      Categories.DeleteAll();
    }
  }
}
