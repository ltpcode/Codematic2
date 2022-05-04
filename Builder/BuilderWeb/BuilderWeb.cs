using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Maticsoft.IDBO;
using Maticsoft.Utility;
using Maticsoft.CodeHelper;

namespace Maticsoft.BuilderWeb
{
    /// <summary>
    /// Web层代码组件
    /// </summary>
    public class BuilderWeb : IBuilder.IBuilderWeb
    {
        #region 私有字段
        protected string _key = "ID";//默认第一个主键字段		
        protected string _keyType = "int";//默认第一个主键类型        
        protected string _namespace = "Maticsoft"; //顶级命名空间名
        private string _folder = "";//所在文件夹
        protected string _modelname; //model类名           
        protected string _bllname; //model类名
        protected List<ColumnInfo> _fieldlist;
        protected List<ColumnInfo> _keys;
        #endregion

        #region 公有属性
        /// <summary>
        /// 顶级命名空间名 
        /// </summary>        
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        /// <summary>
        /// 所在文件夹名
        /// </summary>
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }
        /// <summary>
        /// Model类名
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// BLL类名
        /// </summary>
        public string BLLName
        {
            set { _bllname = value; }
            get { return _bllname; }
        }

        /// <summary>
        /// 实体类的整个命名空间+类名
        /// </summary>
        public string ModelSpace
        {
            get
            {
                string _modelspace = _namespace + "." + "Model";
                if (_folder.Trim() != "")
                {
                    _modelspace += "." + _folder;
                }
                _modelspace += "." + ModelName;
                return _modelspace;
            }
        }

        /// <summary>
        /// 业务逻辑层的操作类名称定义
        /// </summary>
        private string BLLSpace
        {
            get
            {
                string _bllspace = _namespace + "." + "BLL";
                if (_folder.Trim() != "")
                {
                    _bllspace += "." + _folder;
                }
                _bllspace += "." + BLLName;
                return _bllspace;
            }
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// 主键或条件字段列表 
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }
        /// <summary>
        /// 主键标识字段
        /// </summary>
        protected string Key
        {
            get
            {
                foreach (ColumnInfo key in _keys)
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
                return _key;
            }
        }
        #endregion

        public BuilderWeb()
        {
        }

        #region 公共方法



        /// <summary>
        /// 过滤一些不需要显示的列（增加页，修改页，列表页）
        /// </summary>
        /// <param name="columnName"></param>
        private bool isFilterColume(string columnName)
        {
            //个性定制-特殊字段处理
            //if (
            //        (columnName.IndexOf("_iCreator") > 0) ||  //页面不需要这4列
            //        (columnName.IndexOf("_dateCreate") > 0) ||
            //        (columnName.IndexOf("_iMaintainer") > 0) ||
            //        (columnName.IndexOf("_dateMaintain") > 0) ||
            //        (columnName.IndexOf("_bValid") > 0) ||
            //        (columnName.IndexOf("_dateValid") > 0) ||
            //        (columnName.IndexOf("_dateExpire") > 0)
            //        )
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;

        }

        #endregion

        #region Aspx页面html

        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        public string GetAddAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            bool hasDate = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                if (IsIdentity)
                {
                    continue;
                }

                if (isFilterColume(columnName))
                {
                    continue;
                }
                if (columnType.Trim().ToLower() == "uniqueidentifier")
                {
                    continue;
                }

