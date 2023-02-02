using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFSeleniumWebScrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConnectionViewModel model = new ConnectionViewModel();
        private ChromeDriver dr;
        private String url = "https://www.correosdemexico.gob.mx/SSLServicios/SeguimientoEnvio/Seguimiento.aspx";
        private ChromeDriverService cds = ChromeDriverService.CreateDefaultService();
        private ChromeOptions ops = new ChromeOptions();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = model;
            cds.HideCommandPromptWindow= true;
            ops.AddArgument("headless");
            dr = new ChromeDriver(cds, ops);
            dr.Navigate().GoToUrl(url);
            lstrows.Visibility = Visibility.Collapsed;
        }
        private void clear_form_click(object sender, RoutedEventArgs e)
        {
            //  Ahora vamos a trabajar en el modelo q llenara los datos al inicio de la aplicacion
            SearTermTextBox.Text = string.Empty;
            cboyears.SelectedItem= DateTime.Now.Year;
            if (model.Rows != null) model.Rows.Clear();
            lstrows.Visibility = Visibility.Collapsed;
        }
        private void search_click(object sender, RoutedEventArgs e)
        {//Ahora q ya tenemos la carga del modelo y la interfaz principal lista, procedemos con el scrapp
            if (model.Rows != null) model.Rows.Clear();
            model.Rows = new ObservableCollection<Registro>();
            var txt = SearTermTextBox.Text ?? "";
            if (!String.IsNullOrEmpty(txt))
            {
                //Aqui configuramos los campos de busqueda de nuestra interfaz en el formulario web
                var search = dr.FindElement(By.Id("Guia"));
                search.SendKeys(txt);
                var cbo = dr.FindElement(By.Id("Periodo"));
                cbo.SendKeys(cboyears.SelectedValue.ToString());
                var btn = dr.FindElement(By.Id("Busqueda"));
                btn.Click();
                //Ahora procedemos a capturar los datos obtenidos
                bool get_headers = true;
                var xpath = "/html/body/form/main/div[1]/div[6]/div[1]/table/tbody/tr";
                           ///html/body/form/main/div[1]/div[5]/div[1]/table/tbody/tr
                var rows = dr.FindElements(By.XPath(xpath));
                foreach (var row in rows )
                {
                    if (get_headers)
                    {
                        get_headers = false;
                        continue;
                    }
                    var linea = row.Text.Trim();
                    var cells = row.FindElements(By.TagName("td"));
                    model.Rows.Add(new Registro {
                        fecha = cells[0].Text,
                        hora = cells[1].Text,
                        origen = cells[2].Text,
                        evento = cells[3].Text,
                    });
                }
                if (model.Rows.Count > 0) lstrows.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("Favor de ingresar tu numero de guia");
        }
    }
}
