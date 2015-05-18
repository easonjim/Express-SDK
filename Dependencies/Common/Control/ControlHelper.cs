/************************************************************************************		                                    			*
 *      Description:																*
 *				关于页面上Control的基本用法   													    *
 *      Author:																		*
 *				Jim												*
 *      Finish DateTime:															*
 *				2009-04-03														*
 *      History:																	*
 ***********************************************************************************/
using System.Collections.Generic;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

namespace ComLib
{
    /// <summary>
    /// 页面控件通用帮助类,如:清空textbox 获取grid repeater选择的ID集合等
    /// </summary>
    public class ControlHelper
    {
        #region 清空所有的textbox
        /// <summary>
        /// 清空页面的textbox
        /// </summary>
        /// <param name="PageCurrent">页面指针</param>
        public static void ClearTextBoxs(System.Web.UI.Page PageCurrent)
        {
            foreach (Control ctrl in PageCurrent.Controls)
            {
                if (ctrl is HtmlForm)
                {
                    foreach (Control subctrl in ctrl.Controls)
                    {
                        if (subctrl is TextBox)
                        {
                            ((TextBox)subctrl).Text = "";
                        }
                    }
                }
            }
        }
        #endregion

        #region  获取GridView中选中的ID，CheckBox1，HidenID,在checkBox旁边的HiddenFiled绑定ID
        /// <summary>
        /// 获取GridView中选中的ID集合
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="checkboxName">checkBox名字</param>
        /// <param name="HiddenfileName">Hiddenfile控件的名字</param>
        /// <returns>ID的Arraylist集合</returns>
        public static List<string> _GetSelectedArray(GridView gv, string checkboxName, string HiddenfileName)
        {
            List<string> array = new List<string>();
            foreach (GridViewRow gdr in gv.Rows)
            {
                CheckBox ck = gdr.FindControl(checkboxName) as CheckBox;
                if (ck != null && ck.Checked)
                {
                    HiddenField hf = gdr.FindControl(HiddenfileName) as HiddenField;
                    if (hf != null && !string.IsNullOrEmpty(hf.Value))
                        array.Add(hf.Value);
                }
            }
            return array;
        }
        #endregion

        #region "获取Repeater中选中的ID集合"
          /// <summary>
        /// 获取Repeater中选中的ID集合
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="checkboxName">checkBox名字</param>
        /// <param name="HiddenfileName">Hiddenfile控件的名字</param>
        /// <returns>ID的Arraylist集合</returns>
        public static List<string> GetIDlist_FromRepter(Repeater rt, string checkboxName, string HiddenfileName)
        {
            CheckBox chk = null;
            HiddenField hid = null;
            List<string> IDlist = new List<string>();
            foreach (RepeaterItem item in rt.Items)
            {
                chk = (CheckBox)item.FindControl(checkboxName);

                if (chk != null && chk.Checked == true)
                {
                    hid = (HiddenField)item.FindControl(HiddenfileName);
                    if (hid != null && !string.IsNullOrEmpty(hid.Value))
                    {
                        IDlist.Add(hid.Value);
                    }
                }
            }
            return IDlist;
        }
        #endregion
      
        #region "判断 DataTable是否为空 返回bool"
        /// <summary>
        /// 判断dt是否为空 返回bool
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool CheckDataTable(DataTable dt)
        {
            bool Reult = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                Reult = true;
            }
            return Reult;
        }
        #endregion

        #region 绑定一定范围内数值到DDL
        /// <summary>
        /// 绑定一定范围内数值到DDL
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="ddl"></param>
        //public static void BindNumberToDDl(int start, int end, DropDownList ddl)
        //{
        //    if (ddl == null) return;

        //    if (start == end)
        //    {
        //        ListItem li = new ListItem(start.ToString());
        //        ddl.Items.Add(li);
        //    }
        //    else if (start > end)
        //    {
        //        for (int i = start; i >= end; i--)
        //        {
        //            ListItem li = new ListItem(i.ToString());
        //            ddl.Items.Add(li);
        //        }
        //    }
        //    else
        //    {
        //        for (int i = start; i <= end; i++)
        //        {
        //            ListItem li = new ListItem(i.ToString());
        //            ddl.Items.Add(li);
        //        }
        //    }
        //}
        #endregion

        #region "给ListControl添加第一个选项"
        /// <summary>
        /// 给ListControl添加第一个选项
        /// </summary>
        /// <param name="defaultText">默认值是空字符</param>
        /// <param name="ddl"></param>
        public static void AddDefaultToDDl(string defaultText, ListControl listControl)
        {
            ListItem li = new ListItem(defaultText, "");
            listControl.Items.Insert(0, li);
        }
        #endregion

        #region "绑定下拉框控件选中的值"
        /// <summary>
        /// 绑定下拉框控件选中的值
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="SelectedValue"></param>
        public static void BindDropDownList_SelectValue(DropDownList ddl, string SelectedValue)
        {
            if (ddl.Items.Count > 0)
            {
                foreach (ListItem li in ddl.Items)
                {
                    if (li.Value == SelectedValue)
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region "绑定ListBox选中值"
        /// <summary>
        /// 绑定ListBox选中值
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="selectValueList">选中值以,号分隔</param>
        public static void BindListBox_SelectedValue(ListBox lb, List<string> selectValueList)
        {
            if (lb.Items.Count > 0)
            {
                foreach (string s in selectValueList)
                {
                    foreach (ListItem li in lb.Items)
                    {
                        if (li.Value == s)
                        {
                            li.Selected = true;
                            break;
                        }
                    }
                }

            }
        }
        #endregion

        #region "获得ListBox选中值"
        /// <summary>
        /// 获取ListBox选中值
        /// </summary>
        /// <param name="lb"></param>
        /// <returns></returns>
        public static List<string> GetListBoxSelectedValue(ListBox lb)
        {
            List<string> selectedIDList = new List<string>();

            if (lb.Items.Count > 0)
            {
                string flag = "";
                foreach (ListItem li in lb.Items)
                {
                    if (li.Selected)
                    {
                        selectedIDList.Add(li.Value);
                    }
                }
            }
            return selectedIDList;
        }
        #endregion
        
    }
}
