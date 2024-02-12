using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GES
{
    public partial class MainForm : Form
    {
                
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // if values are not empty fields
                if (!string.IsNullOrEmpty(txtDescription.Text) && numericPrice.Value > 0 && !string.IsNullOrEmpty(comboBoxCategory.Text))
                {
                    // update db with values (csv)
                    // Add new empty line at the end, if doesn't exist
                    using (StreamWriter sw = File.AppendText(AppGlobal.dataBasePath))
                    {
                        sw.WriteLine();
                    }
                    string rowToAdd = comboBoxCategory.Text + ";" + txtDescription.Text + ";" + numericPrice.Value.ToString();
                    File.AppendAllText(AppGlobal.dataBasePath, rowToAdd + Environment.NewLine);
                    UpdateDataBase();
                }
                UpdateTotalPrice();
            }
            catch (Exception ex)
            {
                AppGlobal.log.Write("btnAdd_Click" + ex.ToString());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateDataBase();
            UpdateTotalPrice();
            AlignCategoriesFromDB();
            AlignCategoryComboBox();
        }
        private bool UpdateDataBase()
        {
            // backup my DataGrid
            DataGridView backup = dataGridViewSpese;
            try
            {                
                // Clear my DataGrid
                dataGridViewSpese.Rows.Clear();

                using (StreamReader sr = new StreamReader(AppGlobal.dataBasePath))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i > 0 && !string.IsNullOrEmpty(line))
                        {
                            string[] values = line.Split(';');
                            dataGridViewSpese.Rows.Add(values[0], values[1], values[2]);                            
                        }
                        i++;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                AppGlobal.log.Write("UpdateDataBase()-->" + ex.ToString());
                dataGridViewSpese = backup;
                return false;
            }
        }
        private bool UpdateTotalPrice()
        {
            try
            {
                DataTable dt = new DataTable();

                double total = 0.0;
                for (int i = 0; i < dataGridViewSpese.Rows.Count; i++)
                {
                    total += Convert.ToDouble(dataGridViewSpese.Rows[i].Cells[2].Value);
                }
                txtTotalPrice.Text = total.ToString("0.00");
                return true;
            }
            catch (Exception ex)
            {
                AppGlobal.log.Write("UpdateTotalPrice()-->" + ex.ToString());
                return false;
            }
        }

        private void btnOriginal_Click(object sender, EventArgs e)
        {
            UpdateDataBase();
            UpdateTotalPrice();
        }
        private void AlignCategoryComboBox()
        {
            for (int i = 0; i < comboBoxCategory.Items.Count; i++)
            {
                // Assegna l'elemento corrispondente dalla seconda ComboBox
                comboBoxSortCategory.Items.Add(comboBoxCategory.Items[i]);
            }
        }
        private void AlignCategoriesFromDB()
        {
            using (StreamReader sr = new StreamReader(AppGlobal.dataBasePath))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (i > 0 && !string.IsNullOrEmpty(line)) //skip headers and empty rows
                    {
                        string category = line.Split(';')[0];
                        if (!comboBoxCategory.Items.Contains(category))
                        {
                            comboBoxCategory.Items.Add(category);
                        }
                    }
                    i++;
                }
            }
        }
        private void btnSortCategory_Click(object sender, EventArgs e)
        {            
            using (StreamReader sr = new StreamReader(AppGlobal.dataBasePath))
            {
                dataGridViewSpese.Rows.Clear();
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (i > 0 && !string.IsNullOrEmpty(line)) //skip headers and empty rows
                    {
                        string category = line.Split(';')[0];
                        if (i > 0 && !string.IsNullOrEmpty(line) && category == comboBoxSortCategory.Text)
                        {
                            string[] values = line.Split(';');
                            dataGridViewSpese.Rows.Add(values[0], values[1], values[2]);
                        }
                    }
                    i++;
                }
            }
            UpdateTotalPrice();
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            // Ottieni l'elenco degli indici delle righe selezionate
            List<int> selectedRowsIndexes = new List<int>();
            foreach (DataGridViewRow row in dataGridViewSpese.SelectedRows)
            {
                selectedRowsIndexes.Add(row.Index);
            }

            // Rimuovi le righe selezionate dalla DataGridView
            foreach (int index in selectedRowsIndexes.OrderByDescending(i => i))
            {
                dataGridViewSpese.Rows.RemoveAt(index);
            }
            UpdateDataBase();
        }
    }
}
