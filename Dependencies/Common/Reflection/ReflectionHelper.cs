using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Collections.Specialized;
using System.Reflection.Emit;

namespace ComLib
{
    #region " DataReader 转换成 实体"
    /// <summary>
    /// DataReader 转换成 实体
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public class IDataReaderEntityBuilder<Entity>
    {
        private static readonly MethodInfo getValueMethod =
        typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod =
            typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
        private delegate Entity Load(IDataRecord dataRecord);

        private Load handler;
        private IDataReaderEntityBuilder() { }
        public Entity Build(IDataRecord dataRecord)
        {
            return handler(dataRecord);
        }
        public static IDataReaderEntityBuilder<Entity> CreateBuilder(IDataRecord dataRecord)
        {
            IDataReaderEntityBuilder<Entity> dynamicBuilder = new IDataReaderEntityBuilder<Entity>();
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(Entity),
                    new Type[] { typeof(IDataRecord) }, typeof(Entity), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(Entity));
            generator.Emit(OpCodes.Newobj, typeof(Entity).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                PropertyInfo propertyInfo = typeof(Entity).GetProperty(dataRecord.GetName(i),
                                                                   BindingFlags.IgnoreCase |
                                                                   BindingFlags.Public
                                                                   | BindingFlags.Instance);
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }
    #endregion

    #region "DataTable 转换成实体"



    /// <summary>
    /// datatable 转换成实体
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public class DataTableEntityBuilder<Entity>
    {
        private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
        private delegate Entity Load(DataRow dataRecord);

        private Load handler;
        private DataTableEntityBuilder() { }

        public Entity Build(DataRow dataRecord)
        {
            return handler(dataRecord);
        }
        public static DataTableEntityBuilder<Entity> CreateBuilder(DataRow dataRecord)
        {
            DataTableEntityBuilder<Entity> dynamicBuilder = new DataTableEntityBuilder<Entity>();
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(Entity), new Type[] { typeof(DataRow) }, typeof(Entity), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(Entity));
            generator.Emit(OpCodes.Newobj, typeof(Entity).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < dataRecord.ItemArray.Length; i++)
            {
                PropertyInfo propertyInfo = typeof(Entity).GetProperty(dataRecord.Table.Columns[i].ColumnName,
                                                                   BindingFlags.IgnoreCase |
                                                                   BindingFlags.Public
                                                                   | BindingFlags.Instance);
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }

    }
    #endregion

    #region "表单赋值的通用方法"

    /// <summary>
    ///  得到页面表单值赋值给实体 
    ///  下面是用法
    ///  Person Pmodel = new Person();
    ///  HenqPost<Person>.GetPost( ref Pmodel, Request.Form );
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class HenqPost<T> where T : new()
    {
        private static readonly Dictionary<string, PropertyInfo[]> dict;
        public static int GetPost<T>(ref T t, NameValueCollection form, string prefix)
        {
           
            int va = 0;
            PropertyInfo[] pi = null;
            if (dict != null || dict.Count > 0)
            {
                if (dict[t.ToString()] != null)
                    pi = dict[t.ToString()];
                else
                {
                    Type type = t.GetType();
                    pi = type.GetProperties();
                }

            }
            else
            {
                Type type = t.GetType();
                pi = type.GetProperties();
            }

            foreach (PropertyInfo p in pi)
            {
                var tempFromName = form[p.Name];
                tempFromName = string.IsNullOrEmpty(tempFromName)
                                   ? tempFromName
                                   : prefix + tempFromName;
                if (tempFromName != null)
                {
                    try
                    {
                        p.SetValue(t, Convert.ChangeType(tempFromName, p.PropertyType), null);//为属性赋值，并转换键值的类型为该属性的类型
                        va++;//记录赋值成功的属性数
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return va;
        }

        public static int GetPost<T>(ref T t, System.Web.HttpContext hct)
        {
            NameValueCollection nv = hct.Request.Params;
            return GetPost<T>(ref t, nv,"");
        }
    }
    #endregion

}
