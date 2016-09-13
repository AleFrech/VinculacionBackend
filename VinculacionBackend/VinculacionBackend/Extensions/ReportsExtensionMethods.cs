using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace VinculacionBackend.Extensions
{
    public static class ReportsExtensionMethods
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo propInfo in properties)
            {
                dataTable.Columns.Add(new DataColumn(propInfo.Name,
                    Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}