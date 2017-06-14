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

    public ingredient(string name, int id = 0)
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
        bool idEquality = (this.GetId() == otherIngredient.GetId());
        bool nameEquality = (this.GetName() == otherIngredient.GetName());
        return(idEquality && nameEquality);
      }
    }
  }
}
