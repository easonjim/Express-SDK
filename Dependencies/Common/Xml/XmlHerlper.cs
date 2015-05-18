/********************************************************************************** 
* 
* 功能说明:XML处理基类 
* 作者: 刘功勋; 
* 版本:V0.1(C#2.0);时间:2006-12-13 
* 
* *******************************************************************************/ 
using System; 
using System.Data; 
using System.IO; 
using System.Xml; 
namespace ComLib
{
    /// <summary> 
    /// XML 操作基类 
    /// </summary> 
    public class XmlHerlper : IDisposable
    {
        //以下为单一功能的静态类 
        #region 读取XML到DataSet
        /************************************************** 
* 函数名称:GetXml(string XmlPath) 
* 功能说明:读取XML到DataSet 
* 参 数:XmlPath:xml文档路径 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDnsConfig/DnsConfig.xml"); //获取xml路径 
* DataSet ds = EC.XmlObject.GetXml(xmlPath); //读取xml到DataSet中 
************************************************/
        /// <summary> 
        /// 功能:读取XML到DataSet中 
        /// </summary> 
        /// <param name="XmlPath">xml路径</param> 
        /// <returns>DataSet</returns> 
        public static DataSet GetXml( string XmlPath )
        {
            DataSet ds = new DataSet();
            ds.ReadXml( @XmlPath );
            return ds;
        }
        #endregion

        #region 读取xml文档并返回一个节点
        /************************************************** 
* 函数名称:ReadXmlReturnNode(string XmlPath,string Node) 
* 功能说明:读取xml文档并返回一个节点:适用于一级节点 
* 参 数: XmlPath:xml文档路径;Node 返回的节点名称 
* 适应用Xml:<?xml version="1.0" encoding="utf-8" ?> 
* <root> 
* <dns1>ns1.everdns.com</dns1> 
* </root> 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDnsConfig/DnsConfig.xml"); //获取xml路径 
* Response.Write(XmlObject.ReadXmlReturnNode(xmlPath, "mailmanager")); 
************************************************/
        /// <summary> 
        /// 读取xml文档并返回一个节点:适用于一级节点 
        /// </summary> 
        /// <param name="XmlPath">xml路径</param> 
        /// <param name="NodeName">节点</param> 
        /// <returns></returns> 
        public static string ReadXmlReturnNode( string XmlPath, string Node )
        {
            XmlDocument docXml = new XmlDocument();
            docXml.Load( @XmlPath );
            XmlNodeList xn = docXml.GetElementsByTagName( Node );
            if(xn.Count>0){
            return xn.Item( 0 ).InnerText.ToString();
            }
            else
            {
             return "";
            }
        }
        #endregion

        #region 查找数据,返回一个DataSet
        /************************************************** 
* 函数名称:GetXmlData(string xmlPath, string XmlPathNode) 
* 功能说明:查找数据,返回当前节点的所有下级节点,填充到一个DataSet中 
* 参 数:xmlPath:xml文档路径;XmlPathNode:当前节点的路径 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
* DataSet ds = new DataSet(); 
* ds = XmlObject.GetXmlData(xmlPath, "root/items");//读取当前路径 
* this.GridView1.DataSource = ds; 
* this.GridView1.DataBind(); 
* ds.Clear(); 
* ds.Dispose(); 
* Xml示例: 
* <?xml version="1.0" encoding="utf-8" ?> 
* <root> 
* <items name="xinnet"> 
* <url>http://www.paycenter.com.cn/cgi-bin/</url> 
* <port>80</port> 
* </items> 
* </root> 
************************************************/
        /// <summary> 
        /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中 
        /// </summary> 
        /// <param name="xmlPath">xml文档路径</param> 
        /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param> 
        /// <returns></returns> 
        public static DataSet GetXmlData( string xmlPath, string XmlPathNode )
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load( xmlPath );
            DataSet ds = new DataSet();
            StringReader read = new StringReader( objXmlDoc.SelectSingleNode( XmlPathNode ).OuterXml );
            ds.ReadXml( read );
            return ds;
        }

        #endregion

