using QifApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QifLoad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public QifTransaction Qt;
        public DataTable Dt;
        
        public MainWindow()
            
        {
            
            InitializeComponent();
        }

        private void LoadQifFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 

            dlg.DefaultExt = ".qif";

            dlg.Filter = "QIF Files (.qif)|*.qif";



            // Display OpenFileDialog by calling ShowDialog method 

            Nullable<bool> result = dlg.ShowDialog();



            // Get the selected file name and display in a TextBox 

            if (result == true)
            {

                // Open document 

                string filename = dlg.FileName;

                FileNameTextBox.Text = filename;

                String LoadedMessage = "Loaded";
                QifDom Qd = new QifDom();
                //Qd.Import("C:\\Users\\Mike\\Documents\\AccountingDatabase\\010544496_20140817.qif");
                Qd.Import(filename);
                LoadMessage.Text = LoadedMessage;
                Qt = new QifTransaction(Qd);
                Dt = Qt.GetTransactionCollection();
                TransactionGrid.ItemsSource = Dt.DefaultView;
            
            }         
          
        }
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            if (e.PropertyType == typeof(System.Decimal))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "0.00";
                Style style = new Style(); // creates object of style class   
                style.TargetType = typeof(DataGridCell); //sets target type as DataGrid cell   
                Setter setterAlignment = new Setter(); // create objects of setter class   
                setterAlignment.Property = DataGridCell.HorizontalAlignmentProperty;
                setterAlignment.Value = HorizontalAlignment.Right;
                style.Setters.Add(setterAlignment);
                (e.Column as DataGridTextColumn).CellStyle = style;
            }
            
        }

        private void SaveAsCsv_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();



            // Set filter for file extension and default file extension 

            dlg.DefaultExt = ".csv";

            dlg.Filter = "CSV Files (.csv)|*.csv";



            // Display OpenFileDialog by calling ShowDialog method 

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                StreamWriter sw = new StreamWriter(dlg.FileName);
                for (int i = 0; i < Dt.Columns.Count; i++)
                {
                    sw.Write(Dt.Columns[i]);
                    if (i < Dt.Columns.Count - 1)
                        sw.Write(',');
                    else
                        sw.Write('\n');
                }
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < Dt.Columns.Count; i++)
                    {
                        sw.Write( dr[i]);
                        if (i < Dt.Columns.Count - 1)
                            sw.Write(',');
                        else
                            sw.Write('\n');
                    }
                }
                sw.Flush();
                sw.Close();
            }

        }
    }
}
