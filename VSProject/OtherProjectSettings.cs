using System;
using System.Collections.Generic;
using System.Text;

namespace VSProject
{
    public class OtherProjectSettings
    {
        string strCopyProjectDestinationFolder;
        string strCopyProjectUncPath;
        string strCopyProjectOption;
        string strProjectView;
        string strProjectTrust;

        public string CopyProjectDestinationFolder
        {
            get { return strCopyProjectDestinationFolder; }
            set { strCopyProjectDestinationFolder = value; }
        }
        public string CopyProjectUncPath
        {
            get { return strCopyProjectUncPath; }
            set { strCopyProjectUncPath = value; }
        }
        public string CopyProjectOption
        {
            get { return strCopyProjectOption; }
            set { strCopyProjectOption = value; }
        }
        public string ProjectView
        {
            get { return strProjectView; }
            set { strProjectView = value; }
        }
        public string ProjectTrust
        {
            get { return strProjectTrust; }
            set { strProjectTrust = value; }
        }
    }
}
