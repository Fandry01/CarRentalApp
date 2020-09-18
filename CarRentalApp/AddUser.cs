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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities carRentalEntities;
        private ManageUsers _manageUsers;
        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = carRentalEntities.Roles.ToList();
            cbRoles.DataSource = roles;
            cbRoles.ValueMember = "id";
            cbRoles.DisplayMember = "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUserName.Text;
                var roleId = (int)cbRoles.SelectedValue;
                var password = Utils.DefaulthashPassword();
                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true
                };

                carRentalEntities.Users.Add(user);
                carRentalEntities.SaveChanges();

                var userid = user.id;
                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userid
                };
                carRentalEntities.UserRoles.Add(userRole);
                carRentalEntities.SaveChanges();

                MessageBox.Show(" new user added succesfully");
                _manageUsers.PopulateGrid();
                Close();

            }
            catch (Exception)
            {

                MessageBox.Show("an Error has occured");
            }
        }
    }
}
