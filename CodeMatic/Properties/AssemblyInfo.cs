using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

// 有关程序集的常规信息通过下列属性集控制。更改这些属性值可修改与程序集关联的信息。
[assembly: AssemblyTitle("动软代码生成器")]
[assembly: AssemblyProduct("动软代码生成器")]
[assembly: AssemblyDescription("20140303")]
[assembly: AssemblyVersion("2.78")]
[assembly: AssemblyFileVersion("2.78")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("动软卓越（北京）科技有限公司")]
[assembly: AssemblyCopyright("Copyright(C)2004-2013 Maticsoft All Rights Reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 属性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("7188ba74-3917-40aa-8100-33cc8870933a")]






# region "Class to get the information for AboutForm"
/* This class uses the System.Reflection.Assembly class to access assembly meta-data. 
* This class is not a normal feature of AssmblyInfo.cs */
/// <summary> 
/// AssemblyInfo class. 
/// </summary> 
public class AssemblyInfo
{
    //Used by functions to access information from Assembly Attributes 
    /// <summary> 
    /// myType. 
    /// </summary> 
    private Type myType;
    /// <summary> 
    /// Initializes a new instance of the <see cref="AssemblyInfo"/> class. 
    /// </summary> 
    public AssemblyInfo()
    {
        //Shellform here denotes the actual form. 
        myType = typeof(Codematic.MainForm);
    }
    /// <summary> 
    /// Gets the name of the assembly. 
    /// </summary> 
    /// <value>The name of the assembly.</value> 
    public String AssemblyName
    {
        get
        {
            return myType.Assembly.GetName().Name.ToString();
        }
    }
    /// <summary> 
    /// Gets the full name of the assembly. 
    /// </summary> 
    /// <value>The full name of the assembly.</value> 
    public String AssemblyFullName
    {
        get
        {
            return myType.Assembly.GetName().FullName.ToString();
        }
    }
    /// <summary> 
    /// Gets the code base. 
    /// </summary> 
    /// <value>The code base.</value> 
    public String CodeBase
    {
        get
        {
            return myType.Assembly.CodeBase;
        }
    }
    /// <summary> 
    /// Gets the copyright. 
    /// </summary> 
    /// <value>The copyright.</value> 
    public String Copyright
    {
        get
        {
            Type att = typeof(AssemblyCopyrightAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyCopyrightAttribute copyattr = (AssemblyCopyrightAttribute)r[0];
            return copyattr.Copyright;
        }
    }
    /// <summary> 
    /// Gets the company. 
    /// </summary> 
    /// <value>The company.</value> 
    public String Company
    {
        get
        {
            Type att = typeof(AssemblyCompanyAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyCompanyAttribute compattr = (AssemblyCompanyAttribute)r[0];
            return compattr.Company;
        }
    }
    /// <summary> 
    /// Gets the description. 
    /// </summary> 
    /// <value>The description.</value> 
    public String Description
    {
        get
        {
            Type att = typeof(AssemblyDescriptionAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyDescriptionAttribute descattr = (AssemblyDescriptionAttribute)r[0];
            return descattr.Description;
        }
    }
    /// <summary> 
    /// Gets the product. 
    /// </summary> 
    /// <value>The product.</value> 
    public String Product
    {
        get
        {
            Type att = typeof(AssemblyProductAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyProductAttribute prodattr = (AssemblyProductAttribute)r[0];
            return prodattr.Product;
        }
    }
    /// <summary> 
    /// Gets the title. 
    /// </summary> 
    /// <value>The title.</value> 
    public String Title
    {
        get
        {
            Type att = typeof(AssemblyTitleAttribute);
            object[] r = myType.Assembly.GetCustomAttributes(att, false);
            AssemblyTitleAttribute titleattr = (AssemblyTitleAttribute)r[0];
            return titleattr.Title;
        }
    }
    /// <summary> 
    /// Gets the version. 
    /// </summary> 
    /// <value>The version.</value> 
    public String Version
    {
        get
        {
            return myType.Assembly.GetName().Version.ToString();
        }
    }
}
# endregion 