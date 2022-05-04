using System;
using System.Collections.Generic;
using System.Text;

namespace VSProject
{
    public class VisualStudioProject
    {
        string strLastOpenVersion;
        string strReferencePath;
        Config[] obj_config;
        OtherProjectSettings obj_otherProjectSettings;

        public string LastOpenVersion
        {
            get { return strLastOpenVersion; }
            set { strLastOpenVersion = value; }
        }

        public string ReferencePath
        {
            get { return strReferencePath; }
            set { strReferencePath = value; }
        }

        public Config[] ConfigObject
        {
            get { return obj_config; }
            set { obj_config = value; }
        }

        public OtherProjectSettings OtherProjectSettingsObject
        {
            get { return obj_otherProjectSettings; }
            set { obj_otherProjectSettings = value; }
        }
    }
}
