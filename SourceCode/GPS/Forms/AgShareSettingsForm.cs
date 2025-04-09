using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgOpenGPS;
using AgOpenGPS.Properties;

public partial class AgShareSettingsForm : Form
{
    private Button btnTest;
    private Button btnSave;
    private Button btnClose;
    private Label lblStatus;
    private Label label2;
    private TextBox txtApiKey;
    private FormGPS formGPS;

    public AgShareSettingsForm()
    {
        InitializeComponent();
        txtApiKey.Text = Settings.Default.AgShareApiKey ?? "";
    }
        public AgShareSettingsForm(FormGPS formGPS) : this()
    {
        this.formGPS = formGPS;
    }
    private async void btnTest_Click(object sender, EventArgs e)
    {
        lblStatus.Text = "Verbinden...";
        lblStatus.ForeColor = System.Drawing.Color.Gray;

        AgShareApi.SaveApiKey(txtApiKey.Text);

        bool result = await AgShareApi.TestApiKeyAsync();
        lblStatus.Text = result ? "✔ API key is geldig" : "❌ Ongeldige API key";
        lblStatus.ForeColor = result ? System.Drawing.Color.Green : System.Drawing.Color.Red;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        AgShareApi.SaveApiKey(txtApiKey.Text);
        MessageBox.Show("API key opgeslagen.", "Opgeslagen", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void InitializeComponent()
    {
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtApiKey
            // 
            this.txtApiKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApiKey.Location = new System.Drawing.Point(12, 59);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(453, 29);
            this.txtApiKey.TabIndex = 0;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(12, 167);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(110, 49);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
        // 
        // btnSave
        // 
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(188, 167);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 49);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);  
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(355, 167);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 49);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(155, 107);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(189, 25);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status: Connected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Fill in API Key";
            // 
            // AgShareSettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(477, 228);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtApiKey);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AgShareSettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AgShare";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

    }
}
