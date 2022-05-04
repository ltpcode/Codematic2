using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LTP.IDBO;
using LTP.Utility;
using LTP.CodeHelper;
namespace LTP.CodeBuild
{
    public class BuilderWeb : BuilderFrame
    {
        public BuilderWeb(IDbObject idbobj, string dbName, string tableName, string modelName, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string nameSpace, string folder)
        {
            dbobj = idbobj;
            DbName = dbName;
            TableName = tableName;            
            ModelName = modelName;
            Folder = folder;
            NameSpace = nameSpace;
            Fieldlist = fieldlist;
            Keys = keys;

            foreach (ColumnInfo key in keys)
            {
                _key = key.ColumnName;
                _keyType = key.TypeName;
                if (key.IsIdentity)
                {
                    _key = key.ColumnName;
                    _keyType = CodeCommon.DbTypeToCS(key.TypeName);
                    break;
                }
            }
        }

        #region Aspx页面html

        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        public string GetAddAspx()
        {            
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">" );
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                bool ispk=field.IsPK;
                bool IsIdentity = field.IsIdentity;
                if ((ispk) || (IsIdentity))
                {
                    continue;
                }
                if (deText.Trim() == "")
                {
                    deText = columnName;
                }
                strclass.AppendSpaceLine(1, "<tr>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
                strclass.AppendSpaceLine(2, deText);
                strclass.AppendSpaceLine(1, "</td>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
                switch (columnType.Trim().ToLower())
                {
                    case "datetime":
                    case "smalldatetime":
                        strclass.AppendSpaceLine(2, "<INPUT onselectstart=\"return false;\" onkeypress=\"return false\" id=\"txt" + columnName + "\" onfocus=\"setday(this)\"");
                        strclass.AppendSpaceLine(2, " readOnly type=\"text\" size=\"10\" name=\"Text1\" runat=\"server\">");
                        break;
                    case "bit":
                        strclass.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");                        
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "<asp:TextBox id=\"txt" + columnName + "\" runat=\"server\" Width=\"200px\"></asp:TextBox>");
                        break;
                }
                strclass.AppendSpaceLine(1, "</td></tr>");
            }
                        
            //按钮
            strclass.AppendSpaceLine(1, "<tr>");
            strclass.AppendSpaceLine(1, "<td height=\"25\" colspan=\"2\"><div align=\"center\">");
            strclass.AppendSpaceLine(2,"<asp:Button ID=\"btnAdd\" runat=\"server\" Text=\"· 提交 ·\" OnClick=\"btnAdd_Click\" ></asp:Button>");
            strclass.AppendSpaceLine(2,"<asp:Button ID=\"btnCancel\" runat=\"server\" Text=\"· 重填 ·\" OnClick=\"btnCancel_Click\" ></asp:Button>");
            strclass.AppendSpaceLine(1, "</div></td></tr>");

