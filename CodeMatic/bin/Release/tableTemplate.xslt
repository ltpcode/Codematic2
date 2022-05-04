<?xml version="1.0" encoding="UTF-8" ?>
 <xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" omit-xml-declaration="yes" indent="yes" />
    <xsl:template match="/Schema">namespace test
{
      public class <xsl:apply-templates select="//TableName" />
      {

        //变量
        <xsl:for-each select="FIELDS/FIELD">
          <xsl:value-of select="@Type" /> m_<xsl:value-of select="@Name" /> ; 
        </xsl:for-each>

      public void New()
      {
      }

      public void New(<xsl:for-each select="PrimaryKeys/FIELD">
        <xsl:if test="position()!=count(../*)"><xsl:value-of select="@Type" /> p<xsl:value-of select="@Name" />,</xsl:if>
        <xsl:if test="position() = count(../*)"><xsl:value-of select="@Type" /> p<xsl:value-of select="@Name" /></xsl:if>
        </xsl:for-each>)
        {
          <xsl:for-each select="PrimaryKeys/FIELD">
            m_<xsl:value-of select="@Name" /> = p<xsl:value-of select="@Name" />
          </xsl:for-each>
      }

      public void New(<xsl:for-each select="FIELDS/FIELD">
        <xsl:if test="position()!=count(../*)"><xsl:value-of select="@Type" /> p<xsl:value-of select="@Name" /> ,</xsl:if>
        <xsl:if test="position() = count(../*)"><xsl:value-of select="@Type" /> p<xsl:value-of select="@Name" /></xsl:if>
      </xsl:for-each>)
      {
        <xsl:for-each select="FIELDS/FIELD">
            m_<xsl:value-of select="@Name" /> = p<xsl:value-of select="@Name" />
        </xsl:for-each>
        }
        
      <xsl:for-each select="FIELDS/FIELD">
        public <xsl:value-of select="@Type" /> <xsl:value-of select="@Name" />
        {
            get
            {
                return m_<xsl:value-of select="@Name" />;
            }
            set
            {                        
                m_<xsl:value-of select="@Name" /> = Value;
            }
        }
        </xsl:for-each>
      
    }
}</xsl:template>
    <xsl:template match="//TableName">
        <xsl:value-of select="@value" />
    </xsl:template>
</xsl:stylesheet>