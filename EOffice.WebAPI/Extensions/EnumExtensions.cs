using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Extensions;

namespace EOffice.WebAPI.Extensions
{
    public static class EnumExtensions 
    {
        // public static string DescriptionAttr<T>(this T source)
        // {
        //     FieldInfo fi = source.GetType().GetField(source.ToString());
        //
        //     DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
        //         typeof(DescriptionAttribute), false);
        //
        //     if (attributes != null && attributes.Length > 0) return attributes[0].Description;
        //     else return source.ToString();
        // }

        public static (string, string) GetEnumData<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return (source.ToString(), attributes[0].Description);
            else return (source.ToString(), source.ToString());
        }
        
        public static string Description<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
        public static string Name<T>(this T source)
        {
            return source.ToString();
        }

        public static (string,string) GetValue(this Enum source, string name)
        {
            if (source.GetDisplayName() == name)
            {
                return source.GetEnumData();
            }
            return ("")
        }
    }
}