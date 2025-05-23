﻿namespace FluxPrompt
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.PromptTextBox = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ResultDataGridView = new System.Windows.Forms.DataGridView();
            this.PromptPanel = new System.Windows.Forms.Panel();
            this.PromptLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ResultDataGridView)).BeginInit();
            this.PromptPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PromptTextBox
            // 
            this.PromptTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.PromptTextBox.CausesValidation = false;
            this.PromptTextBox.Location = new System.Drawing.Point(30, 0);
            this.PromptTextBox.Name = "PromptTextBox";
            this.PromptTextBox.Multiline = true;
            this.PromptTextBox.Size = new System.Drawing.Size(690, 80);
            this.PromptTextBox.Font = new System.Drawing.Font("Segoe UI Variable", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PromptTextBox.TabIndex = 0;
            this.PromptTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.PromptTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "FluxPrompt";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ResultDataGridView
            // 
            this.ResultDataGridView.AllowUserToAddRows = false;
            this.ResultDataGridView.AllowUserToDeleteRows = false;
            this.ResultDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ResultDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ResultDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.ResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultDataGridView.ColumnHeadersVisible = false;
            this.ResultDataGridView.Location = new System.Drawing.Point(10, 90);
            this.ResultDataGridView.MultiSelect = false;
            this.ResultDataGridView.Name = "ResultDataGridView";
            this.ResultDataGridView.ReadOnly = true;
            this.ResultDataGridView.RowHeadersVisible = false;
            this.ResultDataGridView.RowTemplate.Height = 25;
            this.ResultDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ResultDataGridView.Size = new System.Drawing.Size(780, 382);
            this.ResultDataGridView.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Variable", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ResultDataGridView.TabIndex = 1;
            this.ResultDataGridView.TabStop = false;
            this.ResultDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResultDataGridView_CellClick);
            // 
            // PromptPanel
            // 
            this.PromptPanel.BackColor = System.Drawing.Color.Gray;
            this.PromptPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PromptPanel.Controls.Add(this.PromptLabel);
            this.PromptPanel.Controls.Add(this.PromptTextBox);
            this.PromptPanel.Location = new System.Drawing.Point(10, 10);
            this.PromptPanel.Name = "PromptPanel";
            this.PromptPanel.Size = new System.Drawing.Size(790, 80);
            this.PromptPanel.TabIndex = 2;
            this.PromptPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MovableControls_MouseDown);
            // 
            // PromptLabel
            // 
            this.PromptLabel.AutoSize = true;
            this.PromptLabel.Font = new System.Drawing.Font("Segoe UI Variable", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PromptLabel.ForeColor = System.Drawing.Color.White;
            this.PromptLabel.Location = new System.Drawing.Point(9, 30);
            this.PromptLabel.Name = "PromptLabel";
            this.PromptLabel.Size = new System.Drawing.Size(80, 20);
            this.PromptLabel.TabIndex = 1;
            this.PromptLabel.Text = ">";
            this.PromptLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MovableControls_MouseDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI Variable", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PromptPanel);
            this.Controls.Add(this.ResultDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flux Prompt";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Activated += new System.EventHandler(this.OnActivated);
            this.Deactivate += new System.EventHandler(this.OnDeactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Resize += new System.EventHandler(this.OnResize);
            ((System.ComponentModel.ISupportInitialize)(this.ResultDataGridView)).EndInit();
            this.PromptPanel.ResumeLayout(false);
            this.PromptPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox PromptTextBox;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.DataGridView ResultDataGridView;
        private System.Windows.Forms.Panel PromptPanel;
        private System.Windows.Forms.Label PromptLabel;
    }
}

