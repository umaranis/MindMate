<?xml version="1.0"?>
<!-- This xsl is not used now. Replaced by T4 Template.-->

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
        <xsl:for-each select="Application/Application.Commands/Command">
          
          <xsl:value-of select="@Name"/> = <xsl:value-of select="@Id"/>,
      </xsl:for-each>
    
</xsl:template>

</xsl:stylesheet>
