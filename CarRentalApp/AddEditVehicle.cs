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

    public partial class AddEditVehicle : Form
    {
        private readonly CarRentalEntities carRentalEntities;
        private ManageVehicleListing _manageVehicleListing;
        private bool isEditMode;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = " Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            carRentalEntities = new CarRentalEntities();
        }

        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            carRentalEntities = new CarRentalEntities();
            PopulateFields(carToEdit);
            isEditMode = true;

        }

        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString();
            tbBrand.Text = car.Brand;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicense.Text = car.LicensePlateNumber;
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbBrand.Text) || string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Please ensure u provide a make and a model");
                }
                if (isEditMode)
                {
                    // edit code here
                    var id = int.Parse(lblId.Text);
                    var car = carRentalEntities.TypesOfCars.FirstOrDefault(q => q.Id == id);
                    car.Model = tbModel.Text;
                    car.Brand = tbBrand.Text;
                    car.VIN = tbVIN.Text;
                    car.Year = int.Parse(tbYear.Text);
                    car.LicensePlateNumber = tbLicense.Text;

               
                    
                }
                else
                {
                    // add code here
                    var newCar = new TypesOfCar
                    {
                        LicensePlateNumber = tbLicense.Text,
                        Brand = tbBrand.Text,
                        Model = tbModel.Text,
                        VIN = tbVIN.Text,
                        Year = int.Parse(tbYear.Text)
                    };
                    carRentalEntities.TypesOfCars.Add(newCar);

                    carRentalEntities.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation completed.Refresh grid to see Changes");
                    Close();

                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
