using BtrGudang.AppTier.PackingOrderFeature;
using BtrGudang.Domain.PackingOrderFeature;
using BtrGudang.Helper.Common;
using BtrGudang.Winform.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BtrGudang.Winform.Forms
{
    public partial class PK1PrintPackingOrderForm : Form
    {
        private readonly IPackingOrderRepo _packingOrderRepo;
        private readonly BindingList<PK1AllFakturView> _allFakturList;
        private readonly BindingSource _allFakturBindingSource;

        public PK1PrintPackingOrderForm(IPackingOrderRepo packingOrderRepo)
        {
            InitializeComponent();
            _packingOrderRepo = packingOrderRepo;
            _allFakturList = new BindingList<PK1AllFakturView>();
            _allFakturBindingSource = new BindingSource(_allFakturList, null);

            RegisterControlEventHandler();
            InitGrid();
        }


        private void RegisterControlEventHandler()
        {
            ListFakturButton.Click += ListFakturButton_Click;
        }

        private void InitGrid()
        {
            AllFakturGrid.DataSource = _allFakturBindingSource;
            AllFakturGrid.AutoGenerateColumns = true;
            AllFakturGrid.DataSource = _allFakturBindingSource;

            foreach (DataGridViewColumn col in AllFakturGrid.Columns)
            {
                if (col.ValueType == typeof(Uri))
                {
                    DataGridViewLinkColumn linkCol = new DataGridViewLinkColumn
                    {
                        Name = col.Name,
                        HeaderText = col.HeaderText,
                        DataPropertyName = col.DataPropertyName,
                        UseColumnTextForLinkValue = false,
                    };
                    int index = col.Index;
                    AllFakturGrid.Columns.Remove(col);
                    AllFakturGrid.Columns.Insert(index, linkCol);
                    break;
                }
            }
            AllFakturGrid.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && AllFakturGrid.Columns[e.ColumnIndex].Name == "MapLocation")
                {
                    var location = AllFakturGrid.Rows[e.RowIndex].DataBoundItem as PK1AllFakturView;
                    if (location.MapLocation != null)
                    {
                        System.Diagnostics.Process.Start(location.MapLocation.AbsoluteUri);
                    }
                }
            };
            AllFakturGrid.CellContentClick += (s,e) =>
            {
                var grid = (DataGridView)s;
                if (!(grid.CurrentCell is DataGridViewCheckBoxCell))
                    return;

                grid.EndEdit();
            };
            _allFakturBindingSource.ListChanged += AllFakturBindingSource_ListChanged;
        }

        private void AllFakturBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                if (_allFakturBindingSource[e.NewIndex] is PK1AllFakturView item)
                    AllFakturGrid.InvalidateRow(e.NewIndex);
            }
        }

        private void ListFakturButton_Click(object sender, EventArgs e)
        {
            var tgl1 = Tgl1DatePicker.Value;
            var tgl2 = Tgl2DatePicker.Value;
            var periode = new Periode(tgl1, tgl2);
            var listDataView = _packingOrderRepo.ListData(periode)?.ToList() 
                ?? new List<PackingOrderView>();
            var listModel = listDataView.Select(x => _packingOrderRepo
                .LoadEntity(x).Value)?.ToList() ?? new List<PackingOrderModel>();

            _allFakturList.Clear();
            var listViewPK1 = listModel.Select(x => PK1AllFakturView.CreateFrom(x));
            foreach(var item in listViewPK1)
                _allFakturList.Add(item);
        }
    }

    public class PK1AllFakturView : IPackingOrderKey, INotifyPropertyChanged
    {
        private bool _isPersupplier = true;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PK1AllFakturView(string packingOrderId, 
            string fakturCode, DateTime fakturDate, string adminName, 
            string customerCode, string customerName, string alamat, Uri mapLocation)
        {
            PackingOrderId = packingOrderId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
            AdminName = adminName;
            CustomerCode = customerCode;
            CustomerName = customerName;
            Alamat = alamat;
            MapLocation = mapLocation;
            _isPersupplier = true;
        }
        public string PackingOrderId { get; private set;}
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set;  }
        public string AdminName { get; private set; }
        public string CustomerCode { get; private set; }
        public string CustomerName { get; private set; }
        public string Alamat { get; private set;  }
        public Uri MapLocation { get; private set; }
        public bool PerSupplier { get => _isPersupplier; 
            set 
            { 
                _isPersupplier = value;
                OnPropertyChanged(nameof(PerSupplier));
                OnPropertyChanged(nameof(PerFaktur)); 
            } 
        }
        public bool PerFaktur { get => !_isPersupplier; 
            set
            {
                _isPersupplier = !value;
                OnPropertyChanged(nameof(PerFaktur));
                OnPropertyChanged(nameof(PerSupplier));
            }
        }
        public static PK1AllFakturView CreateFrom(PackingOrderModel view)
        {
            Uri mapLocation = null;
            if (view.Location.Latitude != 0 || view.Location.Longitude != 0)
            {
                mapLocation = new Uri(string.Format(CultureInfo.InvariantCulture,
                    "https://www.google.com/maps?q={0},{1}",
                    view.Location.Latitude,
                    view.Location.Longitude));


            }
            var result = new PK1AllFakturView(view.PackingOrderId, view.Faktur.FakturCode,
                    view.Faktur.FakturDate, view.Faktur.AdminName, view.Customer.CustomerCode,
                    view.Customer.CustomerName, view.Customer.Alamat, mapLocation);
            return result;
        }
    }
}
