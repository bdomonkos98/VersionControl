using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using WebService.MnbServiceReference;

namespace WebService
{
    public partial class Form1 : Form
    {

        BindingList<RateData> Rates = new BindingList<RateData>();

        public Form1()
        {
            InitializeComponent();
            RefreshData();
        }


        private void RefreshData()
        {
            Rates.Clear();
            string xmlstring = GetExchangeRates();
            LoadXml(xmlstring);
            Charting();

            dataGridView1.DataSource = Rates;
        }

        private void Charting()
        {
            chartRateData.DataSource = Rates;

            Series series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void LoadXml(string input)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(input);
       
            foreach (XmlElement item in xml.DocumentElement)
            {           
                RateData r = new RateData();           
                r.Date = DateTime.Parse(item.GetAttribute("date"));
                
                XmlElement child = (XmlElement)item.FirstChild;
                r.Currency = child.GetAttribute("curr");
                r.Value = decimal.Parse(child.InnerText);
                int unit = int.Parse(child.GetAttribute("unit"));
                
                if (unit != 0)
                    r.Value = r.Value / unit;

                Rates.Add(r);
            }
        }

        private string GetExchangeRates()
        {
            MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
            GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody();

            request.currencyNames = comboBox1.SelectedItem.ToString();
            request.startDate = dateTimePicker1.Value.ToString();
            request.endDate = dateTimePicker2.Value.ToString();
                    
            var response = mnbService.GetExchangeRates(request);           
            string result = response.GetExchangeRatesResult;
            return result;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
