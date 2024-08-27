/*
 * Created by Jeff 08/23/2024
 * 
 * This form is for helping people specify their 
 * */
using System;
using System.Windows.Forms;

namespace MicroscopeImageProcessing
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.Text = "Microscope Image Processing";
			this.ClientSize = new System.Drawing.Size(800, 600);

			// Header Label
			Label headerLabel = new Label();
			headerLabel.Text = "Microscope Image Processing";
			headerLabel.Font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold);
			headerLabel.AutoSize = true;
			headerLabel.Location = new System.Drawing.Point(10, 10);
			this.Controls.Add(headerLabel);

			// MenuStrip for navigation
			MenuStrip menuStrip = new MenuStrip();
			ToolStripMenuItem homeMenuItem = new ToolStripMenuItem("Home");
			ToolStripMenuItem processMenuItem = new ToolStripMenuItem("Process");
			ToolStripMenuItem reviewMenuItem = new ToolStripMenuItem("Review");
			ToolStripMenuItem loginMenuItem = new ToolStripMenuItem("Login");
			ToolStripMenuItem logoutMenuItem = new ToolStripMenuItem("Logout");

			menuStrip.Items.Add(homeMenuItem);
			menuStrip.Items.Add(processMenuItem);
			menuStrip.Items.Add(reviewMenuItem);
			menuStrip.Items.Add(loginMenuItem);
			menuStrip.Items.Add(logoutMenuItem);

			menuStrip.Location = new System.Drawing.Point(0, 50);  // Position below the header
			this.Controls.Add(menuStrip);

			// Main content area
			Panel contentPanel = new Panel();
			contentPanel.BorderStyle = BorderStyle.FixedSingle;
			contentPanel.Location = new System.Drawing.Point(10, 80);
			contentPanel.Size = new System.Drawing.Size(780, 450);
			this.Controls.Add(contentPanel);

			// Footer Label
			Label footerLabel = new Label();
			footerLabel.Text = "© 2024 Microscope Image Processing";
			footerLabel.AutoSize = true;
			footerLabel.Location = new System.Drawing.Point(10, 540);
			this.Controls.Add(footerLabel);
		}

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
