using System;
using System.Collections.Generic;
using System.Text;

namespace Codematic
{
    public class CMTreeNode : System.Windows.Forms.TreeNode
    {
        
        #region Enums
        public enum NodeType
        {
            Empty = -1,
            ServerList = 0,
            Server = 1,
            Database = 2,
            TableRoot = 3,
            ViewRoot = 4,
            StoredProcedureRoot = 5,
            FunctionRoot = 6,
            Table = 7,
            View = 8,
            StoredProcedure = 9,
            Function = 10,
            Filed=11,                   
            Triggers = 12,
            Trigger = 13,
            Project = 14,
            Unknown = 15           
        };
        #endregion

        #region Constructor		

        public CMTreeNode(string nodeName, string nodetype, string server, string dbname,string dbtype)
		{
            this.Text = nodeName;
            this.nodeName = nodeName;
            this.nodetype = nodetype;			
			this.server=server;
            this.dbname = dbname;
            this.dbtype = dbtype;
			//this.ImageIndex=GetImageIndex();
			//this.SelectedImageIndex=GetImageIndex();
			//this.DBConnectionType=((QCTreeNode)this.Parent).DBConnectionType;
					
		}
		#endregion

        #region Fields

        public string nodeName;
        public string server;
        public string dbname;
        public string dbtype;
        public string nodetype;
        #endregion

        #region Properies
       

        #endregion

        


    }
}
