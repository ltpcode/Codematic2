<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
       <xsl:output method="xml" omit-xml-declaration="yes" indent="yes" />
      <xsl:template match="/Schema">
        Imports System.Data.SqlClient
        Imports APJ.Common.Data
  
  Namespace Modules.YourModule
      Public Class do<xsl:apply-templates select="//TableName" />
          Implements IDisposable
         
         #Region "feilds constants"
         <xsl:for-each select="FIELDS/FIELD">Public Const FIELD_<xsl:value-of select="@Name" /> As String = "<xsl:value-of select="@Name" />"
         </xsl:for-each>#End Region
         
         #Region "Command constants"
         <xsl:for-each select="Query/Command">Private Const Command_<xsl:value-of select="@Name" /> As String = "<xsl:value-of select="@Name" />"
         </xsl:for-each>
         <xsl:for-each select="Insert/Command">Private Const Command_<xsl:value-of select="@Name" /> As String = "<xsl:value-of select="@Name" />"
         </xsl:for-each>
         <xsl:for-each select="Update/Command">Private Const Command_<xsl:value-of select="@Name" /> As String = "<xsl:value-of select="@Name" />"
         </xsl:for-each>
         <xsl:for-each select="Delete/Command">Private Const Command_<xsl:value-of select="@Name" /> As String = "<xsl:value-of select="@Name" />"
         </xsl:for-each>#End Region
 
         #Region "构造"
         '----------------------------------------------------------------
         ' Sub New:
         '   Constructor. 
         '----------------------------------------------------------------
         Public Sub New()
         End Sub
 
         '----------------------------------------------------------------
         ' Sub Dispose:
         '     Dispose of this object's resources.
         '----------------------------------------------------------------
         Public Sub Dispose() Implements IDisposable.Dispose
             GC.SuppressFinalize(True) ' as a service to those who might inherit from us
         End Sub
         #End Region
         
         #Region "Read <xsl:apply-templates select="//TableName" />"<xsl:for-each select="Query/Command">
             <xsl:choose>
                 <xsl:when test="@ReturnMode='Multiple'">
         Public Function get<xsl:apply-templates select="//TableName" />
         <xsl:choose>
                 <xsl:when test="count(*)>0">By</xsl:when>
                 <xsl:otherwise>All</xsl:otherwise>
         </xsl:choose>
         <xsl:for-each select="FIELD">
                         <xsl:value-of select="@Name" />
                     </xsl:for-each>(<xsl:for-each select="FIELD">
                         <xsl:if test="position()!=count(../*)">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" />, </xsl:if>
                         <xsl:if test="position() = count(../*)">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" /></xsl:if>
                     </xsl:for-each>) As dsEntity<xsl:apply-templates select="//TableName" />
             Dim ds<xsl:apply-templates select="//TableName" /> As New dsEntity<xsl:apply-templates select="//TableName" />
 
             'get the sql statement
             Dim sql As SqlType
 
             sql = DataAccessConfiguration.GetSQL(Me.GetType.Name, Command_<xsl:value-of select="@Name" />)
         <xsl:for-each select="FIELD">    sql.setValue(FIELD_<xsl:value-of select="@Name" />, p<xsl:value-of select="@Name" />)
         </xsl:for-each>
             SQLDBObject.RunProcedureByRef(sql, sql.getMapping(), ds<xsl:apply-templates select="//TableName" />, ds<xsl:apply-templates select="//TableName" />.Table_Name)
 
             Return ds<xsl:apply-templates select="//TableName" />
         End Function
 </xsl:when>
                 <xsl:otherwise>
         Public Function get<xsl:apply-templates select="//TableName" />
         <xsl:choose>
                 <xsl:when test="count(*)>0">By</xsl:when>
                 <xsl:otherwise>All</xsl:otherwise>
         </xsl:choose>
     <xsl:for-each select="FIELD">
                         <xsl:value-of select="@Name" />
                     </xsl:for-each>(<xsl:for-each select="FIELD">
                         <xsl:if test="position()!=last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" />, </xsl:if>
                         <xsl:if test="position() = last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" /></xsl:if>
                     </xsl:for-each>) As Entity<xsl:apply-templates select="//TableName" />
             Dim i<xsl:apply-templates select="//TableName" /> As New Entity<xsl:apply-templates select="//TableName" />
 
             'get the sql statement
             Dim sql As SqlType
 
             sql = DataAccessConfiguration.GetSQL(Me.GetType.Name, Command_<xsl:value-of select="@Name" />)
         <xsl:for-each select="FIELD">    sql.setValue(FIELD_<xsl:value-of select="@Name" />, p<xsl:value-of select="@Name" />)
         </xsl:for-each>
             i<xsl:apply-templates select="//TableName" /> = SQLDBObject.RunProcedureReturnObject(sql, sql.getMapping(), GetType(Entity<xsl:apply-templates select="//TableName" />))
 
             Return i<xsl:apply-templates select="//TableName" />
         End Function
                 </xsl:otherwise>
             </xsl:choose>
         </xsl:for-each>        #End Region
         
         #Region "Insert <xsl:apply-templates select="//TableName" />"<xsl:for-each select="Insert/Command">
         Public Sub <xsl:value-of select="@Name" />(<xsl:for-each select="FIELD">
                         <xsl:if test="position()!=last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" />, </xsl:if>
                        <xsl:if test="position() = last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" /></xsl:if>
                    </xsl:for-each>)
            'get the sql statement
            Dim sql As SqlType
            sql = DataAccessConfiguration.GetSQL(Me.GetType.Name, Command_<xsl:value-of select="@Name" />)
        <xsl:for-each select="FIELD">    sql.setValue(FIELD_<xsl:value-of select="@Name" />, p<xsl:value-of select="@Name" />)
        </xsl:for-each>
            SQLDBObject.RunProcedure(sql, sql.getMapping())
        End Sub