            strclass.AppendLine("</table>" );
            return strclass.ToString();

        }

        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        public string GetUpdateAspx()
        {            
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">" );
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                bool ispk = field.IsPK;
                bool IsIdentity = field.IsIdentity;
                if (deText.Trim() == "")
                {
                    deText = columnName;
                }
                if ((ispk) || (IsIdentity))
                {
                    strclass.AppendSpaceLine(1, "<tr>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
                    strclass.AppendSpaceLine(2, deText);
                    strclass.AppendSpaceLine(1, "</td>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
                    strclass.AppendSpaceLine(2, "<asp:label id=\"lbl" + columnName + "\" runat=\"server\"></asp:label>");
                    strclass.AppendSpaceLine(1, "</td></tr>");
                }
                else
                {
                    //
                    strclass.AppendSpaceLine(1, "<tr>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
                    strclass.AppendSpaceLine(2, deText);
                    strclass.AppendSpaceLine(1, "</td>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
                    switch (columnType.Trim())
                    {
                        case "datetime":
                        case "smalldatetime":
                            strclass.AppendSpaceLine(2, "<INPUT onselectstart=\"return false;\" onkeypress=\"return false\" id=\"txt" + columnName + "\" onfocus=\"setday(this)\"");
                            strclass.AppendSpaceLine(2, " readOnly type=\"text\" size=\"10\" name=\"Text1\" runat=\"server\">");
                            break;
                        case "bit":
                            strclass.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");
                            break;
                        default:
                            strclass.AppendSpaceLine(2, "<asp:TextBox id=\"txt" + columnName + "\" runat=\"server\" Width=\"200px\"></asp:TextBox>");
                            break;
                    }
                    strclass.AppendSpaceLine(1, "</td></tr>");
                }
            }

            
            //按钮
            strclass.AppendSpaceLine(1, "<tr>");
            strclass.AppendSpaceLine(1, "<td height=\"25\" colspan=\"2\"><div align=\"center\">");
            strclass.AppendSpaceLine(2, "<asp:Button ID=\"btnAdd\" runat=\"server\" Text=\"· 提交 ·\" OnClick=\"btnAdd_Click\" ></asp:Button>");
            strclass.AppendSpaceLine(2, "<asp:Button ID=\"btnCancel\" runat=\"server\" Text=\"· 取消 ·\" OnClick=\"btnCancel_Click\" ></asp:Button>");
            strclass.AppendSpaceLine(1, "</div></td></tr>");
            strclass.AppendLine("</table>" );
            return strclass.Value;

        }
       
        /// <summary>
        /// 得到表示层显示窗体的html代码
        /// </summary>     
        public string GetShowAspx()
        {            
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">" + "\r\n");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                //bool ispk = field.IsPK;
                //bool IsIdentity = field.IsIdentity;
                if (deText.Trim() == "")
                {
                    deText = columnName;
                }
                strclass.Append(Space(1) + "<tr>" + "\r\n");
                strclass.Append(Space(1) + "<td height=\"25\" width=\"30%\" align=\"right\">" + "\r\n");
                strclass.Append(Space(2) + deText + "\r\n");
                strclass.Append(Space(1) + "</td>" + "\r\n");
                strclass.Append(Space(1) + "<td height=\"25\" width=\"*\" align=\"left\">" + "\r\n");
                switch (columnType.Trim())
                {
                    case "bit":
                        strclass.Append(Space(2) + "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />" + "\r\n");
                        break;
                    default:
                        strclass.Append(Space(2) + "<asp:Label id=\"lbl" + columnName + "\" runat=\"server\"></asp:Label>" + "\r\n");
                        break;
                }
                strclass.Append(Space(1) + "</td></tr>" + "\r\n");
            }
            
            strclass.Append("</table>" + "\r\n");
            return strclass.ToString();

        }
        
        /// <summary>
        /// 增删改3个页面代码
        /// </summary>      
        public string GetWebHtmlCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
        {
            StringPlus strclass = new StringPlus();
            if (AddForm)
            {
                strclass.AppendLine(" <!--******************************增加页面代码********************************-->");
                strclass.AppendLine(GetAddAspx());
            }
            if (UpdateForm)
            {
                strclass.AppendLine(" <!--******************************修改页面代码********************************-->");
                strclass.AppendLine(GetUpdateAspx());
            }

            if (ShowForm)
            {
                strclass.AppendLine("  <!--******************************显示页面代码********************************-->");
                strclass.AppendLine(GetShowAspx());
            }
            
            return strclass.ToString();
        }
        
        #endregion

        #region 表示层 CS

