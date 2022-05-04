using System;
using System.Collections.Generic;
using System.Text;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
using Maticsoft.CmConfig;
namespace Maticsoft.CodeEngine
{

    [Serializable]
    public class DatabaseHost : TemplateHost
    {
        public DatabaseHost()
        {
        }                                        
        private string _dbname;        
        private string _dbhelperName= "DbHelperSQL";//数据库访问类名
        private List<TableInfo> _tablelist;
        private List<TableInfo> _viewlist;
        private List<TableInfo> _procedurelist;


        private string _dbtype;                        
        private string _procprefix = "";//存储过程前缀
        private string _projectname = "";//项目名称       

        private DbSettings _dbset;

        private string modelPrefix = "";
        private string modelSuffix = "";
        private string bllPrefix = "";
        private string bllSuffix = "";
        private string dalPrefix = "";
        private string dalSuffix = "";
        private string tabnameRule = "same";
        
        public string DbName
        {
            set { _dbname = value; }
            get { return _dbname; }
        }        
        public string DbHelperName
        {
            set { _dbhelperName = value; }
            get { return _dbhelperName; }
        }

        /// <summary>
        /// 当前数据库所有的视图对象数组
        /// </summary>
        public List<TableInfo> ViewList
        {
            set { _viewlist = value; }
            get { return _viewlist; }
        }
        /// <summary>
        /// 当前数据库所有的存储过程对象数组
        /// </summary>
        public List<TableInfo> ProcedureList
        {
            set { _procedurelist = value; }
            get { return _procedurelist; }
        }
        /// <summary>
        /// 当前数据库所有的表对象数组
        /// </summary>
        public List<TableInfo> TableList
        {
            set { _tablelist = value; }
            get { return _tablelist; }
        }
        /// <summary>
        /// 数据源类型 
        /// </summary>        
        public string DbType
        {
            set { _dbtype = value; }
            get { return _dbtype; }
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        public DbSettings DbSet
        {
            set { _dbset = value; }
            get { return _dbset; }
        }
        
        #region 
        
        /// <summary>
        /// 存储过程前缀 
        /// </summary>
        public string ProcPrefix
        {
            set { _procprefix = value; }
            get { return _procprefix; }
        }
        /// <summary>
        /// 项目名称 
        /// </summary>
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
                
        /// <summary>
        /// Model类名前缀 
        /// </summary>        
        public string ModelPrefix
        {
            set { modelPrefix = value; }
            get { return modelPrefix; }
        }
        /// <summary>
        /// Model类名后缀 
        /// </summary>        
        public string ModelSuffix
        {
            set { modelSuffix = value; }
            get { return modelSuffix; }
        }
        /// <summary>
        /// BLL类名前缀 
        /// </summary>        
        public string BLLPrefix
        {
            set { bllPrefix = value; }
            get { return bllPrefix; }
        }
        /// <summary>
        /// BLL类名后缀 
        /// </summary>        
        public string BLLSuffix
        {
            set { bllSuffix = value; }
            get { return bllSuffix; }
        }

        /// <summary>
        /// DAL类名前缀 
        /// </summary>        
        public string DALPrefix
        {
            set { dalPrefix = value; }
            get { return dalPrefix; }
        }
        /// <summary>
        /// DAL类名后缀 
        /// </summary>        
        public string DALSuffix
        {
            set { dalSuffix = value; }
            get { return dalSuffix; }
        }
        /// <summary>
        /// 表名大小写规则: same(保持原样)  lower（全部小写）  upper（全部大写）
        /// </summary>        
        public string TabNameRule
        {
            set { tabnameRule = value; }
            get { return tabnameRule; }
        }
               
        #endregion

        #region 构造属性

        /// <summary>
        /// 不同数据库类的前缀
        /// </summary>
        public string DbParaHead
        {
            get
            {
                return CodeCommon.DbParaHead(DbType);
            }

        }
        /// <summary>
        ///  不同数据库字段类型
        /// </summary>
        public string DbParaDbType
        {
            get
            {
                return CodeCommon.DbParaDbType(DbType);
            }
        }

        /// <summary>
        /// 存储过程参数 调用符号@
        /// </summary>
        public string preParameter
        {
            get
            {
                return CodeCommon.preParameter(DbType);
            }
        }

        #endregion

        #region

        /// <summary>
        /// 组合得到Model类名
        /// </summary>
        /// <param name="TabName">表名</param>
        /// <returns></returns>
        public string GetModelClass(string TabName)
        {
            return _dbset.ModelPrefix + TabNameRuled(TabName) + _dbset.ModelSuffix;
        }
        /// <summary>
        /// 组合得到BLL类名
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public string GetBLLClass(string TabName)
        {
            return _dbset.BLLPrefix + TabNameRuled(TabName) + _dbset.BLLSuffix;
        }

        /// <summary>
        /// 组合得到DAL类名
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public string GetDALClass(string TabName)
        {
            return _dbset.DALPrefix + TabNameRuled(TabName) + _dbset.DALSuffix;
        }

        /// <summary>
        /// 表名处理规则
        /// </summary>
        /// <param name="TabName"></param>       
        /// <returns></returns>
        private string TabNameRuled(string TabName)
        {
            string newTabName = TabName;
            if (_dbset.ReplacedOldStr.Length > 0)
            {
                newTabName = newTabName.Replace(_dbset.ReplacedOldStr, _dbset.ReplacedNewStr);
            }
            switch (_dbset.TabNameRule.ToLower())
            {
                case "lower":
                    newTabName = newTabName.ToLower();
                    break;
                case "upper":
                    newTabName = newTabName.ToUpper();
                    break;
                case "firstupper":
                    {
                        string strfir = newTabName.Substring(0, 1).ToUpper();
                        newTabName = strfir + newTabName.Substring(1);
                    }
                    break;
                case "same":
                    break;
                default:
                    break;
            }
            return newTabName;
        }
        #endregion


        

    }
}
