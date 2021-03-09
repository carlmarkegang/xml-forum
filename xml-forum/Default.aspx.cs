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

        public List<XmlNodeList> PostsList = new List<XmlNodeList>();

        public class MainPost
        {
            List<XmlNodeList> PostsList;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string filepath = HttpContext.Current.Server.MapPath("/Files/Posts/");
            DirectoryInfo d = new DirectoryInfo(filepath);
            

            foreach (DirectoryInfo folder in d.GetDirectories())
            {
                foreach (FileInfo file in folder.GetFiles("*.xml"))
                {
                    PostsList.Add(LoadPosts(file.FullName));
                }
            }



                    
       
            if (IsPostBack)
            {
                
                    CreateNewPostXml(Request.Params["text"]);
            }

               
        }

        public XmlNodeList LoadPosts(string filepath)
        {
            XmlDocument inxml = new XmlDocument();
            inxml.XmlResolver = null;
            inxml.Load(filepath);

            XmlNodeList returnlist = inxml.SelectNodes("Posts/Post");
            
            return returnlist;
        }


        public Boolean CreateNewPostXml(string InnerText)
        {
            var date_now = DateTime.Now;
            var date_now_string = date_now.ToString("yyyyMMddHHmmss");
            XmlDocument xdoc = new XmlDocument();

            XmlElement Posts = xdoc.CreateElement("Posts");
            XmlElement Post = xdoc.CreateElement("Post");
            Post.InnerText = InnerText;
            Post.SetAttribute("date", date_now_string);

            Posts.AppendChild(Post);
            xdoc.AppendChild(Posts);

            Encoding enc = Encoding.GetEncoding("utf-8");
            CreateFolderIfNotExists("/Files/ToProcess/1/");
            string savePath = HttpContext.Current.Server.MapPath("/Files/ToProcess/1/");
            string fileName = Guid.NewGuid().ToString() + ".xml";
            XmlTextWriter xwriter = new XmlTextWriter(savePath + fileName, enc);
            xdoc.Save(xwriter);
            xwriter.Close();

            return true;

        }

        public Boolean CreateFolderIfNotExists(string folderPath)
        {
            bool exists = System.IO.Directory.Exists(Server.MapPath(folderPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(folderPath));
            }
            return true;
        }
    }
}