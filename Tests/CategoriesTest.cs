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

    public void Dispose()
    {
      Categories.DeleteAll();
    }
  }
}
