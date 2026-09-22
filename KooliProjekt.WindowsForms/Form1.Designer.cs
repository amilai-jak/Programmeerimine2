namespace KooliProjekt.WindowsForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private Panel detailsPanel;
        private Label idLabel;
        private TextBox idField;
        private Label nameLabel;
        private TextBox nameField;
        private Label tickerLabel;
        private TextBox tickerField;
        private Label assetClassLabel;
        private TextBox assetClassField;
        private CheckBox isRealEstateField;
        private Button addCommand;
        private Button saveCommand;
        private Button deleteCommand;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private Label MakeLabel(string text, int x, int y, int w = 110)
        {
            var label = new Label();
            label.Location = new Point(x, y);
            label.Name = "label_" + text;
            label.Size = new Size(w, 24);
            label.TabIndex = 0;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            return label;
        }

        private TextBox MakeTextBox(string name, int x, int y, int w = 200)
        {
            var box = new TextBox();
            box.Location = new Point(x, y);
            box.Name = name;
            box.Size = new Size(w, 27);
            box.TabIndex = 1;
            return box;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.dataGridView1 = new DataGridView();
            this.detailsPanel = new Panel();
            this.idLabel = MakeLabel("ID:", 20, 20, 100);
            this.idField = MakeTextBox("idField", 130, 20, 200);
            this.nameLabel = MakeLabel("Nimi:", 20, 60, 100);
            this.nameField = MakeTextBox("nameField", 130, 60, 200);
            this.tickerLabel = MakeLabel("Ticker:", 20, 100, 100);
            this.tickerField = MakeTextBox("tickerField", 130, 100, 200);
            this.assetClassLabel = MakeLabel("Vara klass ID:", 20, 140, 100);
            this.assetClassField = MakeTextBox("assetClassField", 130, 140, 200);
            this.isRealEstateField = new CheckBox();
            this.addCommand = new Button();
            this.saveCommand = new Button();
            this.deleteCommand = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.detailsPanel.SuspendLayout();
            this.SuspendLayout();

            // dataGridView1
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new Size(520, 520);
            this.dataGridView1.TabIndex = 0;

            // detailsPanel
            this.detailsPanel.Controls.Add(this.idLabel);
            this.detailsPanel.Controls.Add(this.idField);
            this.detailsPanel.Controls.Add(this.nameLabel);
            this.detailsPanel.Controls.Add(this.nameField);
            this.detailsPanel.Controls.Add(this.tickerLabel);
            this.detailsPanel.Controls.Add(this.tickerField);
            this.detailsPanel.Controls.Add(this.assetClassLabel);
            this.detailsPanel.Controls.Add(this.assetClassField);
            this.detailsPanel.Controls.Add(this.isRealEstateField);
            this.detailsPanel.Controls.Add(this.addCommand);
            this.detailsPanel.Controls.Add(this.saveCommand);
            this.detailsPanel.Controls.Add(this.deleteCommand);
            this.detailsPanel.Dock = DockStyle.Right;
            this.detailsPanel.Location = new Point(520, 0);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new Size(360, 520);
            this.detailsPanel.TabIndex = 1;

            // isRealEstateField
            this.isRealEstateField.Location = new Point(130, 180);
            this.isRealEstateField.Name = "isRealEstateField";
            this.isRealEstateField.Size = new Size(200, 27);
            this.isRealEstateField.TabIndex = 2;
            this.isRealEstateField.Text = "Kinnisvara";
            this.isRealEstateField.UseVisualStyleBackColor = true;

            // addCommand
            this.addCommand.Location = new Point(20, 240);
            this.addCommand.Name = "addCommand";
            this.addCommand.Size = new Size(100, 35);
            this.addCommand.TabIndex = 3;
            this.addCommand.Text = "Lisa uus";
            this.addCommand.UseVisualStyleBackColor = true;

            // saveCommand
            this.saveCommand.Location = new Point(130, 240);
            this.saveCommand.Name = "saveCommand";
            this.saveCommand.Size = new Size(100, 35);
            this.saveCommand.TabIndex = 4;
            this.saveCommand.Text = "Salvesta";
            this.saveCommand.UseVisualStyleBackColor = true;

            // deleteCommand
            this.deleteCommand.Location = new Point(240, 240);
            this.deleteCommand.Name = "deleteCommand";
            this.deleteCommand.Size = new Size(100, 35);
            this.deleteCommand.TabIndex = 5;
            this.deleteCommand.Text = "Kustuta";
            this.deleteCommand.UseVisualStyleBackColor = true;

            // Form1
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(880, 520);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.detailsPanel);
            this.Name = "Form1";
            this.Text = "KooliProjekt - Varad";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.detailsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
