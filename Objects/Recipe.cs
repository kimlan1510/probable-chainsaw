using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RecipeBox
{
  public class Recipe
  {
    private int _id;
    private string _name;
    private string _instructions;

    public Recipe (string name, string instructions,int id = 0)
    {
      _name = name;
      _instructions = instructions;
      _id = id;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetInstructions()
    {
      return _instructions;
    }

    public override bool Equals(System.Object otherRecipe)
    {
      if(!(otherRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherRecipe;
        bool idEquality = (this.GetId() == newRecipe.GetId());
        bool nameEquality = (this.GetName() == newRecipe.GetName());
        bool instructionsEquality = (this.GetInstructions() == newRecipe.GetInstructions());
        return (idEquality && nameEquality && instructionsEquality);
      }
    }

    public static List<Recipe> GetAll()
    {
      List<Recipe> AllRecipe = new List<Recipe>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string instructions = rdr.GetString(2);
        Recipe newRecipe = new Recipe(name, instructions, id);
        AllRecipe.Add(newRecipe);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllRecipe;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes (name, instructions) OUTPUT INSERTED.id VALUES (@name,  @instructions);", conn);

      SqlParameter namePara = new SqlParameter("@name", this.GetName());
      SqlParameter recipePara = new SqlParameter("@instructions", this.GetInstructions());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(recipePara);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Recipe Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE id = @id;", conn);
      SqlParameter IdPara = new SqlParameter("@id", id.ToString());
      cmd.Parameters.Add(IdPara);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundid = 0;
      string name = null;
      string instructions = null;

      while(rdr.Read())
      {
        foundid = rdr.GetInt32(0);
        name = rdr.GetString(1);
        instructions = rdr.GetString(2);
      }
      Recipe foundRecipe = new Recipe(name, instructions, foundid);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
     return foundRecipe;
    }

    public List<Ingredient> GetIngredient()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT ingredients.* FROM recipes JOIN recipes_ingredients ON (recipes.id = recipes_ingredients.recipes_id) JOIN ingredients ON (recipes_ingredients.ingredients_id = ingredients.id) WHERE recipes.id = @recipesId;", conn);
      SqlParameter IngredientsIdParam = new SqlParameter("@recipesId", this.GetId().ToString());

      cmd.Parameters.Add(IngredientsIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Ingredient> ingredients = new List<Ingredient>{};

      while(rdr.Read())
      {
        int ingredientsId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Ingredient newIngredient = new Ingredient(name, ingredientsId);
        ingredients.Add(newIngredient);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return ingredients;
    }

    public List<Rating> GetRating()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT rating.* FROM recipes JOIN recipes_rating ON (recipes.id = recipes_rating.recipes_id) JOIN rating ON (recipes_rating.rating_id = rating.id) WHERE recipes.id = @recipesId;", conn);
      SqlParameter RecipeIdParam = new SqlParameter("@recipesId", this.GetId().ToString());

      cmd.Parameters.Add(RecipeIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Rating> rating = new List<Rating>{};

      while(rdr.Read())
      {
        int ratingId = rdr.GetInt32(0);
        string user_name = rdr.GetString(1);
        int score = rdr.GetInt32(2);
        Rating newRating = new Rating(user_name, score, ratingId);
        rating.Add(newRating);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return rating;
    }

    public void AddRating(Rating newRating)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_rating (recipes_id, rating_id) VALUES (@RecipeId, @RatingId);", conn);

      SqlParameter ratingIdParameter = new SqlParameter("@RatingId", newRating.GetId());
      SqlParameter recipeIdParameter = new SqlParameter( "@RecipeId", this.GetId());

      cmd.Parameters.Add(recipeIdParameter);
      cmd.Parameters.Add(ratingIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }




    public void AddIngredient(Ingredient newIngredient)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_ingredients (recipes_id, ingredients_id) VALUES (@RecipeId, @IngredientId);", conn);

      SqlParameter recipeIdParameter = new SqlParameter("@RecipeId", this.GetId());
      SqlParameter ingredientIdParameter = new SqlParameter( "@IngredientId", newIngredient.GetId());

      cmd.Parameters.Add(recipeIdParameter);
      cmd.Parameters.Add(ingredientIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Update(string name, string instructions)
    {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("UPDATE recipes SET name = @name, instructions = @instructions WHERE id = @Id;", conn);

    SqlParameter namePara = new SqlParameter("@name", name);
    SqlParameter instructionPara = new SqlParameter("@instructions", instructions);
    SqlParameter idPara = new SqlParameter("@Id", this.GetId());

    cmd.Parameters.Add(namePara);
    cmd.Parameters.Add(instructionPara);
    cmd.Parameters.Add(idPara);

    this._name = name;
    this._instructions = instructions;
    cmd.ExecuteNonQuery();
    conn.Close();
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM recipes WHERE id = @recipeId; DELETE FROM recipes_ingredients WHERE recipes_id = @recipeId; DELETE FROM recipes_rating where recipes_id = @recipeId", conn);
      SqlParameter recipeIdParameter = new SqlParameter("@recipeId", this.GetId());

      cmd.Parameters.Add(recipeIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM recipes;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
