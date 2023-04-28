using Newtonsoft.Json;
using PindurCandy_Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PindurCandy_Admin
{
    /// <summary>
    /// Interaction logic for Termekek.xaml
    /// </summary>
    public partial class Termekek : Window
    {
        bool beolvasva = false;
        int ID = 0;

        private string Ellenorzes()
        {
            return "";
        }

        private void MezokTorlese()
        {
            ID = 0;
            txb_TermekNev.Text = "";
            txb_Link.Text = "";
            txb_Ar.Text = "";
            txb_Leiras.Text = "";
            cmb_Aktiv.Text = "1";
      
        }
        private void AdatBeolvasas()
        {
            List<Models.Termekek> list = new List<Models.Termekek>();
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = Encoding.UTF8;
            string url = $"https://localhost:5001/Termekek/basic";
            MezokTorlese();
            try
            {
                string result = client.DownloadString(url);
                list = JsonConvert.DeserializeObject<List<Models.Termekek>>(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            termekek_adatai.ItemsSource = list;
            beolvasva = true;
        }
        public Termekek()
        {
            InitializeComponent();
            if (!beolvasva)
            {
                AdatBeolvasas();
                List<int> AktivLista = new List<int>() { 0, 1 };
                cmb_Aktiv.ItemsSource = AktivLista;
            }
        }
        private void Termekek_adatai_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Termekek termek = termekek_adatai.SelectedItems[0] as Models.Termekek;
                ID = termek.Id;
                txb_TermekNev.Text = termek.TermekNev;
                byte[] bytes = Encoding.ASCII.GetBytes(txb_kep.Text);
                termek.Kep = bytes;
                txb_Ar.Text =Convert.ToString(termek.Ar);
                txb_Leiras.Text =termek.Leiras;
                txb_Link.Text =termek.Link;
                cmb_Aktiv.Text = termek.Aktiv.ToString();
            }
            catch (Exception ex)
            {
                MezokTorlese();
            }
        }
        private void btn_Tarolas_Click(object sender, RoutedEventArgs e)
        {
            string uzenet = Ellenorzes();
            if (uzenet == "")
            {
                Models.Termekek termek = new Models.Termekek();
                termek.TermekNev = txb_TermekNev.Text;
                termek.Link = txb_Link.Text;
                termek.Ar = int.Parse(txb_Ar.Text);
                termek.Leiras = txb_Leiras.Text;
                byte[] bytes = Encoding.ASCII.GetBytes(txb_kep.Text);
                termek.Kep = bytes;
                termek.Aktiv = int.Parse(cmb_Aktiv.Text);
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = Encoding.UTF8;
                string url = $"https://localhost:5001/Termekek/";
                try
                {
                    string result = client.UploadString(url, "POST", JsonConvert.SerializeObject(termek));
                    MessageBox.Show(result);
                    AdatBeolvasas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(uzenet);
            }
        }
        private void btn_Modositas_Click(object sender, RoutedEventArgs e)
        {
            string uzenet = Ellenorzes();
            if (uzenet == "")
            {
                if (ID != 0)
                {
                    Models.Termekek termek = new Models.Termekek();
                    termek.Id = ID;
                    termek.TermekNev=txb_TermekNev.Text;
                    termek.Link= txb_Link.Text;
                    termek.Ar = int.Parse(txb_Ar.Text);
                    termek.Leiras = txb_Leiras.Text;
                    byte[] bytes = Encoding.ASCII.GetBytes(txb_kep.Text);
                    termek.Kep = bytes;
                    termek.Aktiv = int.Parse(cmb_Aktiv.Text);
                    WebClient client = new WebClient();
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string url = $"https://localhost:5001/Termekek/";
                    try
                    {
                        string result = client.UploadString(url, "PUT", JsonConvert.SerializeObject(termek));
                        MessageBox.Show(result);
                        AdatBeolvasas();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show(uzenet);
            }
        }
        private void btn_Torles_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Biztosan törli a(z) {txb_TermekNev.Text} nevű terméket?",
                    "Termék törlése",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Models.Termekek termek = new Models.Termekek();
                termek.Id = ID;
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = Encoding.UTF8;
                string url = $"https://localhost:5001/Termekek/{ID}";
                try
                {
                    string result = client.UploadString(url, "DELETE", "");
                    MessageBox.Show(result);
                    AdatBeolvasas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All supported graphics|.jpg;.jpeg;*.png|" +
            "JPEG (.jpg;.jpeg)|.jpg;.jpeg|" +
            "Portable Network Graphic (.png)|.png";

            if (openFileDialog.ShowDialog() == true)
            {
                using (FileStream imgStream = File.OpenRead(openFileDialog.FileName))
                {
                    byte[] blob = new byte[imgStream.Length];
                    imgStream.Read(blob, 0, (int)imgStream.Length);
                    string kepbajtok = "";
                    for (int i = 0; i < 1000; i++)
                    {
                        kepbajtok += blob[i].ToString() + " ";
                    }
                    txb_kep.Text = kepbajtok;

                }
            }
        }

        private void btn_vissza_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
    }
