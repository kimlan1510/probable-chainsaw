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



    public void Dispose()
    {
      Rating.DeleteAll();
    }



  }
}
