using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> lakasok;

        Excel.Application xlApp; // A Microsoft Excel alkalmazás
        Excel.Workbook xlWB; // A létrehozott munkafüzet
        Excel.Worksheet xlSheet; // Munkalap a munkafüzeten belül

        public Form1()
        {
            InitializeComponent();

            LoadData();
         
        }

        public void LoadData()
        {
            lakasok = context.Flats.ToList();
        }

        public void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application(); // Excel elindítása és az applikáció objektum betöltése
                xlWB = xlApp.Workbooks.Add(Missing.Value); // Új munkafüzet
                xlSheet = xlWB.ActiveSheet; // Új munkalap
                xlApp.Visible = true; // Control átadása a felhasználónak
                xlApp.UserControl = true; 
            }
            catch (Exception ex) // Hibakezelés a beépített hibaüzenettel
            {
                string hiba = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(hiba, "Error");
             
                xlWB.Close(false, Type.Missing, Type.Missing); // Hiba esetén az Excel applikáció bezárása automatikusan
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }       
    }
}
