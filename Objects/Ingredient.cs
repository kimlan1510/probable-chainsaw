using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
  public class Ingredient
  {
    private int _id;
    private string _name;

    public Ingredient(string name, int id = 0)
    {
      _name = name;
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

    public override bool Equals(System.Object otherIngredient)
    {
      if(!(otherIngredient is Ingredient))
      {
        return false;
      }
      else
      {
        Ingredient newIngredient = (Ingredient) otherIngredient;
        bool idEquality = (this.GetId() == newIngredient.GetId());
        bool nameEquality = (this.GetName() == newIngredient.GetName());
        return(idEquality && nameEquality);
      }
    }

    public static List<Ingredient> GetAll()
    {
      List<Ingredient> AllIngredient = new List<Ingredient>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Ingredient newIngredient = new Ingredient(name, id);
        AllIngredient.Add(newIngredient);
      }
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
      return AllIngredient;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO ingredients (name) OUTPUT INSERTED.id VALUES (@name);", conn);

      SqlParameter namePara = new SqlParameter("@name", this.GetName());

      cmd.Parameters.Add(namePara);


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

    public static Ingredient Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @id;", conn);
      SqlParameter idParameter = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string name = null;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
        Ingredient foundIngredient = new Ingredient(name, foundId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundIngredient;
    }

    public void Update(string name)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE ingredients SET name = @name WHERE id = @Id;", conn);

      SqlParameter namePara = new SqlParameter("@name", name);
      SqlParameter idPara = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(idPara);

      this._name = name;
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Recipe> GetRecipe()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM ingredients JOIN recipes_ingredients ON (ingredients.id = recipes_ingredients.ingredients_id) JOIN recipes ON (recipes_ingredients.recipes_id = recipes.id) WHERE ingredients.id = @IngredientId;", conn);
      SqlParameter IngredientIdParam = new SqlParameter();
      IngredientIdParam.ParameterName = "@IngredientId";
      IngredientIdParam.Value = this.GetId().ToString();

      cmd.Parameters.Add(IngredientIdParam);
      SqlDataReader rdr = cmd.ExecuteReader();
      List<Recipe> recipes = new List<Recipe>{};

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string instruction = rdr.GetString(2);

        Recipe newRecipe = new Recipe(name, instruction, recipeId);
        recipes.Add(newRecipe);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return recipes;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients WHERE id = @ingredientId; DELETE FROM recipes_ingredients WHERE ingredients_id = @ingredientId;", conn);
      SqlParameter ingredientIdParameter = new SqlParameter("@ingredientId", this.GetId());

      cmd.Parameters.Add(ingredientIdParameter);
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
     SqlCommand cmd = new SqlCommand("DELETE FROM ingredients;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
    }
  }
}
