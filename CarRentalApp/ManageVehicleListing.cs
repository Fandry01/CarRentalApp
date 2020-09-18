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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities carRentalEntities;
        public ManageVehicleListing()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            //  var cars = carRentalEntities.TypesOfCars.ToList();
            var cars = carRentalEntities.TypesOfCars.Select(q => new { Brand= q.Brand, Model = q.Model, VIN = q.VIN, Year = q.Year,
                licensePlateNumber = q.LicensePlateNumber, q.Id}).ToList();
            gvVehicleList.DataSource = cars;
         
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            gvVehicleList.Columns[5].Visible = false;
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            
                AddEditVehicle addEditVehicle = new AddEditVehicle(this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            
            
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;
                var car = carRentalEntities.TypesOfCars.FirstOrDefault(q => q.Id == id);
                var addEditVehicle = new AddEditVehicle(car, this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;
                var car = carRentalEntities.TypesOfCars.FirstOrDefault(q => q.Id == id);

                DialogResult dr = MessageBox.Show("Are you sure", "delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
               if(dr == DialogResult.Yes)
                {
                    carRentalEntities.TypesOfCars.Remove(car);
                    carRentalEntities.SaveChanges();
                }


                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }
        
        public void PopulateGrid()
        {
            var cars = carRentalEntities.TypesOfCars.Select(
                q => new
                {
                   Brand = q.Brand,
                   Model = q.Model,
                   VIN = q.VIN,
                   Year = q.Year,
                   LicensePlateNumber  = q.LicensePlateNumber,
                   q.Id
                }) .ToList();
            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            gvVehicleList.Columns["Id"].Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid(); 

        }


    }
}
