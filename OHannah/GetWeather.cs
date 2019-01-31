using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;


namespace OHannah
{
    class GetWeather
    {
        private const string APIKEY = "4831c685cfa4a3bf1418c37f2190f841";
        private string URL;
        private XmlDocument xmlDocument;

        public GetWeather(string city)
        {
            this.City = city;
            this.CheckWeather();
        }

        public void CheckWeather()
        {
            URL = "http://api.openweathermap.org/data/2.5/weather?q=" + City + "&mode=xml&appid=" + APIKEY;
            xmlDocument = GetXML(URL);
            this.GetData();
            //MessageBox.Show(string.Format("Temp: {0}\tMax: {1}\tMin:{2}\tWind: {3}\tHumidity: {4}\tCloudes: {5}\t", Temp, TempMax, TempMin, Wind, Humidity,Clouds));
        }
        

        public string City { get;  set ; }
        public double Temp { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public string Humidity { get; set; }
        public string Wind { get; set; }
        public string Clouds { get; set; }
        //public string Visibility { get; set; }

        public void GetData()
        {
            XmlNode temp_node = null;
            try
            {
                double temp=0,max = 0, min = 0;
                temp_node = xmlDocument.SelectSingleNode("//temperature");
                double.TryParse((temp_node.Attributes["value"].Value), out temp);
                if (temp != 0)
                {
                    Temp = temp - 273;
                    Temp = Math.Round(Temp, 2);
                }
                double.TryParse((temp_node.Attributes["max"].Value),out max);
                if (max != 0)
                {
                    TempMax = max - 273;
                    TempMax = Math.Round(TempMax, 2);
                }
                double.TryParse((temp_node.Attributes["min"].Value),out min);
                if (min != 0)
                {
                    TempMin = min - 273;
                    TempMin = Math.Round(TempMin, 2);
                }
                temp_node = xmlDocument.SelectSingleNode("//humidity");
                Humidity = temp_node.Attributes["value"].Value;
                temp_node = xmlDocument.SelectSingleNode("//speed");
                Wind = temp_node.Attributes["value"].Value;
                temp_node = xmlDocument.SelectSingleNode("//clouds");
                Clouds = temp_node.Attributes["name"].Value;
                //temp_node = xmlDocument.SelectSingleNode("//visibility");
                //Visibility = temp_node.Attributes["value"].Value;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }



        }

        private XmlDocument GetXML(string CurrentURL)
        {

            try
            {
                using (WebClient client = new WebClient())
                {
                    string xmlContent = client.DownloadString(CurrentURL);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlContent);
                    return xmlDocument;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }

    
    }
}
