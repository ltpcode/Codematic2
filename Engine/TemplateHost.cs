using System;
using System.Collections.Generic;
using System.Text;
using Maticsoft.CodeHelper;
using Maticsoft.CmConfig;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TextTemplating;

namespace Maticsoft.CodeEngine
{
    //The text template transformation engine is responsible for running the transformation process.
    //The host is responsible for all input and output, locating files, 
    //and anything else related to the external environment.   
    [Serializable]
    public class TemplateHost : ITextTemplatingEngineHost
    {
        //public CustomCmdLineHost()
        //{
        //    this.LoadIncludeText( [0] = @"D:\Project Test\CustomHost\CustomHost\bin\Debug\CustomHost.exe";
        //}

        #region 属性

        /// <summary>
        /// 获取所处理文本模板的路径和文件名。 
        /// </summary>     
        internal string _templateFileValue;
        public string TemplateFile
        {
            get { return _templateFileValue; }
            set { _templateFileValue = value; }
        }

        /// <summary>
        /// 获取程序集引用的列表。当编译和执行生成的类时，引擎用到这些引用
        /// 这个是你的代码中要引入的包包，这个例子中引入了 System.dll 和 本项目的 dll，
        /// 如果你不想引用，那么就得在 你的 tt 文件中引入 
        /// </summary>
        public IList<string> StandardAssemblyReferences
        {
            get
            {
                return new string[]
                {                    
                    typeof(System.Uri).Assembly.Location,
                    typeof(ColumnInfo).Assembly.Location,
                    typeof(CodeCommon).Assembly.Location,
                    typeof(DbSettings).Assembly.Location,
                    typeof(DatabaseHost).Assembly.Location,
                    typeof(TableHost).Assembly.Location,
                    typeof(ProcedureHost).Assembly.Location,
                    typeof(TemplateHost).Assembly.Location //本项目的dll，
                    //typeof(DbType).Assembly.get_Location(), 
                    //typeof(DefaultHost).Assembly.get_Location(), 
                    //typeof(DbSchema).Assembly.get_Location() };
                    
                };
            }
        }


        /// <summary>
        /// 获取命名空间的列表。引擎将增加这些声明到生成的类。
        /// 这个是你的代码中要using 的命名空间，这个例子中引入了 System 和 本项目的，
        /// 如果你不想引用，那么就得在你的 tt 文件中写
        /// </summary>
        public IList<string> StandardImports
        {
            get
            {
                return new string[]
                {
                    "System",
                    "System.Text", 
                    //"System.Data", 
                    "System.Collections.Generic", 
                    "Maticsoft.CodeHelper", 
                    "Maticsoft.CodeEngine"
                };
            }
        }

        #endregion


        #region 扩展属性
        
        private string _namespace = "Maticsoft";//顶级命名空间名                        
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }       
                
        
        //This will be the extension of the generated text output file.
        //The host can provide a default by setting the value of the field here.
        //The engine can change this value based on the optional output directive
        //if the user specifies it in the text template.        
        private string _fileExtensionValue = ".cs";
        public string FileExtension
        {
            get { return _fileExtensionValue; }
        }

        //This will be the encoding of the generated text output file.
        //The host can provide a default by setting the value of the field here.
        //The engine can change this value based on the optional output directive
        //if the user specifies it in the text template.
        //---------------------------------------------------------------------
        private Encoding _fileEncodingValue = Encoding.UTF8;
        public Encoding FileEncoding
        {
            get { return _fileEncodingValue; }
        }

        //These are the errors that occur when the engine processes a template.
        //The engine passes the errors to the host when it is done processing,
        //and the host can decide how to display them. For example, the host 
        //can display the errors in the UI or write them to a file.
        //---------------------------------------------------------------------
        private CompilerErrorCollection _ErrorCollection;
        public CompilerErrorCollection ErrorCollection
        {
            get
            {
                return this._ErrorCollection;
            }
        }


        #endregion

        #region 方法


        //方法定义为类功能还可以包含嵌入的文本块。 
        //考虑将类的功能放在一个单独的文件，其中您可以 $$$$到一个或多个模板文件。 
        //Called by the Engine to enquire about the processing options you require. 
        //If you recognize that option, return an appropriate value. 
        //Otherwise, pass back NULL.
        public object GetHostOption(string optionName)
        {
            object returnObject;
            switch (optionName)
            {
                case "CacheAssemblies":
                    returnObject = true;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }

