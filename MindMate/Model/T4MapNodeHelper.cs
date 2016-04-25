using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using MindMate.Plugins.Tasks;

#if DEBUG

namespace MindMate.Model
{
    /// <summary>
    /// A helper class for writing T4 code generation templates.
    /// Only available in DEBUG build.
    /// </summary>
    public class T4MapNodeHelper
    {
        public PropertyInfo[] GetProperties()
        {
            return typeof(MapNode).GetProperties();
        }

        public IEnumerable<PropertyInfo> GetSerializedProperties()
        {
            var props = from p in typeof (MapNode).GetProperties()
                where p.GetCustomAttributes(true).Any(a => a.GetType() == typeof (MindMate.Serialization.SerializedAttribute))
                select p;
            
            return props;
        }

        public IEnumerable<PropertyInfo> GetSerializedPropertiesInOrder()
        {
            var props = from p in typeof(MapNode).GetProperties()
                        let a = (MindMate.Serialization.SerializedAttribute)Attribute.GetCustomAttribute(p, typeof(MindMate.Serialization.SerializedAttribute))
                        where a != null
                        orderby a.Order ascending 
                        select p;

            return props;
        }

        public IEnumerable<PropertyInfo> GetSerializedScalarPropertiesInOrder()
        {
            var props = from p in typeof(MapNode).GetProperties()
                        let a = (MindMate.Serialization.SerializedAttribute)Attribute.GetCustomAttribute(p, typeof(MindMate.Serialization.SerializedAttribute))
                        where a != null && 
                            (!p.PropertyType.GetInterfaces().Contains(typeof(System.Collections.IEnumerable))
                            ||
                            p.PropertyType == typeof(string))
                        orderby a.Order ascending
                        select p;

            return props;
        }

        public string GetToStringStatement(PropertyInfo p, string objIdentifier)
        {
            if (p.PropertyType == typeof (string))
            {
                return objIdentifier + "." + p.Name;
            }
            if(p.PropertyType.IsPrimitive)
            {
                return objIdentifier + "." + p.Name + ".ToString()";
            }
            if (p.PropertyType.IsEnum)
            {
                return objIdentifier + "." + p.Name + ".ToString()";
            }
            if (p.PropertyType == typeof (Color))
            {
                return "Convert.ToColorHexValue(" + objIdentifier + "." + p.Name + ")";
            }
            if (p.PropertyType == typeof (DateTime))
            {
                return objIdentifier + "." + p.Name + ".ToString(CultureInfo.InvariantCulture)";
            }

            return objIdentifier + "." + p.Name + ".ToString()";
        }

        public string GetConvertFromStringStatement(PropertyInfo p, string stringExpression)
        {
            if (p.PropertyType == typeof(string))
            {
                return stringExpression;
            }
            if (p.PropertyType.IsPrimitive)
            {
                return p.PropertyType.Name + ".Parse(" + stringExpression + ")";
            }
            if (p.PropertyType.IsEnum)
            {
                return "(" + p.PropertyType.Name + ")Enum.Parse(typeof(" + p.PropertyType.Name + "), " + stringExpression + ")";
            }
            if (p.PropertyType == typeof(Color))
            {
                return "(Color)new ColorConverter().ConvertFromString(" + stringExpression + ")";
            }
            if (p.PropertyType == typeof(DateTime))
            {
                return "DateHelper.ToDateTime(" + stringExpression + ")";
            }

            return stringExpression;
        }
    }

}

#endif
