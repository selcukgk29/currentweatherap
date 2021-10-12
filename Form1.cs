using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Globalization;
using System.Timers;


namespace HavaDurumu
{
    public partial class Form1 : Form
    {
        const string appid = "4382fe876b8b8c686710424a0c5ca6ff";
        string sehir = "istanbul";

        public Form1()
        {
            InitializeComponent();
            getforcast(sehir);          
          
        }     
        DateTime getDate(double millisecound)
        {

            DateTime day = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(millisecound).ToLocalTime();
            return day;
        }

        public static string karakterCevir(string kelime)
        {
            string mesaj = kelime;
            char[] oldValue = new char[] { 'ö', 'Ö', 'ü', 'Ü', 'ç', 'Ç', 'İ', 'ı', 'Ğ', 'ğ', 'Ş', 'ş', ' ' };
            char[] newValue = new char[] { 'o', 'O', 'u', 'U', 'c', 'C', 'I', 'i', 'G', 'g', 'S', 's', '_' };
            for (int sayac = 0; sayac < oldValue.Length; sayac++)
            {
                mesaj = mesaj.Replace(oldValue[sayac], newValue[sayac]);
            }
            return mesaj;
        }

        void getforcast(string sehir)
        {

            using (WebClient web = new System.Net.WebClient())
            {
                try
                {
                    string url = string.Format("http://api.weatherapi.com/v1/current.json?key=761f8471d71b4400ba5101704210910&q={0}&aqi=no", sehir);
                    var json = web.DownloadString(url);
                    var Object = JsonConvert.DeserializeObject<Root>(json);
                    Root havadurumu = Object;
                    label1.Text = string.Format("{0}", havadurumu.location.name);
                    label4.Text = string.Format("{0}", havadurumu.location.country);
                    label8.Text = string.Format("{0}", havadurumu.current.condition.text);
                    label11.Text = string.Format("{0}", getDate(havadurumu.location.localtime_epoch).DayOfWeek);
                    label3.Text = string.Format("{0}\u00B0" + "C", Convert.ToInt32(havadurumu.current.temp_c));
                    label6.Text = string.Format("{0} %", havadurumu.current.humidity);
                    label7.Text = string.Format("{0} km/h", Convert.ToInt32(havadurumu.current.wind_kph));
                    label10.Text = string.Format("{0}", havadurumu.current.last_updated);                   
                    string pngname = ((havadurumu.current.condition.icon + "  ").Substring(34, 14));
                    pictureBox1.Image = Image.FromFile("C:/Users/selcu/OneDrive/Masaüstü/c#proje/HavaDurumu/HavaDurumu/HavaDurumu/weather/64x64" + pngname);
                }
                catch
                {

                }

                //var culture = new System.Globalization.CultureInfo("tr-TR");
                // var day = culture.DateTimeFormat.GetDayName(getDate(havadurumu.location.localtime_epoch).DayOfWeek);               
             }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(20,10,10,50);
            timer1.Interval = 1000;
            timer1.Enabled = true;            

        }
        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            sehir = textBox1.Text;
            timer1.Start();
            textBox1.Text = "";
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                sehir = textBox1.Text;
                textBox1.Text = "";
                
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            getforcast(karakterCevir(sehir));

        }
    }
    
}
