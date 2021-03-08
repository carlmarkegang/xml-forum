using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace xml_forum
{

    public partial class _Default : Page
    {

        public XmlNodeList Posts;
        public FileInfo myFile;
        protected void Page_Load(object sender, EventArgs e)
        {
            string filepath = HttpContext.Current.Server.MapPath("/Files/");
            DirectoryInfo d = new DirectoryInfo(filepath);
            if (d.GetFiles().Length > 0)
            {
                myFile = (from f in d.GetFiles() orderby f.LastWriteTime descending select f).First();
                Posts = LoadPosts(myFile.FullName);
            }

            

            CreateXml();


          
        }

        public XmlNodeList LoadPosts(string filepath)
        {
            XmlDocument inxml = new XmlDocument();
            inxml.XmlResolver = null;
            inxml.Load(filepath);

            XmlNodeList returnlist = inxml.SelectNodes("Posts/Post");
            
            return returnlist;
        }


        public Boolean CreateXml()
        {
            var date_now = DateTime.Now;
            var date_now_string = date_now.ToString("yyyyMMddHHmmss");
            XmlDocument xdoc = new XmlDocument();

            XmlElement Posts = xdoc.CreateElement("Posts");
            XmlElement Post = xdoc.CreateElement("Post");
            Post.InnerText = Guid.NewGuid().ToString();

            Posts.AppendChild(Post);
            xdoc.AppendChild(Posts);

            Encoding enc = Encoding.GetEncoding("utf-8");
            string savePath = HttpContext.Current.Server.MapPath("/Files/ToProcess/");
            string fileName = Guid.NewGuid().ToString() + ".xml";
            XmlTextWriter xwriter = new XmlTextWriter(savePath + fileName, enc);
            xdoc.Save(xwriter);
            xwriter.Close();

            return true;

        }
    }
}