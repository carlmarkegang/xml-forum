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

        static FileInfo myFile;
        static string filepath_full;

        static void Main(string[] args)
        {
            filepath_full = args[0];
            DirectoryInfo d = new DirectoryInfo(filepath_full + "/files/");
            myFile = (from FileInfo f in d.GetFiles() orderby f.LastWriteTime descending select f).First();
            ProcessFiles();
            File.Move(myFile.FullName, filepath_full + "/Files/Archive/" + myFile.Name);
        }


        static Boolean ProcessFiles()
        {
            string filepath = filepath_full + "/files/ToProcess/";
            DirectoryInfo d = new DirectoryInfo(filepath);
            var PostsList = new List<string>();

            foreach (var file in d.GetFiles("*.xml"))
            {

                XmlNodeList Posts = LoadPosts(file.FullName);
                foreach (XmlNode Post in Posts)
                {
                    PostsList.Add(Post.InnerText);
                }
                File.Delete(file.FullName);

            }

            CreateNewPostXml(filepath, PostsList);


            return true;
        }

        static Boolean CreateNewPostXml(string filename, List<string> Posts)
        {
            XmlDocument inxml = new XmlDocument();
            inxml.XmlResolver = null;
            inxml.Load(myFile.FullName);

            XmlNode PostsNode = inxml.SelectSingleNode("Posts");
            foreach (string Post in Posts)
            {
                XmlElement create_post = inxml.CreateElement("Post");
                create_post.InnerText = Post;
                PostsNode.AppendChild(create_post);
            }

            Encoding enc = Encoding.GetEncoding("utf-8");

            string savePath = filepath_full + "/Files/";
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

    }
}
