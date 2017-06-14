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

    public static void DeleteAll()
    {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
    }
  }
}