        #region 更新Xml节点内容
        /************************************************** 
* 函数名称:XmlNodeReplace(string xmlPath,string Node,string Content) 
* 功能说明:更新Xml节点内容 
* 参 数:xmlPath:xml文档路径;Node:当前节点的路径;Content:内容 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
* XmlObject.XmlNodeReplace(xmlPath, "root/test", "56789"); //更新节点内容 
************************************************/
        /// <summary> 
        /// 更新Xml节点内容 
        /// </summary> 
        /// <param name="xmlPath">xml路径</param> 
        /// <param name="Node">要更换内容的节点:节点路径 根节点/父节点/当前节点</param> 
        /// <param name="Content">新的内容</param> 
        public static void XmlNodeReplace( string xmlPath, string Node, string Content )
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load( xmlPath );
            objXmlDoc.SelectSingleNode( Node ).InnerText = Content;
            objXmlDoc.Save( xmlPath );
        }
        #endregion

        #region 删除XML节点和此节点下的子节点
        /************************************************** 
* 函数名称:XmlNodeDelete(string xmlPath,string Node) 
* 功能说明:删除XML节点和此节点下的子节点 
* 参 数:xmlPath:xml文档路径;Node:当前节点的路径; 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
* XmlObject.XmlNodeDelete(xmlPath, "root/test"); //删除当前节点 
************************************************/
        /// <summary> 
        /// 删除XML节点和此节点下的子节点 
        /// </summary> 
        /// <param name="xmlPath">xml文档路径</param> 
        /// <param name="Node">节点路径</param> 
        public static void XmlNodeDelete( string xmlPath, string Node )
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load( xmlPath );
            string mainNode = Node.Substring( 0, Node.LastIndexOf( "/" ) );
            objXmlDoc.SelectSingleNode( mainNode ).RemoveChild( objXmlDoc.SelectSingleNode( Node ) );
            objXmlDoc.Save( xmlPath );
        }
        #endregion

        #region 插入一个节点和此节点的字节点
        /************************************************** 
* 函数名称:XmlInsertNode(string xmlPath, string MailNode, string ChildNode, string Element,string Content) 
* 功能说明:插入一个节点和此节点的字节点 
* 参 数:xmlPath:xml文档路径;MailNode:当前节点的路径;ChildNode:新插入的节点;Element:插入节点的子节点;Content:子节点的内容 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
* XmlObject.XmlInsertNode(xmlPath, "root/test","test1","test2","测试内容"); //插入一个节点和此节点的字节点 
* 生成的XML格式为 
* <test> 
* <test1> 
* <test2>测试内容</test2> 
* </test1> 
* </test> 
************************************************/
        /// <summary> 
        /// 插入一个节点和此节点的字节点 
        /// </summary> 
        /// <param name="xmlPath">xml路径</param> 
        /// <param name="MailNode">当前节点路径</param> 
        /// <param name="ChildNode">新插入节点</param> 
        /// <param name="Element">插入节点的子节点</param> 
        /// <param name="Content">子节点的内容</param> 
        public static void XmlInsertNode( string xmlPath, string MailNode, string ChildNode, string Element, string Content )
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load( xmlPath );
            XmlNode objRootNode = objXmlDoc.SelectSingleNode( MailNode );
            XmlElement objChildNode = objXmlDoc.CreateElement( ChildNode );
            objRootNode.AppendChild( objChildNode );
            XmlElement objElement = objXmlDoc.CreateElement( Element );
            objElement.InnerText = Content;
            objChildNode.AppendChild( objElement );
            objXmlDoc.Save( xmlPath );
        }

        /// <summary>
        /// 插入一个节点和此节点的多个子节点 
        /// </summary>
        /// <param name="xmlPath">xml路径</param> 
        /// <param name="MailNode">当前节点路径</param> 
        /// <param name="ChildNode">新插入节点</param> 
        /// <param name="ElementInfo">子节点信息</param>
        ///  <param name="TableName">表名 作为属性</param>
        public static void XmlInsertNodeForTable( string xmlPath, string MailNode, string ChildNode, DataTable ElementInfo,string TableName )
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load( xmlPath );
            XmlNode objRootNode = objXmlDoc.SelectSingleNode( MailNode );

            //XmlElement objChildNode = objXmlDoc.CreateElement( ChildNode );
            //objRootNode.AppendChild( objChildNode );
            /************************************
             这里XML文档出来的格式是 
              <FromInfo>
              <TableName></TableName>
              <ControlName>标题</ControlName>
              <BoundColumn>标题</BoundColumn>
              <ControlType>text</ControlType>
              <IsList>N</IsList>
              <SQL></SQL>
              </FromInfo>
             
             */
            
            for( int i = 0; i < ElementInfo.Rows.Count; i++ ) {
                XmlElement ObjNextChildNode = objXmlDoc.CreateElement( "FromInfo" );
                objRootNode.AppendChild( ObjNextChildNode );
                XmlElement objElement = objXmlDoc.CreateElement("TableName");
                objElement.InnerText = TableName;
                ObjNextChildNode.AppendChild( objElement );
                for( int Num = 0; Num < ElementInfo.Columns.Count; Num++ ) {

                    objElement = objXmlDoc.CreateElement( ElementInfo.Columns[Num].ColumnName );
                    objElement.InnerText = ElementInfo.Rows[i][ElementInfo.Columns[Num].ColumnName].ToString();

                    ObjNextChildNode.AppendChild( objElement );
                }
            }
            objXmlDoc.Save( xmlPath );
        }

        #endregion

        #region 插入一节点,带一属性
        /************************************************** 
* 函数名称:XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content) 
* 功能说明:插入一节点,带一属性 
* 参 数:xmlPath:xml文档路径;MailNode:当前节点的路径;Element:新插入的节点;Attrib:属性名称;AttribContent:属性值;Content:节点的内容 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
* XmlObject.XmlInsertElement(xmlPath, "root/test", "test1", "Attrib", "属性值", "节点内容"); //插入一节点,带一属性 
* 生成的XML格式为 
* <test> 
* <test1 Attrib="属性值">节点内容</test1> 
* </test> 
************************************************/
        /// <summary> 
        /// 插入一节点,带一属性 
        /// </summary> 
        /// <param name="xmlPath">Xml文档路径</param> 
        /// <param name="MainNode">当前节点路径</param> 
        /// <param name="Element">新节点</param> 
        /// <param name="Attrib">属性名称</param> 
        /// <param name="AttribContent">属性值</param> 
        /// <param name="Content">新节点值</param> 
        public static void XmlInsertElement( string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content )
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load( xmlPath );
            XmlNode objNode = objXmlDoc.SelectSingleNode( MainNode );
            XmlElement objElement = objXmlDoc.CreateElement( Element );
            objElement.SetAttribute( Attrib, AttribContent );
            objElement.InnerText = Content;
            objNode.AppendChild( objElement );
            objXmlDoc.Save( xmlPath );
        }
        #endregion

        #region 插入一节点不带属性
        /************************************************** 
* 函数名称:XmlInsertElement(string xmlPath, string MainNode, string Element, string Content) 
* 功能说明:插入一节点不带属性 
* 参 数:xmlPath:xml文档路径;MailNode:当前节点的路径;Element:新插入的节点;Content:节点的内容 
* 使用示列: 
* using EC; //引用命名空间 
* string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
* XmlObject.XmlInsertElement(xmlPath, "root/test", "text1", "节点内容"); //插入一节点不带属性 
* 生成的XML格式为 
* <test> 
* <text1>节点内容</text1> 
* </test> 
************************************************/
        public static void XmlInsertElement( string xmlPath, string MainNode, string Element, string Content )
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load( xmlPath );
            XmlNode objNode = objXmlDoc.SelectSingleNode( MainNode );
            XmlElement objElement = objXmlDoc.CreateElement( Element );
            objElement.InnerText = Content;
            objNode.AppendChild( objElement );
            objXmlDoc.Save( xmlPath );
        }

       
        #endregion

        //必须创建对象才能使用的类 
        private bool _alreadyDispose = false;
        private string xmlPath;
        private XmlDocument xmlDoc = new XmlDocument();
        private XmlNode xmlNode;
        private XmlElement xmlElem;

        #region 构造与释构
        public XmlHerlper()
        {
        }
        ~XmlHerlper()
        {
            Dispose();
        }
        protected virtual void Dispose( bool isDisposing )
        {
            if( _alreadyDispose ) return;
            if( isDisposing ) {
                // 
            }
            _alreadyDispose = true;
        }
        #endregion
        #region IDisposable 成员
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
        #endregion
    }
}
