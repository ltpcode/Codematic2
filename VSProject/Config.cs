using System;
using System.Collections.Generic;
using System.Text;

namespace VSProject
{
    public class Config
    {
        string strName;
        string strEnableASPDebugging;
        string strEnableASPXDebugging;
        string strEnableUnmanagedDebugging;
        string strEnableSQLServerDebugging;
        string strRemoteDebugEnabled;
        string strRemoteDebugMachine;
        string strStartAction;
        string strStartArguments;
        string strStartPage;
        string strStartProgram;
        string strStartURL;
        string strStartWorkingDirectory;
        string strStartWithIE;

        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }

        public string EnableASPDebugging
        {
            get { return strEnableASPDebugging; }
            set { strEnableASPDebugging = value; }
        }

        public string EnableASPXDebugging
        {
            get { return strEnableASPXDebugging; }
            set { strEnableASPXDebugging = value; }
        }

        public string EnableUnmanagedDebugging
        {
            get { return strEnableUnmanagedDebugging; }
            set { strEnableUnmanagedDebugging = value; }
        }

        public string EnableSQLServerDebugging
        {
            get { return strEnableSQLServerDebugging; }
            set { strEnableSQLServerDebugging = value; }
        }

        public string RemoteDebugEnabled
        {
            get { return strRemoteDebugEnabled; }
            set { strRemoteDebugEnabled = value; }
        }

        public string RemoteDebugMachine
        {
            get { return strRemoteDebugMachine; }
            set { strRemoteDebugMachine = value; }
        }

        public string StartAction
        {
            get { return strStartAction; }
            set { strStartAction = value; }
        }

        public string StartArguments
        {
            get { return strStartArguments; }
            set { strStartArguments = value; }
        }

        public string StartPage
        {
            get { return strStartPage; }
            set { strStartPage = value; }
        }

        public string StartProgram
        {
            get { return strStartProgram; }
            set { strStartProgram = value; }
        }

        public string StartURL
        {
            get { return strStartURL; }
            set { strStartURL = value; }
        }

        public string StartWorkingDirectory
        {
            get { return strStartWorkingDirectory; }
            set { strStartWorkingDirectory = value; }
        }

        public string StartWithIE
        {
            get { return strStartWithIE; }
            set { strStartWithIE = value; }
        }
    }
      

}
