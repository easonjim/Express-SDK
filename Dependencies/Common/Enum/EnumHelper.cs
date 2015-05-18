/********************************************************************************
     ** 创建人：    Jim
     ** 创建时间:   2009/11/02
     ** 功能描述：  通过枚举类型 绑定到ListControl 控件的通用类
     **            
     ** 用法:       var dict = EnumManager<LogState>.GetEnumDictionary();
                    this.DropDownList1.DataSource = dict;
                    this.DropDownList1.DataTextField = "value";
                    this.DropDownList1.DataValueField = "key";
                    this.DropDownList1.DataBind();
 * 
        直接传入要绑定的Control:   EnumManager<LogState>.Bind_Enum_Control(ListControl);
*********************************************************************************/
using System;
using System.Collections.Generic;

using System.Web.UI.WebControls;

namespace ComLib
{
    /// <summary>
    /// 通过枚举类型 绑定到ListControl 控件的通用类
    /// 用法:       var dict = EnumManager<T>.GetEnumDictionar
    ///             this.DropDownList1.DataSource = dict;
    ///             this.DropDownList1.DataTextField = "value";
    ///             this.DropDownList1.DataValueField = "key";
    ///             this.DropDownList1.DataBind();
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public static class EnumManager<TEnum> where TEnum : struct, IConvertible
    {
        private static readonly Dictionary<string, Dictionary<int, string>> EnumDictionary = new Dictionary<string, Dictionary<int, string>>();

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumDictionary()
        {
            if (EnumDictionary.ContainsKey(typeof(TEnum).FullName))
                return EnumDictionary[typeof(TEnum).FullName];

            var EnumDict = Add_Enum_To_Diconary();
            return EnumDict;

        }
        /// <summary>
        /// 传入控件进行绑定
        /// </summary>
        /// <param name="control"></param>
        public static void Bind_Enum_Control(ListControl control)
        {
            control.DataSource = GetEnumDictionary();
            control.DataTextField = "value";
            control.DataValueField = "key";
            control.DataBind();
        }
        /// <summary>
        /// 通过key获取Enum的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEnumValue(int key)
        {
            if (EnumDictionary.ContainsKey(typeof(TEnum).FullName))
                return EnumDictionary[typeof(TEnum).FullName][key];
            var EnumDic = Add_Enum_To_Diconary();
            return EnumDic[key];
        }

        #region "主方法"
        private static Dictionary<int, string> Add_Enum_To_Diconary()
        {
            Type t = typeof(TEnum);
            string[] _names = Enum.GetNames(t);
            int[] _values = Enum.GetValues(t) as int[];
            Dictionary<int, string> EnumDict = new Dictionary<int, string>();

            for (int i = 0; i < _values.Length; i++)
            {
                EnumDict.Add(_values[i], _names[i]);
            }

            EnumDictionary.Add(t.FullName, EnumDict);
            return EnumDict;
        }
        #endregion


    }


}
