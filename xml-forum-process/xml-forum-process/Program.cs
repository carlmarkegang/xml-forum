using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace xml_forum_process
{
    class Program
    {
        static string filepath_full;

        static void Main(string[] args)
        {
            filepath_full = args[0];
            ProcessFiles();
        }


        static Boolean ProcessFiles()
        {
            string filepath = filepath_full + "/files/ToProcess/";
            DirectoryInfo d = new DirectoryInfo(filepath);
            var PostsList = new List<string>();

            foreach (DirectoryInfo folder in d.GetDirectories())
            {
                foreach (FileInfo file in folder.GetFiles("*.xml"))
                {

                    XmlNodeList Posts = LoadPosts(file.FullName);
                    foreach (XmlNode Post in Posts)
                    {
                        PostsList.Add(Post.InnerText);
                    }
                    

                    CreateBaseXml(folder.Name);
                    AddNewPostsToXml(folder.Name, PostsList);
                    File.Delete(file.FullName);
                }

          
            }



            return true;
        }

        static Boolean AddNewPostsToXml(string folderName, List<string> Posts)
        {
            XmlDocument inxml = new XmlDocument();
            inxml.XmlResolver = null;
            string savePath = filepath_full + "/Files/Posts/" + folderName + "/";
            DirectoryInfo d = new DirectoryInfo(savePath);
            FileInfo filename = (from f in d.GetFiles() orderby f.LastWriteTime descending select f).First();
            inxml.Load(savePath + filename.Name);

            XmlNode PostsNode = inxml.SelectSingleNode("Posts");
            foreach (string Post in Posts)
            {
                XmlElement create_post = inxml.CreateElement("Post");
                create_post.InnerText = Post;
                PostsNode.AppendChild(create_post);
            }

            Encoding enc = Encoding.GetEncoding("utf-8");

            XmlTextWriter xwriter = new XmlTextWriter(savePath + filename.Name, enc);
            inxml.Save(xwriter);
            xwriter.Close();

            return true;

        }

        static Boolean CreateBaseXml(string folderName)
        {
            if (System.IO.Directory.Exists(filepath_full + "/Files/Posts/" + folderName + "/"))
            {
                return true;
            }
            XmlDocument inxml = new XmlDocument();
            inxml.XmlResolver = null;

            XmlElement create_posts = inxml.CreateElement("Posts");
            inxml.AppendChild(create_posts);


            Encoding enc = Encoding.GetEncoding("utf-8");

            string savePath = filepath_full + "/Files/Posts/" + folderName + "/";
            CreateFolderIfNotExists(savePath);

            string fileName = Guid.NewGuid().ToString() + ".xml";
            XmlTextWriter xwriter = new XmlTextWriter(savePath + fileName, enc);
            inxml.Save(xwriter);
            xwriter.Close();

            return true;
        }

        static XmlNodeList LoadPosts(string filepath)
        {
            XmlDocument inxml = new XmlDocument();
            inxml.XmlResolver = null;
            inxml.Load(filepath);

            XmlNodeList returnlist = inxml.SelectNodes("Posts/Post");

            return returnlist;
        }

        static Boolean CreateFolderIfNotExists(string folderPath)
        {
            bool exists = System.IO.Directory.Exists(folderPath);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return true;
        }

    }
}
