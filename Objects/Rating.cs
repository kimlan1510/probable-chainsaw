using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
  public class Rating
  {
    private int _id;
    private string _user_name;
    private int _score;

    public Rating(string user_name, int score, int id = 0)
    {
      _id = id;
      _user_name = user_name;
      _score = score;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetUserName()
    {
      return _user_name;
    }
    public int GetScore()
    {
      return _score;
    }

    public override bool Equals(System.Object otherRating)
    {
      if(!(otherRating is Rating))
      {
        return false;
      }
      else
      {
        Rating newRating = (Rating) otherRating;
        bool idEquality = (this.GetId() == newRating.GetId());
        bool userNameEquality = (this.GetUserName() == newRating.GetUserName());
        bool scoreEquality = (this.GetScore() == newRating.GetScore());
        return(idEquality && scoreEquality && userNameEquality);
      }
    }

    public static List<Rating> GetAll()
    {
      List<Rating> AllRating = new List<Rating>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM rating", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string userName = rdr.GetString(1);
        int score = rdr.GetInt32(2);
        Rating newRating = new Rating(userName, score, id);
        AllRating.Add(newRating);
      }
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
      return AllRating;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO rating (user_name, score) OUTPUT INSERTED.id VALUES (@user_name, @score);", conn);

      SqlParameter userNamePara = new SqlParameter("@user_name", this.GetUserName());
      SqlParameter scorePara = new SqlParameter("@score", this.GetScore());

      cmd.Parameters.Add(scorePara);
      cmd.Parameters.Add(userNamePara);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Rating Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM rating WHERE id = @id;", conn);
      SqlParameter idParameter = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string user_name = null;
      int score = 0;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        user_name = rdr.GetString(1);
        score= rdr.GetInt32(2);

      }
        Rating foundRating = new Rating(user_name, score, foundId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundRating;
    }

    public void Update(string user_name, int score)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE rating SET user_name = @user_name, score = @score WHERE id = @Id;", conn);

      SqlParameter userNamePara = new SqlParameter("@user_name", user_name);
      SqlParameter scorePara = new SqlParameter("@score", score);
      SqlParameter idPara = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(userNamePara);
      cmd.Parameters.Add(scorePara);
      cmd.Parameters.Add(idPara);

      this._user_name = user_name;
      this._score = score;
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddRatingToRecipe(Recipe newRecipe)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_rating (recipes_id, rating_id) VALUES (@RecipeId, @RatingId);", conn);

      SqlParameter ratingIdParameter = new SqlParameter("@RatingId", this.GetId());
      SqlParameter recipeIdParameter = new SqlParameter( "@RecipeId", newRecipe.GetId());

      cmd.Parameters.Add(recipeIdParameter);
      cmd.Parameters.Add(ratingIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM rating WHERE id = @ratingId; DELETE FROM recipes_rating WHERE rating_id = @ratingId;", conn);
      SqlParameter ratingIdParameter = new SqlParameter("@ratingId", this.GetId());

      cmd.Parameters.Add(ratingIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }







    public static void DeleteAll()
    {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("DELETE FROM rating;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
    }

  }
}
