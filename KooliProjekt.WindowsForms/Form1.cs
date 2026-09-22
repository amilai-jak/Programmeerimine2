using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    // 26.03.2026 - vorm kasutab nuud IMainView interface'i
    // 27.03.2026 - nupuvajutuste sundmused on presenteris
    public partial class Form1 : Form, IMainView
    {
        private readonly IApiClient _apiClient;
        private MainViewPresenter _mainViewPresenter;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<Asset> DataSource
        {
            get { return (IList<Asset>)dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Asset SelectedItem
        {
            get
            {
                if (dataGridView1.CurrentRow == null)
                {
                    return null;
                }

                return (Asset)dataGridView1.CurrentRow.DataBoundItem;
            }
            set
            {
                dataGridView1.DataSource = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentId
        {
            get { return int.Parse(idField.Text); }
            set { idField.Text = value.ToString(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentName
        {
            get { return nameField.Text; }
            set { nameField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentTicker
        {
            get { return tickerField.Text; }
            set { tickerField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentAssetClassID
        {
            get
            {
                if (!int.TryParse(assetClassField.Text, out var assetClassId))
                {
                    return 0;
                }

                return assetClassId;
            }
            set { assetClassField.Text = value.ToString(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CurrentIsRealEstate
        {
            get { return isRealEstateField.Checked; }
            set { isRealEstateField.Checked = value; }
        }

        public Form1(IApiClient apiClient)
        {
            _apiClient = apiClient;

            InitializeComponent();

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            saveCommand.Click += SaveCommand_Click;
            addCommand.Click += AddCommand_Click;
            deleteCommand.Click += DeleteCommand_Click;
        }

        public void SetPresenter(MainViewPresenter presenter)
        {
            _mainViewPresenter = presenter;
        }

        public bool ConfirmDelete()
        {
            var message = "Oled kindel, et soovid kustutada " + nameField.Text + "?";
            var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return (answer == DialogResult.Yes);
        }

        // Koosta etteantud veateatest ja OperationResult sees olevatest vigadest
        // veateade ja naita seda kasutajale
        public void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = "";
            var propertyErrors = "";

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            error = error.Trim();

            MessageBox.Show(error, "Viga!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void DeleteCommand_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Delete();
        }

        private void AddCommand_Click(object sender, EventArgs e)
        {
            _mainViewPresenter.SetSelection(null);
        }

        private async void SaveCommand_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Save();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                _mainViewPresenter.SetSelection(null);
                return;
            }

            var selectedItem = (Asset)dataGridView1.CurrentRow.DataBoundItem;
            _mainViewPresenter.SetSelection(selectedItem);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await _mainViewPresenter.LoadData();
        }
    }
}