        /// <summary>
        /// 得到表示层增加窗体的代码
        /// </summary>      
        public string GetAddAspxCs()
        {            
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            StringBuilder strclass0 = new StringBuilder();
            StringBuilder strclass1 = new StringBuilder();
            StringBuilder strclass2 = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append("	string strErr=\"\";\r\n");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                bool ispk = field.IsPK;
                bool IsIdentity = field.IsIdentity;
                if ((ispk) || (IsIdentity))
                {
                    continue;
                }
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                        strclass0.Append("	int " + columnName + "=int.Parse(this.txt" + columnName + ".Text);" + "\r\n");
                        strclass1.Append("	if(!PageValidate.IsNumber(txt" + columnName + ".Text))" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不是数字！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                    case "float":
                    case "numeric":
                    case "decimal":
                        strclass0.Append("	decimal " + columnName + "=decimal.Parse(this.txt" + columnName + ".Text);" + "\r\n");
                        strclass1.Append("	if(!PageValidate.IsDecimal(txt" + columnName + ".Text))" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不是数字！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                    case "datetime":
                    case "smalldatetime":
                        strclass0.Append("	DateTime " + columnName + "=DateTime.Parse(this.txt" + columnName + ".Text);" + "\r\n");
                        strclass1.Append("	if(!PageValidate.IsDateTime(txt" + columnName + ".Text))" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不是时间格式！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                    case "bool":
                        strclass0.Append("	bool " + columnName + "=this.chk" + columnName + ".Checked;" + "\r\n");                                                
                        break;
                    case "byte[]":
                        strclass0.Append("	byte[] " + columnName + "= new UnicodeEncoding().GetBytes(this.txt" + columnName + ".Text);" + "\r\n");
                        break;
                    default:
                        strclass0.Append("	string " + columnName + "=this.txt" + columnName + ".Text;" + "\r\n");
                        strclass1.Append("	if(this.txt" + columnName + ".Text ==\"\")" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不能为空！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                }
                strclass2.Append("	model." + columnName + "=" + columnName + ";" + "\r\n");

            }

            strclass.Append(strclass1.ToString() + "\r\n");
            strclass.Append("	if(strErr!=\"\")" + "\r\n");
            strclass.Append("	{" + "\r\n");
            strclass.Append("		MessageBox.Show(this,strErr);" + "\r\n");
            strclass.Append("		return;" + "\r\n");
            strclass.Append("	}" + "\r\n");
            strclass.Append(strclass0.ToString() + "\r\n");
            strclass.Append("\r\n");
            strclass.Append("	" + ModelSpace + " model=new " + ModelSpace + "();" + "\r\n");
            strclass.Append(strclass2.ToString());



            strclass.Append("	" + BLLSpace + " bll=new " + BLLSpace + "();" + "\r\n");
            strclass.Append("	bll.Add(model);");

            return strclass.ToString();
        }

        /// <summary>
        /// 得到修改窗体的代码
        /// </summary>      
        public string GetUpdateAspxCs()
        {            
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            StringBuilder strclass0 = new StringBuilder();
            StringBuilder strclass1 = new StringBuilder();
            StringBuilder strclass2 = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append("	string strErr=\"\";\r\n");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;                
                bool ispk = field.IsPK;
                bool IsIdentity = field.IsIdentity;
                if ((ispk) || (IsIdentity))
                {
                    continue;
                }
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                        strclass0.Append("	int " + columnName + "=int.Parse(this.txt" + columnName + ".Text);" + "\r\n");
                        strclass1.Append("	if(!PageValidate.IsNumber(txt" + columnName + ".Text))" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不是数字！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                    case "float":
                    case "numeric":
                    case "decimal":
                        strclass0.Append("	decimal " + columnName + "=decimal.Parse(this.txt" + columnName + ".Text);" + "\r\n");
                        strclass1.Append("	if(!PageValidate.IsDecimal(txt" + columnName + ".Text))" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不是数字！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                    case "datetime":
                    case "smalldatetime":
                        strclass0.Append("	DateTime " + columnName + "=DateTime.Parse(this.txt" + columnName + ".Text);" + "\r\n");
                        strclass1.Append("	if(!PageValidate.IsDateTime(txt" + columnName + ".Text))" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不是时间格式！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                    case "bool":
                        strclass0.Append("	bool " + columnName + "=this.chk" + columnName + ".Checked;" + "\r\n");
                        break;
                    case "byte[]":
                        strclass0.Append("	byte[] " + columnName + "= new UnicodeEncoding().GetBytes(this.txt" + columnName + ".Text);" + "\r\n");
                        break;
                    default:
                        strclass0.Append("	string " + columnName + "=this.txt" + columnName + ".Text;" + "\r\n");
                        strclass1.Append("	if(this.txt" + columnName + ".Text ==\"\")" + "\r\n");
                        strclass1.Append("	{" + "\r\n");
                        strclass1.Append("		strErr+=\"" + columnName + "不能为空！\\\\n\";	" + "\r\n");
                        strclass1.Append("	}" + "\r\n");
                        break;
                }
                strclass2.Append("	model." + columnName + "=" + columnName + ";" + "\r\n");

            }
            
            strclass.Append(strclass1.ToString() + "\r\n");
            strclass.Append("	if(strErr!=\"\")" + "\r\n");
            strclass.Append("	{" + "\r\n");
            strclass.Append("		MessageBox.Show(this,strErr);" + "\r\n");
            strclass.Append("		return;" + "\r\n");
            strclass.Append("	}" + "\r\n");
            strclass.Append(strclass0.ToString() + "\r\n");
            strclass.Append("\r\n");
            strclass.Append("	" + ModelSpace + " model=new " + ModelSpace + "();" + "\r\n");
            strclass.Append(strclass2.ToString());
            
            strclass.Append("	" + BLLSpace + " bll=new " + BLLSpace + "();" + "\r\n");
            strclass.Append("	bll.Update(model);");

            return strclass.ToString();
        }

        /// <summary>
        /// 得到修改窗体的代码
        /// </summary>       
        public string GetUpdateShowAspxCs()
        {
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            string key = Key;//CodeCommon.DbTypeToCS(_keyType) + " " + key
            strclass.Append("private void ShowInfo(" + LTP.CodeHelper.CodeCommon.GetInParameter(Keys) + ")" + "\r\n");
            strclass.Append("{" + "\r\n");
            strclass.Append("	" + BLLSpace + " bll=new " + BLLSpace + "();" + "\r\n");
            strclass.Append("	" + ModelSpace + " model=bll.GetModel(" + LTP.CodeHelper.CodeCommon.GetFieldstrlist(Keys) + ");" + "\r\n");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                bool ispk = field.IsPK;
                bool IsIdentity = field.IsIdentity;

                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                    case "float":
                    case "numeric":
                    case "decimal":
                    case "datetime":
                    case "smalldatetime":
                        if ((ispk) || (IsIdentity))
                        {
                            strclass.Append("	this.lbl" + columnName + ".Text=model." + columnName + ".ToString();" + "\r\n");
                        }
                        else
                        {
                            strclass.Append("	this.txt" + columnName + ".Text=model." + columnName + ".ToString();" + "\r\n");
                        }                        
                        break;                   
                    case "bool":                        
                        strclass.Append("	this.chk" + columnName + ".Checked=model." + columnName +";" + "\r\n");
                        break;
                    case "byte[]":
                        strclass.Append("	this.txt" + columnName + ".Text=model." + columnName + ".ToString();" + "\r\n");
                        break;
                    default:
                        if ((ispk) || (IsIdentity))
                        {
                            strclass.Append("	this.lbl" + columnName + ".Text=model." + columnName + ";" + "\r\n");
                        }
                        else
                        {
                            strclass.Append("	this.txt" + columnName + ".Text=model." + columnName + ";" + "\r\n");
                        }                         
                        break;
                }
            }

            strclass.Append("\r\n");
            strclass.Append("}");
            return strclass.ToString();
        }


        /// <summary>
        /// 得到表示层显示窗体的代码
        /// </summary>       
        public string GetShowAspxCs()
        {
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            string key = Key;
            strclass.Append("private void ShowInfo(" + LTP.CodeHelper.CodeCommon.GetInParameter(Keys) + ")" + "\r\n");
            strclass.Append("{" + "\r\n");
            strclass.Append("	" + BLLSpace + " bll=new " + BLLSpace + "();" + "\r\n");
            strclass.Append("	" + ModelSpace + " model=bll.GetModel(" + LTP.CodeHelper.CodeCommon.GetFieldstrlist(Keys) + ");" + "\r\n");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                bool ispk = field.IsPK;
                bool IsIdentity = field.IsIdentity;
                if ((ispk) || (IsIdentity))
                {
                    continue;
                }
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                    case "float":
                    case "numeric":
                    case "decimal":
                    case "datetime":
                    case "smalldatetime":
                        strclass.Append("	this.lbl" + columnName + ".Text=model." + columnName + ".ToString();" + "\r\n");
                        break;
                    case "bool":
                        strclass.Append("	this.chk" + columnName + ".Checked=model." + columnName + ";" + "\r\n");
                        break;
                    case "byte[]":
                        strclass.Append("	this.lbl" + columnName + ".Text=model." + columnName + ".ToString();" + "\r\n");
                        break;
                    default:
                        strclass.Append("	this.lbl" + columnName + ".Text=model." + columnName + ";" + "\r\n");
                        break;
                }
            }
                        