                deText = CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                strclass.AppendSpaceLine(1, "<tr>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
                strclass.AppendSpaceLine(2, deText);
                strclass.AppendSpaceLine(1, "：</td>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
                
                
                switch (columnType.Trim().ToLower())
                {
                    case "datetime":
                    case "smalldatetime":
                        strclass.AppendSpaceLine(2, "<asp:TextBox ID=\"txt" + columnName + "\" runat=\"server\" Width=\"70px\"  onfocus=\"setday(this)\"></asp:TextBox>");
                        hasDate = true;
                        break;
                    case "bit":
                        strclass.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");
                        break;
                    case "uniqueidentifier":
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "<asp:TextBox id=\"txt" + columnName + "\" runat=\"server\" Width=\"200px\"></asp:TextBox>");
                        break;
                }
                strclass.AppendSpaceLine(1, "</td></tr>");
            }                       
            strclass.AppendLine("</table>");
            if (hasDate)
            {
                strclass.AppendLine("<script src=\"/js/calendar1.js\" type=\"text/javascript\"></script>");
            }
            return strclass.ToString();

        }

        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        public string GetUpdateAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            bool hasDate = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);
                if (isFilterColume(columnName))
                {
                    continue;
                }

                if ((ispk) || (IsIdentity) || (columnType.Trim().ToLower() == "uniqueidentifier"))
                {
                    strclass.AppendSpaceLine(1, "<tr>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
                    strclass.AppendSpaceLine(2, deText);
                    strclass.AppendSpaceLine(1, "：</td>");
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
                    strclass.AppendSpaceLine(1, "：</td>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
                                                           
                    switch (columnType.Trim().ToLower())
                    {
                        case "datetime":
                        case "smalldatetime":
                            //strclass.AppendSpaceLine(2, "<INPUT onselectstart=\"return false;\" onkeypress=\"return false\" id=\"txt" + columnName + "\" onfocus=\"setday(this)\"");
                            //strclass.AppendSpaceLine(2, " readOnly type=\"text\" size=\"10\" name=\"Text1\" runat=\"server\">");
                            strclass.AppendSpaceLine(2, "<asp:TextBox ID=\"txt" + columnName + "\" runat=\"server\" Width=\"70px\"  onfocus=\"setday(this)\"></asp:TextBox>");
                            hasDate = true;
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

            ////按钮
            //strclass.AppendSpaceLine(1, "<tr>");
            //strclass.AppendSpaceLine(1, "<td height=\"25\" colspan=\"2\"><div align=\"center\">");
            //strclass.AppendSpaceLine(2, "<asp:Button ID=\"btnSave\" runat=\"server\" Text=\"· 保存 ·\" OnClick=\"btnSave_Click\" ></asp:Button>");
            ////strclass.AppendSpaceLine(2, "<asp:Button ID=\"btnCancel\" runat=\"server\" Text=\"· 取消 ·\" OnClick=\"btnCancel_Click\" ></asp:Button>");
            //strclass.AppendSpaceLine(1, "</div></td></tr>");
            strclass.AppendLine("</table>");
            if (hasDate)
            {
                strclass.AppendLine("<script src=\"/js/calendar1.js\" type=\"text/javascript\"></script>");
            }
            return strclass.Value;

        }

        /// <summary>
        /// 得到表示层显示窗体的html代码
        /// </summary>     
        public string GetShowAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

               
                strclass.AppendSpaceLine(1, "<tr>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
                strclass.AppendSpaceLine(2, deText);
                strclass.AppendSpaceLine(1, "：</td>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
                switch (columnType.Trim().ToLower())
                {
                    //case "bit":
                    //    strclass.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />" );
                    //    break;
                    default:
                        strclass.AppendSpaceLine(2, "<asp:Label id=\"lbl" + columnName + "\" runat=\"server\"></asp:Label>");
                        break;
                }
                strclass.AppendSpaceLine(1, "</td></tr>");
            }
            strclass.AppendLine("</table>");
            return strclass.ToString();

        }

        /// <summary>
        /// 得到表示层列表窗体的html代码
        /// </summary>     
        public string GetListAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();

            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                if (IsIdentity)
                {
                    continue;
                }

                if (isFilterColume(columnName))
                {
                    continue;
                }                               

                switch (columnType.Trim().ToLower())
                {
                    case "bit":
                    case "dateTime":
                        strclass.AppendSpaceLine(2, "<asp:BoundField DataField=\"" + columnName + "\" HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\"  /> ");
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "<asp:BoundField DataField=\"" + columnName + "\" HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\"  /> ");
                        break;
                }
            }
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
            if (SearchForm)
            {
                strclass.AppendLine("  <!--******************************列表页面代码********************************-->");
                strclass.AppendLine(GetListAspx());
            }
            return strclass.ToString();
        }
        #endregion

        #region 表示层 CS

        /// <summary>
        /// 生成表示层页面的CS代码
        /// </summary>
        /// <param name="ExistsKey"></param>
        /// <param name="AddForm">是否生成增加窗体的代码</param>
        /// <param name="UpdateForm">是否生成修改窗体的代码</param>
        /// <param name="ShowForm">是否生成显示窗体的代码</param>
        /// <param name="SearchForm">是否生成查询窗体的代码</param>
        /// <returns></returns>
        public string GetWebCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
        {
            StringPlus strclass = new StringPlus();
            if (AddForm)
            {
                strclass.AppendLine("  /******************************增加窗体代码********************************/");
                strclass.AppendLine(GetAddAspxCs());
            }
            if (UpdateForm)
            {
                strclass.AppendLine("  /******************************修改窗体代码********************************/");
                strclass.AppendLine("  /*修改代码-显示 */");
                strclass.AppendLine(GetUpdateShowAspxCs());
                strclass.AppendLine("  /*修改代码-提交更新 */");
                strclass.AppendLine(GetUpdateAspxCs());
            }
            if (ShowForm)
            {
                strclass.AppendLine("  /******************************显示窗体代码********************************/");
                strclass.AppendLine(GetShowAspxCs());
            }
            //if (DelForm)
            //{
            //    strclass.Append("  /******************************删除窗体代码********************************/");
            //    strclass.Append("");
            //    strclass.Append(CreatDeleteForm());
            //}
            return strclass.Value;
        }

        /// <summary>
        /// 得到表示层增加窗体的代码
        /// </summary>      
        public string GetAddAspxCs()
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass0 = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(3, "string strErr=\"\";");
            //bool ishasuser = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                if ((IsIdentity))
                {
                    continue;
                }
                if ("uniqueidentifier" == columnType.ToLower())
                {
                    continue;
                }
                //个性定制-特殊字段处理
                //if ((!ishasuser) && ((columnName.IndexOf("_iCreator") > 0) || (columnName.IndexOf("_iMaintainer") > 0)))
                //{
                //    strclass0.AppendSpaceLine(3, "User currentUser;");
                //    strclass0.AppendSpaceLine(3, "if (Session[\"UserInfo\"] != null)");
                //    strclass0.AppendSpaceLine(3, "{");
                //    strclass0.AppendSpaceLine(4, "currentUser = (User)Session[\"UserInfo\"];");
                //    strclass0.AppendSpaceLine(3, "}else{");
                //    strclass0.AppendSpaceLine(4, "return;");
                //    strclass0.AppendSpaceLine(3, "}");
                //    ishasuser = true;
                //}

                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

               
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                       strclass0.AppendSpaceLine(3, "int " + columnName + "=int.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "格式错误！\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        break;
                    case "float":
                    case "numeric":
                    case "decimal":
                        strclass0.AppendSpaceLine(3, "decimal " + columnName + "=decimal.Parse(this.txt" + columnName + ".Text);");
                        strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDecimal(txt" + columnName + ".Text))");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "格式错误！\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");
                        break;
                    case "datetime":
                    case "smalldatetime":
                        strclass0.AppendSpaceLine(3, "DateTime " + columnName + "=DateTime.Parse(this.txt" + columnName + ".Text);");
                        strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDateTime(txt" + columnName + ".Text))");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "格式错误！\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");

                        break;
                    case "bool":
                        strclass0.AppendSpaceLine(3, "bool " + columnName + "=this.chk" + columnName + ".Checked;");
                        break;
                    case "byte[]":
                        strclass0.AppendSpaceLine(3, "byte[] " + columnName + "= new UnicodeEncoding().GetBytes(this.txt" + columnName + ".Text);");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        break;
                    default:
                        strclass0.AppendSpaceLine(3, "string " + columnName + "=this.txt" + columnName + ".Text;");
                        strclass1.AppendSpaceLine(3, "if(this.txt" + columnName + ".Text.Trim().Length==0)");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "不能为空！\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");
                        break;
                }
                strclass2.AppendSpaceLine(3, "model." + columnName + "=" + columnName + ";");
            }
            strclass.AppendLine(strclass1.ToString());
            strclass.AppendSpaceLine(3, "if(strErr!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "MessageBox.Show(this,strErr);");
            strclass.AppendSpaceLine(4, "return;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendLine(strclass0.ToString());
            strclass.AppendSpaceLine(3, ModelSpace + " model=new " + ModelSpace + "();");
            strclass.AppendLine(strclass2.ToString());
            strclass.AppendSpaceLine(3, BLLSpace + " bll=new " + BLLSpace + "();");
            strclass.AppendSpaceLine(3, "bll.Add(model);");
            strclass.AppendSpaceLine(3, "Maticsoft.Common.MessageBox.ShowAndRedirect(this,\"保存成功！\",\"add.aspx\");");
            return strclass.Value;
        }

        /// <summary>
        /// 得到修改窗体的代码
        /// </summary>      
        public string GetUpdateAspxCs()
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass0 = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(3, "string strErr=\"\";");
            //bool ishasuser = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;

                //个性定制-特殊字段处理
                //if (
                //    (columnName.IndexOf("_iCreator") > 0) ||  //页面不需要这2列
                //    (columnName.IndexOf("_dateCreate") > 0) ||
                //    (columnName.IndexOf("_bValid") > 0) ||
                //    (columnName.IndexOf("_dateValid") > 0) ||
                //    (columnName.IndexOf("_dateExpire") > 0)
                //    )
                //{
                //    continue;
                //}
                //if ((!ishasuser) && (columnName.IndexOf("_iMaintainer") > 0))
                //{
                //    strclass0.AppendSpaceLine(4, "User currentUser;");
                //    strclass0.AppendSpaceLine(3, "if (Session[\"UserInfo\"] != null)");
                //    strclass0.AppendSpaceLine(3, "{");
                //    strclass0.AppendSpaceLine(4, "currentUser = (User)Session[\"UserInfo\"];");
                //    strclass0.AppendSpaceLine(3, "}else{");
                //    strclass0.AppendSpaceLine(4, "return;");
                //    strclass0.AppendSpaceLine(3, "}");
                //    ishasuser = true;
                //}


                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                //个性定制-特殊字段处理
                //if ((columnName.IndexOf("_cLang") > 0) && (columnType.Trim().ToLower() == "varchar"))//语言代码
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistLanguage1.LanguageCode;");
                //    continue;
                //}
                //if (columnName.IndexOf("_iAuthority") > 0)//权限角色代码
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistPermission1.PermissionID;");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrency") > 0)//货币代码
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistCurrency1.CurrencyCode;");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrencyUnit") > 0)//货币代码
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistCurrencyUnit1.CurrencyUnitID;");
                //    continue;
                //}

                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                        if ((ispk) || (IsIdentity))
                        {
                            strclass0.AppendSpaceLine(3, "int " + columnName + "=int.Parse(this.lbl" + columnName + ".Text);");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "int " + columnName + "=int.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "格式错误！\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "long":                    
                        if ((ispk) || (IsIdentity))
                        {
                            strclass0.AppendSpaceLine(3, "long " + columnName + "=long.Parse(this.lbl" + columnName + ".Text);");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "long " + columnName + "=long.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "格式错误！\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "float":
                    case "numeric":
                    case "decimal":
                        if ((ispk) || (IsIdentity))
                        {
                            strclass0.AppendSpaceLine(3, "decimal " + columnName + "=decimal.Parse(this.lbl" + columnName + ".Text);");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "decimal " + columnName + "=decimal.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDecimal(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "格式错误！\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "datetime":
                    case "smalldatetime":                        
                        strclass0.AppendSpaceLine(3, "DateTime " + columnName + "=DateTime.Parse(this.txt" + columnName + ".Text);");
                        strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDateTime(txt" + columnName + ".Text))");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "格式错误！\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");

                        break;
                    case "bool":
                        strclass0.AppendSpaceLine(3, "bool " + columnName + "=this.chk" + columnName + ".Checked;");
                        break;
                    case "byte[]":
                        strclass0.AppendSpaceLine(3, "byte[] " + columnName + "= new UnicodeEncoding().GetBytes(this.txt" + columnName + ".Text);");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        strclass0.AppendSpaceLine(3, "Guid " + columnName + "= new Guid(this.lbl" + columnName + ".Text);");
                        break;
                    default:
                        if ((ispk) || (IsIdentity))
                        {
                            strclass0.AppendSpaceLine(3, "string " + columnName + "=this.lbl" + columnName + ".Text;");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "string " + columnName + "=this.txt" + columnName + ".Text;");
                            strclass1.AppendSpaceLine(3, "if(this.txt" + columnName + ".Text.Trim().Length==0)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "不能为空！\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                }
                strclass2.AppendSpaceLine(3, "model." + columnName + "=" + columnName + ";");

            }
            strclass.AppendLine(strclass1.ToString());
            strclass.AppendSpaceLine(3, "if(strErr!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "MessageBox.Show(this,strErr);");
            strclass.AppendSpaceLine(4, "return;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendLine(strclass0.ToString());
            strclass.AppendLine();
            strclass.AppendSpaceLine(3, ModelSpace + " model=new " + ModelSpace + "();");
            strclass.AppendLine(strclass2.ToString());
            strclass.AppendSpaceLine(3, BLLSpace + " bll=new " + BLLSpace + "();");
            strclass.AppendSpaceLine(3, "bll.Update(model);");
            strclass.AppendSpaceLine(3, "Maticsoft.Common.MessageBox.ShowAndRedirect(this,\"保存成功！\",\"list.aspx\");");
            return strclass.ToString();
        }

        /// <summary>
        /// 得到修改窗体的代码
        /// </summary>       
        public string GetUpdateShowAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            string key = Key;
            strclass.AppendSpaceLine(1, "private void ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, BLLSpace + " bll=new " + BLLSpace + "();");
            strclass.AppendSpaceLine(2, ModelSpace + " model=bll.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;

                if (isFilterColume(columnName))
                {
                    continue;
                }

                //个性定制-特殊字段处理
                //if ((columnName.IndexOf("_cLang") > 0) && (columnType.Trim().ToLower() == "varchar"))
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistLanguage1.LanguageCode =model." + columnName + ";");
                //    continue;
                //}
                //if (columnName.IndexOf("_iAuthority") > 0)
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistPermission1.PermissionID =model." + columnName + ";");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrency") > 0)//货币代码
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistCurrency1.CurrencyCode =model." + columnName + ";");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrencyUnit") > 0)//货币代码
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistCurrencyUnit1.CurrencyUnitID =model." + columnName + ";");
                //    continue;
                //}

                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "long":
                    case "smallint":
                    case "float":
                    case "numeric":
                    case "decimal":
                    case "datetime":
                    case "smalldatetime":
                        if ((ispk) || (IsIdentity))
                        {
                            strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        }
                        else
                        {
                            strclass.AppendSpaceLine(2, "this.txt" + columnName + ".Text=model." + columnName + ".ToString();");
                        }
                        break;
                    case "bool":
                        strclass.AppendSpaceLine(2, "this.chk" + columnName + ".Checked=model." + columnName + ";");
                        break;
                    case "byte[]":
                        strclass.AppendSpaceLine(2, "this.txt" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    default:
                        if ((ispk) || (IsIdentity))
                        {
                            strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ";");
                        }
                        else
                        {
                            strclass.AppendSpaceLine(2, "this.txt" + columnName + ".Text=model." + columnName + ";");
                        }
                        break;
                }
            }
            strclass.AppendLine();
            strclass.AppendSpaceLine(1, "}");
            return strclass.Value;
        }


        /// <summary>
        /// 得到表示层显示窗体的代码
        /// </summary>       
        public string GetShowAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            string key = Key;
            strclass.AppendSpaceLine(1, "private void ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, BLLSpace + " bll=new " + BLLSpace + "();");
            strclass.AppendSpaceLine(2, ModelSpace + " model=bll.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            //bool ishasuser = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                //if ((ispk) || (IsIdentity))
                //{
                //    continue;
                //}

                #region 特殊字段处理
                //个性定制-特殊字段处理
                //if (columnName.IndexOf("_iAuthority") > 0)
                //{
                //    continue;
                //}
                //if ((columnName.IndexOf("_cLang") > 0) && (columnType.Trim().ToLower() == "varchar"))//语言代码
                //{
                //    strclass.AppendSpaceLine(2, "BLL.SysManage.MultiLanguage bllML = new BLL.SysManage.MultiLanguage();");
                //    strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text= bllML.GetLanguageNameByCache(model." + columnName + ");");
                //    continue;
                //}
                //if ((!ishasuser) && ((columnName.IndexOf("_iCreator") > 0) || (columnName.IndexOf("_iMaintainer") > 0)))
                //{
                //    strclass.AppendSpaceLine(2, "Maticsoft.Accounts.Bus.User user = new Maticsoft.Accounts.Bus.User();");
                //    ishasuser = true;
                //}
                //if ((columnName.IndexOf("_iCreator") > 0) || (columnName.IndexOf("_iMaintainer") > 0))
                //{
                //    strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text= user.GetTrueNameByCache(model." + columnName + ");");
                //    ishasuser = true;
                //    continue;
                //}
                #endregion

                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "long":
                    case "smallint":
                    case "float":
                    case "numeric":
                    case "decimal":
                    case "datetime":
                    case "smalldatetime":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    case "bool":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + "?\"是\":\"否\";");
                        break;
                    case "byte[]":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ";");
                        break;
                }
            }
            strclass.AppendLine();
            strclass.AppendSpaceLine(1, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// 得到表示层列表窗体的代码
        /// </summary>       
        public string GetListAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpace(2, BLLSpace + " bll = new " + BLLSpace + "();");

            return strclass.ToString();
        }

        /// <summary>
        /// 删除页面
        /// </summary>
        /// <returns></returns>
        public string GetDeleteAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(1, "if(!Page.IsPostBack)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, BLLSpace + " bll=new " + BLLSpace + "();");
            switch (_keyType.Trim())
            {
                case "int":
                case "long":
                case "smallint":
                case "float":
                case "numeric":
                case "decimal":
                case "datetime":
                case "smalldatetime":
                    strclass.AppendSpaceLine(3, _keyType + " " + _key + "=" + _keyType + ".Parse(Request.Params[\"id\"]);");
                    break;
                default:
                    strclass.AppendSpaceLine(3, "string " + _key + "=Request.Params[\"id\"];");
                    break;
            }
            strclass.AppendSpaceLine(3, "bll.Delete(" + _key + ");");
            strclass.AppendSpaceLine(3, "Response.Redirect(\"list.aspx\");");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

        }

        public string CreatSearchForm()
        {
            StringPlus strclass = new StringPlus();

            return strclass.Value;
        }



        #endregion//表示层

        #region  生成aspx.designer.cs
        /// <summary>
        /// 增加窗体的html代码
        /// </summary>      
        public string GetAddDesigner()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                if (IsIdentity)
                {
                    continue;
                }
                if (isFilterColume(columnName))
                {
                    continue;
                }
                if ("uniqueidentifier" == columnType.ToLower())
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
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnSave;");
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
            return strclass.ToString();

        }

        /// <summary>
        /// 修改窗体的html代码
        /// </summary>      
        public string GetUpdateDesigner()
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                if (isFilterColume(columnName))
                {
                    continue;
                }

                if ((ispk) || (IsIdentity) || (columnType.Trim().ToLower() == "uniqueidentifier"))
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
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnSave;");
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
            return strclass.Value;

        }

        /// <summary>
        /// 显示窗体的html代码
        /// </summary>     
        public string GetShowDesigner()
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;

                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    //case "bool":
                    //    strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
                    //    break;
                    default:
                        strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Label lbl" + columnName + ";");
                        break;
                }

            }
            return strclass.ToString();

        }

        /// <summary>
        /// 列表窗体的html代码
        /// </summary>     
        public string GetListDesigner()
        {
            StringPlus strclass = new StringPlus();            
            return strclass.ToString();
        }


        #endregion



    }


}
