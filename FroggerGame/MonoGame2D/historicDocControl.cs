using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace MonoGame2D
{
    public class HistoricDocControl
    {
        public HistoricDocControl()
        {

        }

        public List<PlayerDatas> getTopXPlayers(int numberOfTops, PlayerDatas PlayerToInsert)
        {
            List<PlayerDatas> historics = new List<PlayerDatas>();
            List<PlayerDatas> historicsToReturn = new List<PlayerDatas>();
            // insere ou atualiza
            /*if (existThePlayer(PlayerToInsert))
            {
                editPalyerHistoric(PlayerToInsert);
            }
            else
            {
                addPlayerToHistoric(PlayerToInsert);
            }*/
            historics = getPlayersHistoric();
            historics.Add(PlayerToInsert);
            historics = sort(historics);
            int control;
            if (numberOfTops > historics.Count())
            {
                control = historics.Count();
            }
            else
            {
                control = numberOfTops;
            }
            for(int i = 0 ; i < control ; i++)
            {
                historicsToReturn.Add(historics[i]);
            }
            return historicsToReturn;
        }

        private static List<PlayerDatas> getPlayersHistoric()
        {
            List<PlayerDatas> historics = new List<PlayerDatas>();
            XElement xml = XElement.Load("PlayersHistoric.xml");
            foreach (XElement x in xml.Elements())
            {
                PlayerDatas p = new PlayerDatas();
                p.setName(x.Attribute("name").Value);
                p.setPoints(int.Parse(x.Attribute("points").Value));
                historics.Add(p);
            }
            return historics;
        }

        private static void addPlayerToHistoric(PlayerDatas p)
        {
            XElement x = new XElement("player");
            x.Add(new XAttribute("name", p.getName()));
            x.Add(new XAttribute("points", p.getPoints().ToString()));
            XElement xml = XElement.Load("PlayersHistoric.xml");
            xml.Add(x);
            byte[] byteArray = Encoding.ASCII.GetBytes(xml.ToString());
            MemoryStream stream = new MemoryStream(byteArray);
            xml.Save(stream);
        }

        private static void editPalyerHistoric(PlayerDatas player)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes("PlayersHistoric.xml");
            MemoryStream stream = new MemoryStream(byteArray);
            XElement xml = XElement.Load("PlayersHistoric.xml");
            XElement x = xml.Elements().Where(p => p.Attribute("name").Value.Equals(player.getName())).FirstOrDefault();
            if (x != null)
            {
               if ((int.Parse(x.Attribute("points").Value)) < player.getPoints())
               {
                   x.Attribute("points").SetValue(player.getPoints().ToString());
               }
               xml.Save(stream);
            }
        }

        private static List<PlayerDatas> sort(List<PlayerDatas> list)
        {
            PlayerDatas temp = new PlayerDatas();
            for (int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list.Count() - 1; j++)
                {
                    if (list[j].getPoints() < list[j + 1].getPoints())
                    {
                        temp = list[j + 1];
                        list[j + 1] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }

        private static bool existThePlayer(PlayerDatas player)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes("PlayersHistoric.xml");
            MemoryStream stream = new MemoryStream(byteArray);
            XElement xml = XElement.Load("PlayersHistoric.xml");
            XElement x = xml.Elements().Where(p => p.Attribute("name").Value.Equals(player.getName())).FirstOrDefault();
            if (x != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        ~HistoricDocControl()
        {

        }
    }
}