        //The engine calls this method based on the optional include directive
        //if the user has specified it in the text template.
        //This method can be called 0, 1, or more times.
        //The included text is returned in the context parameter.
        //If the host searches the registry for the location of include files,
        //or if the host searches multiple locations by default, the host can
        //return the final path of the include file in the location parameter.
        //获取文本，它对应于包含部分文本模板文件的请求。 
        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = System.String.Empty;
            location = System.String.Empty;
            //If the argument is the fully qualified path of an existing file, then we are done.            
            if (File.Exists(requestFileName))
            {
                content = File.ReadAllText(requestFileName);
                return true;
            }
            //This can be customized to search specific paths for the file.
            //This can be customized to accept paths to search as command line arguments.            
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 接收来自转换引擎的错误和警告集合。
        /// </summary>
        public void LogErrors(CompilerErrorCollection errors)
        {
            _ErrorCollection = errors;
        }


        /// <summary>
        /// 提供运行所生成转换类的应用程序域。
        /// This is the application domain that is used to compile and run
        /// the generated transformation class to create the generated text output.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            //This host will provide a new application domain each time the 
            //engine processes a text template.
            //-------------------------------------------------------------
            return AppDomain.CreateDomain("Generation App Domain");

            //This could be changed to return the current appdomain, but new 
            //assemblies are loaded into this AppDomain on a regular basis.
            //If the AppDomain lasts too long, it will grow indefintely, 
            //which might be regarded as a leak.

            //This could be customized to cache the application domain for 
            //a certain number of text template generations (for example, 10).

            //This could be customized based on the contents of the text 
            //template, which are provided as a parameter for that purpose.
        }



        //允许主机提供有关程序集位置的附加信息。 
        //The engine calls this method to resolve assembly references used in
        //the generated transformation class project and for the optional 
        //assembly directive if the user has specified it in the text template.
        //This method can be called 0, 1, or more times.        
        public string ResolveAssemblyReference(string assemblyReference)
        {
            //If the argument is the fully qualified path of an existing file,then we are done. (This does not do any work.)            
            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }

            //Maybe the assembly is in the same folder as the text template that called the directive.            
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            //This can be customized to search specific paths for the file or to search the GAC.

            //This can be customized to accept paths to search as command line arguments.

            //If we cannot do better, return the original file name.
            return "";
        }


        //The engine calls this method based on the directives the user has 
        //specified in the text template.
        //This method can be called 0, 1, or more times.
        //---------------------------------------------------------------------
        public Type ResolveDirectiveProcessor(string processorName)
        {
            //This host will not resolve any specific processors.

            //Check the processor name, and if it is the name of a processor the 
            //host wants to support, return the type of the processor.
            //---------------------------------------------------------------------
            if (string.Compare(processorName, "XYZ", StringComparison.OrdinalIgnoreCase) == 0)
            {
                //return typeof();
            }
            //This can be customized to search specific paths for the file or to search the GAC

            //If the directive processor cannot be found, throw an error.
            throw new Exception("没有找到指令处理器");//Directive Processor not found
        }

        //If a call to a directive in a text template does not provide a value
        //for a required parameter, the directive processor can try to get it
        //from the host by calling this method.
        //This method can be called 0, 1, or more times.
        //---------------------------------------------------------------------
        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
            {
                throw new ArgumentNullException("the directiveId cannot be null");
            }
            if (processorName == null)
            {
                throw new ArgumentNullException("the processorName cannot be null");
            }
            if (parameterName == null)
            {
                throw new ArgumentNullException("the parameterName cannot be null");
            }

            //Code to provide "hard-coded" parameter values goes here.
            //This code depends on the directive processors this host will interact with.

            //If we cannot do better, return the empty string.
            return String.Empty;
        }



        //A directive processor can call this method if a file name does not 
        //have a path.
        //The host can attempt to provide path information by searching 
        //specific paths for the file and returning the file and path if found.
        //This method can be called 0, 1, or more times.
        //---------------------------------------------------------------------
        public string ResolvePath(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }

            //If the argument is the fully qualified path of an existing file,
            //then we are done
            //----------------------------------------------------------------
            if (File.Exists(fileName))
            {
                return fileName;
            }

            //Maybe the file is in the same folder as the text template that 
            //called the directive.
            //----------------------------------------------------------------
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            //Look more places.
            //----------------------------------------------------------------
            //More code can go here...

            //If we cannot do better, return the original file name.
            return fileName;
        }
        
        //The engine calls this method to change the extension of the 
        //generated text output file based on the optional output directive 
        //if the user specifies it in the text template.
        //---------------------------------------------------------------------
        public void SetFileExtension(string extension)
        {
            //The parameter extension has a '.' in front of it already.
            //--------------------------------------------------------
            _fileExtensionValue = extension;
        }


        //The engine calls this method to change the encoding of the 
        //generated text output file based on the optional output directive 
        //if the user specifies it in the text template.
        //----------------------------------------------------------------------
        public void SetOutputEncoding(System.Text.Encoding encoding, bool fromOutputDirective)
        {
            _fileEncodingValue = encoding;
        }

        #endregion


    }


}