</xsl:for-each>        #End Region
        
        #Region "Update <xsl:apply-templates select="//TableName" />"<xsl:for-each select="Update/Command">
        Public Sub <xsl:value-of select="@Name" />(<xsl:for-each select="FIELD">
                        <xsl:if test="position()!=last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" />, </xsl:if>
                        <xsl:if test="position() = last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" /></xsl:if>
                    </xsl:for-each>)
            'get the sql statement
            Dim sql As SqlType
            sql = DataAccessConfiguration.GetSQL(Me.GetType.Name, Command_<xsl:value-of select="@Name" />)
        <xsl:for-each select="FIELD">    sql.setValue(FIELD_<xsl:value-of select="@Name" />, p<xsl:value-of select="@Name" />)
        </xsl:for-each>
            SQLDBObject.RunProcedure(sql, sql.getMapping())
        End Sub
</xsl:for-each>        #End Region
        
        #Region "Delete <xsl:apply-templates select="//TableName" />"<xsl:for-each select="Delete/Command">
        Public Sub <xsl:value-of select="@Name" />(<xsl:for-each select="FIELD">
                        <xsl:if test="position()!=last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" />, </xsl:if>
                        <xsl:if test="position() = last()">Byval p<xsl:value-of select="@Name" /> as <xsl:value-of select="@Type" /></xsl:if>
                    </xsl:for-each>)
            'get the sql statement
            Dim sql As SqlType
            sql = DataAccessConfiguration.GetSQL(Me.GetType.Name, Command_<xsl:value-of select="@Name" />)
        <xsl:for-each select="FIELD">    sql.setValue(FIELD_<xsl:value-of select="@Name" />, p<xsl:value-of select="@Name" />)
        </xsl:for-each>
            SQLDBObject.RunProcedure(sql, sql.getMapping())
        End Sub
</xsl:for-each>        #End Region
    End Class
End Namespace</xsl:template>
    <xsl:template match="//TableName">
        <xsl:value-of select="@value" />
    </xsl:template>

  
</xsl:stylesheet>