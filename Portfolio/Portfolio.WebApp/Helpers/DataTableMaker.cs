using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using Portfolio.PorfolioDomain.Core.Entities;

namespace Portfolio.WebApp.Helpers {
  public static class DataTableMaker {

    public static DataTable ConvertTo<T>(IList<T> list) {
      DataTable table = CreateTable<T>();
      Type entityType = typeof(T);
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

      foreach (T item in list)
      {
        DataRow row = table.NewRow();

        foreach (PropertyDescriptor prop in properties)
        {
          row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        }

        table.Rows.Add(row);
      }

      return table;
    }

    public static DataTable CreateTable<T>() {
      Type entityType = typeof(T);
      DataTable table = new DataTable(entityType.Name);
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

      foreach (PropertyDescriptor prop in properties)
      {
        // HERE IS WHERE THE ERROR IS THROWN FOR NULLABLE TYPES
        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
      }
      if (typeof(T) == typeof(Project)) {
          table.Columns.Remove("ProjectOwner");
          table.Columns.Remove("ProjectRequirements");
          table.Columns.Remove("ProjectLinks");
          table.Columns.Remove("PublishedHistory");


      }
      if (typeof(T) == typeof(Certification)) {
        table.Columns.Remove("ProjectCreator");
      }
      return table;
    }
  }
}