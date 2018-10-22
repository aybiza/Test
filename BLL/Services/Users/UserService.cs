using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using BLL.Models;

namespace BLL.Services.Users
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Récupérer un utilisateur à partir de soit son login soit son email ca dépend de paramètre Type
        /// </summary>
        /// <param name="Key">il contient la valeur de clé définie par le paramétre type</param>
        /// <param name="type">Le type de paramètre key (soit login soit mot de passe)</param>
        /// <returns>User c'est l'utilisateur exist, null sinon</returns>
        public User GetUser(string Key, string type)
        {            
            string xmlFile = File.ReadAllText(@"C:\Users\ZDR\Desktop\ApiAuthentification\ApiAuthentification\DAL\Data\Users.xml");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFile);

            XmlNode root = doc.FirstChild;
            var list = new List<User>();
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                if(((type == "Login" && root.ChildNodes[i].FirstChild.Name == "Login")
                    || (type == "Email" && root.ChildNodes[i].FirstChild.Name == "Email") )
                    && root.ChildNodes[i].FirstChild.InnerText.ToUpper() == Key.ToUpper())
                {
                    return new User()
                    {
                        Login = root.ChildNodes[i].FirstChild.InnerText,
                        Password = root.ChildNodes[i].ChildNodes[1].InnerText,
                        Email = root.ChildNodes[i].LastChild.InnerText

                    };
                }
            }

            return null;
        }
    }
}
