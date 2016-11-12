namespace IntuitNotesApp
{
    partial class IntuitNotes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerNotes = new System.Windows.Forms.SplitContainer();
            this.dvNotes = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnEmail = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.NotesBody = new System.Windows.Forms.RichTextBox();
            this.Add = new System.Windows.Forms.ToolStripButton();
            this.Save = new System.Windows.Forms.ToolStripButton();
            this.Sync = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNotes)).BeginInit();
            this.splitContainerNotes.Panel1.SuspendLayout();
            this.splitContainerNotes.Panel2.SuspendLayout();
            this.splitContainerNotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvNotes)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(885, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // splitContainerNotes
            // 
            this.splitContainerNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerNotes.Location = new System.Drawing.Point(0, 28);
            this.splitContainerNotes.Name = "splitContainerNotes";
            // 
            // splitContainerNotes.Panel1
            // 
            this.splitContainerNotes.Panel1.Controls.Add(this.dvNotes);
            this.splitContainerNotes.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainerNotes.Panel2
            // 
            this.splitContainerNotes.Panel2.Controls.Add(this.btnEmail);
            this.splitContainerNotes.Panel2.Controls.Add(this.lblTitle);
            this.splitContainerNotes.Panel2.Controls.Add(this.txtTitle);
            this.splitContainerNotes.Panel2.Controls.Add(this.NotesBody);
            this.splitContainerNotes.Size = new System.Drawing.Size(885, 585);
            this.splitContainerNotes.SplitterDistance = 295;
            this.splitContainerNotes.TabIndex = 1;
            // 
            // dvNotes
            // 
            this.dvNotes.AllowUserToAddRows = false;
            this.dvNotes.AllowUserToDeleteRows = false;
            this.dvNotes.AllowUserToResizeColumns = false;
            this.dvNotes.AllowUserToResizeRows = false;
            this.dvNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvNotes.ColumnHeadersVisible = false;
            this.dvNotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.ID});
            this.dvNotes.Location = new System.Drawing.Point(3, 66);
            this.dvNotes.Name = "dvNotes";
            this.dvNotes.ReadOnly = true;
            this.dvNotes.RowHeadersVisible = false;
            this.dvNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvNotes.Size = new System.Drawing.Size(240, 150);
            this.dvNotes.TabIndex = 1;
            this.dvNotes.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvNotes_RowLeave);
            this.dvNotes.SelectionChanged += new System.EventHandler(this.dvNotes_SelectionChanged);
            // 
            // Title
            // 
            this.Title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Title.DataPropertyName = "Title";
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "NoteGuid";
            this.ID.HeaderText = "id";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Add,
            this.Save,
            this.Sync});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(295, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnEmail
            // 
            this.btnEmail.Location = new System.Drawing.Point(499, 4);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(75, 23);
            this.btnEmail.TabIndex = 3;
            this.btnEmail.Text = "SendMail";
            this.btnEmail.UseVisualStyleBackColor = true;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(30, 15);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(56, 7);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(402, 20);
            this.txtTitle.TabIndex = 1;
            this.txtTitle.Tag = "Title";
            this.txtTitle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTitle_KeyPress);
            // 
            // NotesBody
            // 
            this.NotesBody.Location = new System.Drawing.Point(3, 44);
            this.NotesBody.Name = "NotesBody";
            this.NotesBody.Size = new System.Drawing.Size(580, 541);
            this.NotesBody.TabIndex = 0;
            this.NotesBody.Text = "";
            this.NotesBody.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NotesBody_KeyPress);
            // 
            // Add
            // 
            this.Add.Image = global::IntuitNotesApp.Properties.Resources.add_icon_13061;
            this.Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(57, 24);
            this.Add.Text = "Add";
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Save
            // 
            this.Save.Image = global::IntuitNotesApp.Properties.Resources.save_256;
            this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(60, 24);
            this.Save.Text = "Save";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Sync
            // 
            this.Sync.Image = global::IntuitNotesApp.Properties.Resources.Sync_;
            this.Sync.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Sync.Name = "Sync";
            this.Sync.Size = new System.Drawing.Size(59, 24);
            this.Sync.Text = "Sync";
            this.Sync.Click += new System.EventHandler(this.Sync_Click);
            // 
            // IntuitNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 613);
            this.Controls.Add(this.splitContainerNotes);
            this.Controls.Add(this.menuStrip1);
            this.Name = "IntuitNotes";
            this.Text = "IntuitNotes";
            this.Load += new System.EventHandler(this.IntuitNotes_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainerNotes.Panel1.ResumeLayout(false);
            this.splitContainerNotes.Panel1.PerformLayout();
            this.splitContainerNotes.Panel2.ResumeLayout(false);
            this.splitContainerNotes.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNotes)).EndInit();
            this.splitContainerNotes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvNotes)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerNotes;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Add;
        private System.Windows.Forms.ToolStripButton Save;
        private System.Windows.Forms.ToolStripButton Sync;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.RichTextBox NotesBody;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dvNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.Button btnEmail;
    }
}

