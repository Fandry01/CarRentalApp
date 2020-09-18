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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities carRentalEntities;
        public AddEditRentalRecord()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
            lblTitle.Text = " Add New Rental";
            this.Text = "add new Rental";
            isEditMode = false;
            carRentalEntities = new CarRentalEntities();
        }
        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "edit Rental Record";
            if(recordToEdit == null)
            {
                MessageBox.Show("have you selected the valid record to edit?");
            }
            else
            {
                carRentalEntities = new CarRentalEntities();
                PopulateFields(recordToEdit);
                isEditMode = true;
            }
         
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
             tbCustomerName.Text =recordToEdit.CustomerName;
             dpRented.Value = (DateTime)recordToEdit.DateRented;
             dpReturn.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.id.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dpRented.Value;
                var dateIn = dpReturn.Value;
                var carType = cbTypeCar.Text;
                double cost = Convert.ToDouble(tbCost.Text);

                var isvalid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isvalid = false;
                   errorMessage += "Error: Please enter missing data.\n\r";
                }

                if (dateOut > dateIn)
                {
                    isvalid = false;
                    errorMessage += "fill in valid dates";
                }

                if (isvalid == true)
                {
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.id == id);
                       
                    }
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCar = (int)cbTypeCar.SelectedValue;

                    if (!isEditMode)
                        carRentalEntities.CarRentalRecords.Add(rentalRecord);
                    carRentalEntities.SaveChanges();

                    MessageBox.Show($"thank you for renting: {customerName}" +
                   $" this is your order date :{dateOut} and return date: {dateIn}" +
                   $" and the car you rented {carType}" +
                   $"price of your reservation is :{cost}");

                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var cars = carRentalEntities.TypesOfCars.ToList();
            var cars = carRentalEntities.TypesOfCars
                .Select(q => new { Id = q.Id, Name = q.Brand + "" + q.Model }).ToList();
            cbTypeCar.DisplayMember = "Name";
            cbTypeCar.ValueMember = "Id";
            cbTypeCar.DataSource = cars;
        }

       
    }
}
