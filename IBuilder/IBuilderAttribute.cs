using System;
using System.Collections.Generic;
using System.Text;

namespace Maticsoft.IBuilder
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class IBuilderAttribute : Attribute
    {
        #region  Ù–‘
        private string _guid;
        private string _name;
        private string _desc;
        private string _assembly;
        private string _classname;
        private string _version;

        public string Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string Decription
        {
            set { _desc = value; }
            get { return _desc; }
        }
        public string Assembly
        {
            set { _assembly = value; }
            get { return _assembly; }
        }
        public string Classname
        {
            set { _classname = value; }
            get { return _classname; }
        }
        public string Version
        {
            set { _version = value; }
            get { return _version; }
        }

        #endregion
    }
}
