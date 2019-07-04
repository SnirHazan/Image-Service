using WebApplication2.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.IO;
using System.Threading;

namespace WebApplication2.Models
{
    public class ImageWeb
    {
        //public delegate void SomeThingWasChanged();
        //public event SomeThingWasChanged Changed;

        private static Communication.Client webClient;
        private static Models.Configuration Config1;
        public ImageWeb()
        {
            webClient = Communication.Client.GetInstance;
            webClient.RecivedMessageFromServer();
            Config1 = Configuration.GetInstance;
            NumberOfImgs = NumberOfImages();
            CommandRecievedEventArgs commanToSent = new CommandRecievedEventArgs((int)CommandStateEnum.GET_APP_CONFIG, new string[5], "");
            webClient.SendCommandToServer(commanToSent);
            List<Creator> student = InizilaizedCreator();
            Students = student;

        }

        public int NumberOfImages()
        {
            int counter = 0;
            int count = 0;
            while (Config1.OutputDir == "" && count < 3)
            {
                Thread.Sleep(1000);
                count++;
            }
            if (Config1.OutputDir != "")
            {
                DirectoryInfo di = new DirectoryInfo(Config1.OutputDir);
                counter += di.GetFiles("*.jpg", SearchOption.AllDirectories).Length;
                counter += di.GetFiles("*.gif", SearchOption.AllDirectories).Length;
                counter += di.GetFiles("*.bmp", SearchOption.AllDirectories).Length;
                counter += di.GetFiles("*.png", SearchOption.AllDirectories).Length;
            }

            return counter / 2;
        }
        public void CheckConnection()
        {
            webClient.IsClientConnected();
            Status = "Disconnected";
            if (webClient.IsConnected)
                Status = "Connected";
        }

        private List<Creator> InizilaizedCreator()
        {

            List<Creator> temp = new List<Creator>();
            String[] lines = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/Students.txt"));
            foreach (string line in lines)
            {
                string[] tmp = line.Split(';');
                Creator std = new Creator
                {
                    FirstName = tmp[0],
                    LastName = tmp[1],
                    ID = tmp[2]
                };
                temp.Add(std);
            }
            return temp;
        }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Number Of Images")]
        public int NumberOfImgs { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Creators")]
        public List<Creator> Students { get; set; }


        public class Creator
        {
            [Required]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "ID")]
            public string ID { get; set; }
        }

    }
}