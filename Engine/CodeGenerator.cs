using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TextTemplating;
using Maticsoft.CodeHelper;
namespace Maticsoft.CodeEngine
{
    public class CodeGenerator
    {
        

        /// <summary>
        /// 根据模板内容，生成代码
        /// </summary>
        /// <param name="input">模板内容</param>
        /// <param name="host">模板主机</param>
        /// <param name="errorMsg">编译错误信息</param>
        /// <returns>生成代码</returns>
        public CodeInfo GenerateCode(string input, TemplateHost host)
        {
            CodeInfo codeinfo = new CodeInfo();
            Engine engine = new Engine();
            codeinfo.Code = engine.ProcessTemplate(input, host);
            StringBuilder errmsg=new StringBuilder();
            foreach (CompilerError error in host.ErrorCollection)
            {
                errmsg.AppendLine(error.ToString());
            }
            codeinfo.ErrorMsg = errmsg.ToString();
            codeinfo.FileExtension = host.FileExtension;
            
            return codeinfo;
        }

        /// <summary>
        /// 批量生成多个模板的代码
        /// </summary>
        /// <param name="args"></param>
        public CodeInfo BatchGenerateCode(string[] templatefile)
        {
            #region 判断模板文件

            string templateFileName = null;
            if (templatefile.Length == 0)
            {
                throw new System.Exception("you must provide a text template file path");
            }
            templateFileName = templatefile[0];
            if (templateFileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }
            if (!File.Exists(templateFileName))
            {
                throw new FileNotFoundException("the file cannot be found");
            }
            #endregion

            TemplateHost host = new TemplateHost();
            host.TemplateFile = templateFileName;

            //Read the text template.
            string input = File.ReadAllText(templateFileName);
                        
            return GenerateCode(input, host);


            //string outputFileName = Path.GetFileNameWithoutExtension(templateFileName);
            //outputFileName = Path.Combine(Path.GetDirectoryName(templateFileName), outputFileName);
            //outputFileName = outputFileName + "1" + host.FileExtension;
            //File.WriteAllText(outputFileName, outputCode, host.FileEncoding);
            //foreach (CompilerError error in host.ErrorCollection)
            //{
            //    File.WriteAllText(outputFileName, error.ToString(), host.FileEncoding);
            //}
        }
    }
}
