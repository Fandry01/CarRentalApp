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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities carRentalEntities;
        public ManageRentalRecords()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
        }
        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void PopulateGrid()
        {
            var records = carRentalEntities.CarRentalRecords.Select(q => new
            {
                Customer = q.CustomerName,
                DateOut = q.DateRented,
                DateIn = q.DateReturned,
                Id = q.id,
                Cost = q.Cost,
                Car = q.TypesOfCar.Brand + " " + q.TypesOfCar.Model
             }).ToList();

            gvRecordList.DataSource = records;

            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date out";
            gvRecordList.Columns["Id"].Visible = false;
        }
        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            AddEditVehicle addEditVehicle = new AddEditVehicle();
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                var record = carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.id == id);

                var addEditRentalRecord = new AddEditRentalRecord(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();


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
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;
                var record = carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.id == id);
                carRentalEntities.CarRentalRecords.Remove(record);
                carRentalEntities.SaveChanges();

                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void gvRecordList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
