using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
  public class Categories
  {
    private int _id;
    private string _name;

    public Categories(string name, int id = 0)
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

    public override bool Equals(System.Object otherCategories)
    {
      if(!(otherCategories is Categories))
      {
        return false;
      }
      else
      {
        Categories newCategories = (Categories) otherCategories;
        bool idEquality = (this.GetId() == newCategories.GetId());
        bool nameEquality = (this.GetName() == newCategories.GetName());
        return(idEquality && nameEquality);
      }
    }

    public static List<Categories> GetAll()
    {
      List<Categories> AllCategories = new List<Categories>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Categories newCategories = new Categories(name, id);
        AllCategories.Add(newCategories);
      }
      if (rdr != null)
      {
       rdr.Close();
      }
      if (conn != null)
      {
       conn.Close();
      }
      return AllCategories;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@name);", conn);

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

    public void AddRecipe(Recipe newRecipe)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories_recipes (recipes_id, categories_id) VALUES (@RecipeId, @CategoriesId);", conn);

      SqlParameter recipeIdParameter = new SqlParameter("@RecipeId", newRecipe.GetId());
      SqlParameter categoriesIdParameter = new SqlParameter( "@CategoriesId", this.GetId());

      cmd.Parameters.Add(recipeIdParameter);
      cmd.Parameters.Add(categoriesIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }


    public static Categories Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @id;", conn);
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
        Categories foundCategories = new Categories(name, foundId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCategories;
    }

    public void Update(string name)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE categories SET name = @name WHERE id = @Id;", conn);

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

      SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM categories JOIN categories_recipes ON (categories.id = categories_recipes.categories_id) JOIN recipes ON (categories_recipes.recipes_id = recipes.id) WHERE categories.id = @categoriesId;", conn);
      SqlParameter CategoriesIdParam = new SqlParameter("@categoriesId", this.GetId().ToString());

      cmd.Parameters.Add(CategoriesIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Recipe> recipes = new List<Recipe>{};

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string instructions = rdr.GetString(2);
        Recipe newRecipe = new Recipe(name, instructions, recipeId);
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

      SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id = @categoryId; DELETE FROM categories_recipes WHERE categories_id = @categoryId;", conn);
      SqlParameter categoryIdParameter = new SqlParameter("@categoryId", this.GetId());

      cmd.Parameters.Add(categoryIdParameter);
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
     SqlCommand cmd = new SqlCommand("DELETE FROM categories; DELETE FROM categories_recipes", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
    }
  }
}
