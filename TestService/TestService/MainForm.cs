using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestService
{
    public partial class MainForm : Form
    {

        public DataSet ds_customers;
        public BindingSource bs_customers;


        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            //make sure they have the name filled in
            if (ValidateFields())
            {
                this.Cursor = Cursors.WaitCursor;
                errorMessage = DAC.AddACustomer(txtFirstName.Text, txtLastName.Text, txtFavoriteMovie.Text, txtFavoriteLanguage.Text);
                this.Cursor = Cursors.Default;
            }
            if (errorMessage.Length > 0)
                MessageBox.Show(errorMessage);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            if (ValidateFields())
            {
                this.Cursor = Cursors.WaitCursor;
                errorMessage = DAC.UpdateFavoritesByName(txtFirstName.Text, txtLastName.Text, txtFavoriteMovie.Text, txtFavoriteLanguage.Text);
                this.Cursor = Cursors.Default;
            }
            if (errorMessage.Length > 0)
                MessageBox.Show(errorMessage);
        }

        private void btnGetFavorites_Click(object sender, EventArgs e)
        {
            string favoriteMovie = string.Empty;
            string favoriteLanguage = string.Empty;
            string errorMessage = string.Empty;
            if (ValidateFields())
            {
                this.Cursor = Cursors.WaitCursor;
                errorMessage = DAC.GetFavoritesForCustomer(out favoriteMovie, out favoriteLanguage, txtFirstName.Text, txtLastName.Text);
                this.Cursor = Cursors.Default;
                if (errorMessage.Length > 0)
                {
                    MessageBox.Show(errorMessage);
                    txtFavoriteLanguage.Text = string.Empty;
                    txtFavoriteMovie.Text = string.Empty;
                }
                else
                {
                    txtFavoriteLanguage.Text = favoriteLanguage;
                    txtFavoriteMovie.Text = favoriteMovie;
                }
            }

        }

        private void btnGetCustomerList_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ds_customers = new DataSet();
            string errorMessage = DAC.GetCustomerList(out ds_customers);
            if (errorMessage.Length == 0)
            {
                //populate the grid
                if (ds_customers != null && ds_customers.Tables.Count > 0)
                {
                    bs_customers = new BindingSource();
                    bs_customers.DataSource = ds_customers.Tables[0];
                    dgvCustomers.DataSource = bs_customers;
                }
                dgvCustomers.AutoResizeColumns();
            }
            else
                MessageBox.Show(errorMessage);
            this.Cursor = Cursors.Default;
        }

        private void btnAddToQueue_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                this.Cursor = Cursors.WaitCursor;
                string errorMessage = DAC.AddToQueue(txtFirstName.Text, txtLastName.Text);
                if (errorMessage.Length > 0)
                    MessageBox.Show(errorMessage);
                this.Cursor = Cursors.Default;
            }

        }

        private bool ValidateFields()
        {
            bool success = true;
            txtLastName.Text = txtLastName.Text.Trim();
            txtFirstName.Text = txtFirstName.Text.Trim();
            txtFavoriteMovie.Text = txtFavoriteMovie.Text.Trim();
            txtFavoriteLanguage.Text = txtFavoriteLanguage.Text.Trim();

            if (txtLastName.Text.Length == 0 || txtFirstName.Text.Length == 0)
            {
                success = false;
                MessageBox.Show("Must specify First Name and Last Name.");
            }

            return success;

        }


    }
}
