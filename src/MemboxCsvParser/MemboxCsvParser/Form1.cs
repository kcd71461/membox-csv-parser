using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using MemboxCsvParser.Data;
using Newtonsoft.Json;

namespace MemboxCsvParser
{
    public partial class Form1 : Form
    {
        private Dictionary<string, List<Card>> cardMap = null;

        private JsonSerializerSettings serializeSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
            Formatting = Formatting.None
        };

        public Form1()
        {
            InitializeComponent();
            this.fileNameTextBox.Text = Properties.Settings.Default.fileName;
            this.templateIdTextBox.Text = Properties.Settings.Default.templateId;
            this.imageUrlTextBox.Text = Properties.Settings.Default.imageUrlPath;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.fileNameTextBox.Text = ofd.FileName;
                }
            }
        }

        private void parseButton_Click(object sender, EventArgs e)
        {
            var fileName = this.fileNameTextBox.Text;
            this.cardMap = new Dictionary<string, List<Card>>();
            var templateId = int.Parse(this.templateIdTextBox.Text);

            Properties.Settings.Default.fileName = this.fileNameTextBox.Text;
            Properties.Settings.Default.templateId = this.templateIdTextBox.Text;
            Properties.Settings.Default.Save();

            using (TextReader reader = File.OpenText(this.fileNameTextBox.Text))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.HasHeaderRecord = true;
                while (csv.Read())
                {
                    var code = csv.GetField<string>("코드");
                    if (!this.cardMap.ContainsKey(code))
                    {
                        this.cardMap[code] = new List<Card>();
                    }
                    if (templateId == 8)
                        this.cardMap[code].Add(Template8.Parse(csv, this.imageUrlTextBox.Text));
                }
            }
            this.comboBox1.DataSource = this.cardMap.Keys.ToArray();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (string) this.comboBox1.SelectedItem;
            if (this.cardMap != null && this.cardMap.ContainsKey(selectedItem))
            {
                this.richTextBox1.Text = JsonConvert.SerializeObject(cardMap[selectedItem], serializeSetting);
            }
            else
            {
                this.richTextBox1.Text = "";
            }
        }
    }
}