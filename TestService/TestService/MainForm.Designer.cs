namespace TestService
{
    partial class MainForm
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
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtFavoriteMovie = new System.Windows.Forms.TextBox();
            this.lblFavoriteMovie = new System.Windows.Forms.Label();
            this.txtFavoriteLanguage = new System.Windows.Forms.TextBox();
            this.lblFavoriteLanguage = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnGetFavorites = new System.Windows.Forms.Button();
            this.btnGetCustomerList = new System.Windows.Forms.Button();
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.btnAddToQueue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(72, 29);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(91, 19);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First Name:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(169, 26);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(317, 27);
            this.txtFirstName.TabIndex = 1;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(169, 59);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(317, 27);
            this.txtLastName.TabIndex = 3;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(74, 62);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(89, 19);
            this.lblLastName.TabIndex = 2;
            this.lblLastName.Text = "Last Name:";
            // 
            // txtFavoriteMovie
            // 
            this.txtFavoriteMovie.Location = new System.Drawing.Point(169, 93);
            this.txtFavoriteMovie.Name = "txtFavoriteMovie";
            this.txtFavoriteMovie.Size = new System.Drawing.Size(317, 27);
            this.txtFavoriteMovie.TabIndex = 5;
            // 
            // lblFavoriteMovie
            // 
            this.lblFavoriteMovie.AutoSize = true;
            this.lblFavoriteMovie.Location = new System.Drawing.Point(46, 96);
            this.lblFavoriteMovie.Name = "lblFavoriteMovie";
            this.lblFavoriteMovie.Size = new System.Drawing.Size(117, 19);
            this.lblFavoriteMovie.TabIndex = 4;
            this.lblFavoriteMovie.Text = "Favorite Movie:";
            // 
            // txtFavoriteLanguage
            // 
            this.txtFavoriteLanguage.Location = new System.Drawing.Point(169, 126);
            this.txtFavoriteLanguage.Name = "txtFavoriteLanguage";
            this.txtFavoriteLanguage.Size = new System.Drawing.Size(317, 27);
            this.txtFavoriteLanguage.TabIndex = 7;
            // 
            // lblFavoriteLanguage
            // 
            this.lblFavoriteLanguage.AutoSize = true;
            this.lblFavoriteLanguage.Location = new System.Drawing.Point(19, 129);
            this.lblFavoriteLanguage.Name = "lblFavoriteLanguage";
            this.lblFavoriteLanguage.Size = new System.Drawing.Size(144, 19);
            this.lblFavoriteLanguage.TabIndex = 6;
            this.lblFavoriteLanguage.Text = "Favorite Language:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(26, 189);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(53, 27);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(93, 189);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(146, 27);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update Favorites";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnGetFavorites
            // 
            this.btnGetFavorites.Location = new System.Drawing.Point(253, 189);
            this.btnGetFavorites.Name = "btnGetFavorites";
            this.btnGetFavorites.Size = new System.Drawing.Size(146, 27);
            this.btnGetFavorites.TabIndex = 10;
            this.btnGetFavorites.Text = "Get Favorites";
            this.btnGetFavorites.UseVisualStyleBackColor = true;
            this.btnGetFavorites.Click += new System.EventHandler(this.btnGetFavorites_Click);
            // 
            // btnGetCustomerList
            // 
            this.btnGetCustomerList.Location = new System.Drawing.Point(413, 189);
            this.btnGetCustomerList.Name = "btnGetCustomerList";
            this.btnGetCustomerList.Size = new System.Drawing.Size(146, 27);
            this.btnGetCustomerList.TabIndex = 11;
            this.btnGetCustomerList.Text = "Get Customer List";
            this.btnGetCustomerList.UseVisualStyleBackColor = true;
            this.btnGetCustomerList.Click += new System.EventHandler(this.btnGetCustomerList_Click);
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomers.Location = new System.Drawing.Point(26, 240);
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.Size = new System.Drawing.Size(675, 227);
            this.dgvCustomers.TabIndex = 12;
            // 
            // btnAddToQueue
            // 
            this.btnAddToQueue.Location = new System.Drawing.Point(573, 189);
            this.btnAddToQueue.Name = "btnAddToQueue";
            this.btnAddToQueue.Size = new System.Drawing.Size(127, 27);
            this.btnAddToQueue.TabIndex = 13;
            this.btnAddToQueue.Text = "Add To Queue";
            this.btnAddToQueue.UseVisualStyleBackColor = true;
            this.btnAddToQueue.Click += new System.EventHandler(this.btnAddToQueue_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 484);
            this.Controls.Add(this.btnAddToQueue);
            this.Controls.Add(this.dgvCustomers);
            this.Controls.Add(this.btnGetCustomerList);
            this.Controls.Add(this.btnGetFavorites);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtFavoriteLanguage);
            this.Controls.Add(this.lblFavoriteLanguage);
            this.Controls.Add(this.txtFavoriteMovie);
            this.Controls.Add(this.lblFavoriteMovie);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblFirstName);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Test the Service";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtFavoriteMovie;
        private System.Windows.Forms.Label lblFavoriteMovie;
        private System.Windows.Forms.TextBox txtFavoriteLanguage;
        private System.Windows.Forms.Label lblFavoriteLanguage;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnGetFavorites;
        private System.Windows.Forms.Button btnGetCustomerList;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Button btnAddToQueue;
    }
}