            strclass.Append("\r\n");
            strclass.Append("}");
            return strclass.ToString();
        }


        /// <summary>
        /// 删除页面
        /// </summary>
        /// <returns></returns>
        public string CreatDeleteForm()
        {
            StringBuilder strclass = new StringBuilder();
            strclass.Append("if(!Page.IsPostBack)\r\n");
            strclass.Append("{\r\n");
            strclass.Append("	" + BLLSpace + " bll=new " + BLLSpace + "();\r\n");
            switch (_keyType.Trim())
            {
                case "int":
                case "smallint":
                case "float":
                case "numeric":
                case "decimal":
                case "datetime":
                case "smalldatetime":
                    strclass.Append("	" + _keyType + " " + _key + "=" + _keyType + ".Parse(Request.Params[\"id\"]);\r\n");
                    break;
                default:
                    strclass.Append("	string " + _key + "=Request.Params[\"id\"];\r\n");
                    break;
            }
            strclass.Append("	bll.Delete(" + _key + ");\r\n");
            strclass.Append("	Response.Redirect(\"index.aspx\");\r\n");
            strclass.Append("}\r\n");
            return strclass.ToString();

        }

        public string CreatSearchForm()
        {
            StringBuilder strclass = new StringBuilder();

            return strclass.ToString();
        }

        public string GetWebCode(bool ExistsKey, bool AddForm,bool UpdateForm, bool ShowForm, bool SearchForm)
        {
            StringPlus strclass = new StringPlus();
            if (AddForm)
            {
                strclass.AppendLine("  /******************************增加窗体代码********************************/" );
                strclass.AppendLine(GetAddAspxCs() + "\r\n");                
            }
            if (UpdateForm)
            {
                strclass.AppendLine("  /******************************修改窗体代码********************************/");
                strclass.AppendLine("  /*修改代码-显示 */");
                strclass.AppendLine(GetUpdateShowAspxCs() + "\r\n");
                strclass.AppendLine("  /*修改代码-提交更新 */");
                strclass.AppendLine(GetUpdateAspxCs() + "\r\n");                
            }            
            if (ShowForm)
            {
                strclass.AppendLine("  /******************************显示窗体代码********************************/" );
                strclass.AppendLine(GetShowAspxCs() + "\r\n");
            }                       
            //if (DelForm)
            //{
            //    strclass.Append("  /******************************删除窗体代码********************************/" );
            //    strclass.Append("\r\n");
            //    strclass.Append(CreatDeleteForm() + "\r\n");
            //}

            return strclass.Value;
        }

        #endregion//表示层

        #region  生成aspx.designer.cs
        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        public string GetAddDesigner()
        {
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();           
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                bool ispk = field.IsPK;
                bool IsIdentity = field.IsIdentity;
                if ((ispk) || (IsIdentity))
                {
                    continue;
                }
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "datetime":
                    case "smalldatetime":
                        strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                        break;
                    case "bool":
                        strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                        break;
                }                
            }
            //按钮
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnAdd;");
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
            return strclass.ToString();

        }

        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        public string GetUpdateDesigner()
        {
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
          
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                bool ispk = field.IsPK;
                bool IsIdentity = field.IsIdentity;
                if (deText.Trim() == "")
                {
                    deText = columnName;
                }
                if ((ispk) || (IsIdentity))
                {
                    strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Label lbl" + columnName + ";");                    
                }
                else
                {
                    switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                    {
                        case "datetime":
                        case "smalldatetime":
                            strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                            break;
                        case "bool":
                            strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
                            break;
                        default:
                            strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                            break;
                    }                
                }
            }

            //按钮            
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnAdd;");
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
            return strclass.Value;

        }

        /// <summary>
        /// 得到表示层显示窗体的html代码
        /// </summary>     
        public string GetShowDesigner()
        {
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();                       
            
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.DeText;
                
                if (deText.Trim() == "")
                {
                    deText = columnName;
                }
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {                    
                    case "bool":
                        strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
                        break;
                    default:
                        strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Label lbl" + columnName + ";");
                        break;
                }      
                            
            }                        
            return strclass.ToString();

        }
        #endregion

    }
}
