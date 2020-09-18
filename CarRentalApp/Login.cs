using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{

    public partial class Login : Form
    {
        private readonly CarRentalEntities carRentalEntities;
        public Login()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = tbUsername.Text.Trim();
                var password = tbPassword.Text;


                var hashed_password = Utils.hashPassword(password);

                var user = carRentalEntities.Users.FirstOrDefault(q => q.username == username && q.password == password);
                if(user == null)
                {
                    MessageBox.Show("Please provide valid credentials");
                }
                else
                {
                    var role = user.UserRoles.FirstOrDefault();
                    var roleShortName = role.Role.shortname;
                    var mainWindow = new MainWindow(this, user );
                    mainWindow.Show();
                    Hide();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Something went wrong please try again");
            }
        }
    }
}
