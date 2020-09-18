using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageUsers : Form
    {

        private readonly CarRentalEntities carRentalEntities;
        public ManageUsers()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }

        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;
                var user = carRentalEntities.Users.FirstOrDefault(q => q.id == id);
                var hashed_password = Utils.DefaulthashPassword();
                user.password = hashed_password;
                carRentalEntities.SaveChanges();

                MessageBox.Show($"{user.username} password has been reset!");
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;
                var user = carRentalEntities.Users.FirstOrDefault(q => q.id == id);

                user.isActive = user.isActive == true ? false : true; 
                carRentalEntities.SaveChanges();

                MessageBox.Show($"{user.username} Active status has changed !");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void gvUserList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            PopulateGrid();
        }


        public void PopulateGrid()
        {
            var userRecords = carRentalEntities.Users.Select(q => new
            {
                id = q.id,
                username = q.username,
                userrole= q.UserRoles.FirstOrDefault().Role.name,
                isactive = q.isActive,
                
            }).ToList();
            gvUserList.DataSource = userRecords;
            gvUserList.Columns["username"].HeaderText = "Username";
            gvUserList.Columns["userRole"].HeaderText = "RoleName";
            gvUserList.Columns["isActive"].HeaderText = "Active";
            gvUserList.Columns["id"].Visible = false;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
