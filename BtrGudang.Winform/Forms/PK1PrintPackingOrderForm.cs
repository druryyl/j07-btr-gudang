using BtrGudang.AppTier.PackingOrderFeature;
using BtrGudang.Domain.PackingOrderFeature;
using BtrGudang.Helper.Common;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
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
            PrintPerFakturButton.Click += PrintPerFakturButton_Click;
            PrintPerSupplierButton.Click += PrintPerSupplierButton_Click;
            FilterTextBox.TextChanged += FilterText_TextChanged;
        }

        private void PrintPerSupplierButton_Click(object sender, EventArgs e)
        {
            ProsesPerSupplier();
        }

        #region Proses Per-Supplier
        private void ProsesPerSupplier()
        {
            var listModel = _allFakturList
                .Where(x => x.PerSupplier)
                .Select(x => _packingOrderRepo.LoadEntity(x).Value);
            var listPerSupplier = PrintPackingOrderPerSupplierView
                .CreateFrom(listModel)?.ToList() 
                ?? new List<PrintPackingOrderPerSupplierView>();
            ExportPerSupplierToExcel(listPerSupplier);

        }
        private void ExportPerSupplierToExcel(IEnumerable<PrintPackingOrderPerSupplierView> data)
        {
            if (data == null || !data.Any())
            {
                MessageBox.Show(@"No data to export", @"Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //  Get file path from save dialog
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"packing-order-per-supplier-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("Packing Order Per Supplier");

                int currentRow = 3;
                int no = 1;

                foreach (var supplier in data)
                {
                    //  Supplier header
                    ws.Cell(currentRow, 1).Value = $"Supplier: {supplier.Supplier}";
                    ws.Range(currentRow, 1, currentRow, 8).Merge().Style
                        .Font.SetBold(true)
                        .Font.SetFontSize(11)
                        .Fill.SetBackgroundColor(XLColor.LightGray);
                    currentRow++;

                    foreach (var kategori in supplier.ListKategori)
                    {
                        //  Kategori header
                        ws.Cell(currentRow, 2).Value = $"Kategori: {kategori.Kategori}";
                        ws.Range(currentRow, 2, currentRow, 8).Merge().Style
                            .Font.SetBold(true)
                            .Font.SetFontColor(XLColor.DarkBlue)
                            .Fill.SetBackgroundColor(XLColor.LightBlue);
                        currentRow++;

                        //  Table headers for items
                        ws.Cell(currentRow, 2).Value = "No";
                        ws.Cell(currentRow, 3).Value = "Brg Code";
                        ws.Cell(currentRow, 4).Value = "Brg Name";
                        ws.Cell(currentRow, 5).Value = "Qty Besar";
                        ws.Cell(currentRow, 6).Value = "Sat Besar";
                        ws.Cell(currentRow, 7).Value = "Qty Kecil";
                        ws.Cell(currentRow, 8).Value = "Sat Kecil";

                        //  Style header row
                        ws.Range(currentRow, 2, currentRow, 8).Style
                            .Font.SetBold(true)
                            .Fill.SetBackgroundColor(XLColor.LightGray)
                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                            .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        int itemRow = currentRow + 1;
                        int itemNo = 1;

                        foreach (var brg in kategori.ListBrg)
                        {
                            ws.Cell(itemRow, 2).Value = itemNo++;
                            ws.Cell(itemRow, 3).Value = brg.BrgCode;
                            ws.Cell(itemRow, 4).Value = brg.BrgName;
                            ws.Cell(itemRow, 5).Value = brg.SumQtyBesar;
                            ws.Cell(itemRow, 6).Value = brg.SatBesar;
                            ws.Cell(itemRow, 7).Value = brg.SumQtyKecil;
                            ws.Cell(itemRow, 8).Value = brg.SatKecil;

                            //  Apply borders to item row
                            ws.Range(itemRow, 2, itemRow, 8).Style
                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                            //  Right-align numbers
                            ws.Cell(itemRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            ws.Cell(itemRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                            itemRow++;
                        }

                        //  Add empty row between categories
                        if (kategori.ListBrg.Any())
                        {
                            //  Add summary row for kategori
                            ws.Cell(itemRow, 4).Value = $"Total {kategori.Kategori}:";
                            ws.Cell(itemRow, 5).FormulaA1 = $"=SUM(E{itemRow - kategori.ListBrg.Count()}:E{itemRow - 1})";
                            ws.Cell(itemRow, 7).FormulaA1 = $"=SUM(G{itemRow - kategori.ListBrg.Count()}:G{itemRow - 1})";

                            ws.Range(itemRow, 4, itemRow, 5).Style
                                .Font.SetBold(true)
                                .Fill.SetBackgroundColor(XLColor.LightYellow);
                            ws.Range(itemRow, 7, itemRow, 7).Style
                                .Font.SetBold(true)
                                .Fill.SetBackgroundColor(XLColor.LightYellow);

                            itemRow++;
                        }

                        currentRow = itemRow;
                    }

                    //  Add empty row between suppliers
                    currentRow++;
                }

                //  Adjust column widths
                ws.Column(1).Width = 3;    // For grouping indentation
                ws.Column(2).Width = 5;    // No
                ws.Column(3).Width = 15;   // Brg Code
                ws.Column(4).Width = 30;   // Brg Name
                ws.Column(5).Width = 12;   // Qty Besar
                ws.Column(6).Width = 10;   // Sat Besar
                ws.Column(7).Width = 12;   // Qty Kecil
                ws.Column(8).Width = 10;   // Sat Kecil

                //  Add title and date
                ws.Cell(1, 1).Value = "Packing Order Per Supplier Report";
                ws.Cell(2, 1).Value = $"Generated: {DateTime.Now:dd-MM-yyyy HH:mm:ss}";
                ws.Range(1, 1, 2, 8).Style.Font.SetBold(true);

                //  Apply number formats
                ws.RangeUsed().Style.Font.SetFontName("Lucida Console").Font.SetFontSize(9);

                //  Save and open
                wb.SaveAs(filePath);
            }

            System.Diagnostics.Process.Start(filePath);
        }
        #endregion

        #region Proses Per-Faktur
        private void PrintPerFakturButton_Click(object sender, EventArgs e)
        {
            var listModel = _allFakturList
                .Where(x => x.PerFaktur)
                .Select(x => _packingOrderRepo.LoadEntity(x).Value);
            var listPerFaktur = PrintPackingOrderPerFakturView
                .CreateFrom(listModel)?.ToList()
                ?? new List<PrintPackingOrderPerFakturView>();
            ExportPerFakturToExcel(listPerFaktur);
        }
        private void ExportPerFakturToExcel(IEnumerable<PrintPackingOrderPerFakturView> data)
        {
            if (data == null || !data.Any())
            {
                MessageBox.Show(@"No data to export", @"Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //  Get file path from save dialog
            string filePath;
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"Excel Files|*.xlsx";
                saveFileDialog.Title = @"Save Excel File";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = $"packing-order-per-faktur-{DateTime.Now:yyyy-MM-dd-HHmm}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                filePath = saveFileDialog.FileName;
            }

            using (IXLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("Packing Order Per Faktur");

                int currentRow = 1;

                //  Title
                ws.Cell(currentRow, 1).Value = "PACKING ORDER PER FAKTUR REPORT";
                ws.Range(currentRow, 1, currentRow, 8).Merge().Style
                    .Font.SetBold(true)
                    .Font.SetFontSize(14)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                currentRow += 2;

                foreach (var faktur in data)
                {
                    //  Faktur Header Section - All headers on the left
                    //  Row 1: Faktur Code and Date
                    ws.Cell(currentRow, 1).Value = "Faktur Code / Date:";
                    ws.Cell(currentRow, 2).Value = $"{faktur.FakturCode} / {faktur.FakturDate}";
                    ws.Range(currentRow, 2, currentRow, 8).Merge();
                    currentRow++;

                    //  Row 2: Customer Code and Name
                    ws.Cell(currentRow, 1).Value = "Customer Code / Name:";
                    ws.Cell(currentRow, 2).Value = $"{faktur.CustomerCode} / {faktur.CustomerName}";
                    ws.Range(currentRow, 2, currentRow, 8).Merge();
                    currentRow++;

                    //  Row 3: Customer Address
                    ws.Cell(currentRow, 1).Value = "Customer Address:";
                    ws.Cell(currentRow, 2).Value = faktur.Alamat;
                    ws.Range(currentRow, 2, currentRow, 8).Merge();
                    currentRow++;

                    //  Row 4: Map Location
                    ws.Cell(currentRow, 1).Value = "Map Location:";
                    if (!string.IsNullOrEmpty(faktur.Location) && faktur.Location != "-")
                    {
                        ws.Cell(currentRow, 2).Value = "Click to open in Google Maps";
                        ws.Range(currentRow, 2, currentRow, 8).Merge();

                        //  Make it a hyperlink
                        var linkCell = ws.Cell(currentRow, 2);
                        linkCell.SetHyperlink(new XLHyperlink(faktur.Location));
                        linkCell.Style.Font.SetFontColor(XLColor.Blue).Font.SetUnderline();
                    }
                    else
                    {
                        ws.Cell(currentRow, 2).Value = "-";
                        ws.Range(currentRow, 2, currentRow, 8).Merge();
                    }

                    //  Style all header labels
                    ws.Range(currentRow - 3, 1, currentRow, 1).Style
                        .Font.SetBold(true)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    //  Style all header values (merged cells)
                    ws.Range(currentRow - 3, 2, currentRow, 8).Style
                        .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                        .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                        .Fill.SetBackgroundColor(XLColor.LightBlue)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    currentRow += 2; // Add space before items

                    //  Items Table Header - Starting at Column B
                    ws.Cell(currentRow, 2).Value = "No";
                    ws.Cell(currentRow, 3).Value = "Brg Code";
                    ws.Cell(currentRow, 4).Value = "Brg Name";
                    ws.Cell(currentRow, 5).Value = "Category - Supplier";
                    ws.Cell(currentRow, 6).Value = "Qty Besar";
                    ws.Cell(currentRow, 7).Value = "Qty Kecil";

                    //  Style table header
                    ws.Range(currentRow, 2, currentRow, 7).Style
                        .Font.SetBold(true)
                        .Fill.SetBackgroundColor(XLColor.LightGray)
                        .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                        .Border.SetInsideBorder(XLBorderStyleValues.Hair)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    int itemRow = currentRow + 1;
                    int itemNo = 1;

                    //  Items Data - Starting at Column B
                    foreach (var brg in faktur.ListBrg)
                    {
                        ws.Cell(itemRow, 2).Value = itemNo++;
                        ws.Cell(itemRow, 3).Value = brg.BrgCode;
                        ws.Cell(itemRow, 4).Value = brg.BrgName;
                        ws.Cell(itemRow, 5).Value = brg.KategoriSupplier;
                        ws.Cell(itemRow, 6).Value = brg.QtyBesar;
                        ws.Cell(itemRow, 7).Value = brg.QtyKecil;

                        //  Apply borders to item row
                        ws.Range(itemRow, 2, itemRow, 7).Style
                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                            .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                        //  Center align the number column
                        ws.Cell(itemRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        itemRow++;
                    }

                    //  Add summary row for this faktur
                    if (faktur.ListBrg.Any())
                    {
                        ws.Cell(itemRow, 5).Value = $"Total Items: {faktur.ListBrg.Count()}";
                        ws.Range(itemRow, 5, itemRow, 7).Merge().Style
                            .Font.SetBold(true)
                            .Fill.SetBackgroundColor(XLColor.LightYellow)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        itemRow++;
                    }

                    currentRow = itemRow + 2; // Add 2 empty rows between fakturs
                }

                //  Adjust column widths
                ws.Column(1).Width = 20;  // Header labels
                ws.Column(2).Width = 5;   // No (starting at B)
                ws.Column(3).Width = 15;  // Brg Code
                ws.Column(4).Width = 30;  // Brg Name
                ws.Column(5).Width = 35;  // Category - Supplier
                ws.Column(6).Width = 15;  // Qty Besar
                ws.Column(7).Width = 15;  // Qty Kecil
                ws.Column(8).Width = 2;   // Spacing

                //  Apply font to all cells
                ws.RangeUsed().Style.Font.SetFontName("Lucida Console").Font.SetFontSize(9);

                //  Add generation info at the bottom
                int lastRow = ws.LastRowUsed().RowNumber();
                ws.Cell(lastRow + 2, 1).Value = $"Report generated: {DateTime.Now:dd-MM-yyyy HH:mm:ss}";
                ws.Range(lastRow + 2, 1, lastRow + 2, 7).Merge().Style
                    .Font.SetItalic(true)
                    .Font.SetFontColor(XLColor.Gray);

                //  Save and open
                wb.SaveAs(filePath);
            }

            System.Diagnostics.Process.Start(filePath);
        }        
        #endregion

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

        private void FilterText_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filterText = FilterTextBox.Text.Trim();

            if (string.IsNullOrEmpty(filterText))
            {
                _allFakturBindingSource.DataSource = _allFakturList;
            }
            else
            {
                var filtered = _allFakturList.Where(x =>
                    (!string.IsNullOrEmpty(x.CustomerName) && x.CustomerName.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(x.CustomerCode) && x.CustomerCode.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(x.FakturCode) && x.FakturCode.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(x.Alamat) && x.Alamat.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();

                _allFakturBindingSource.DataSource = new BindingList<PK1AllFakturView>(filtered);
            }
        }
    }

    #region Binding Data Source View Structure
    public class PK1AllFakturView : IPackingOrderKey, INotifyPropertyChanged
    {
        private bool? _isPersupplier; // Changed to nullable to allow all false state
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
            _isPersupplier = null; // Initially both false
        }

        public string PackingOrderId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string AdminName { get; private set; }
        public string CustomerCode { get; private set; }
        public string CustomerName { get; private set; }
        public string Alamat { get; private set; }
        public Uri MapLocation { get; private set; }

        public bool PerSupplier
        {
            get => _isPersupplier == true;
            set
            {
                if (value)
                {
                    // Setting PerSupplier to true means PerFaktur must be false
                    _isPersupplier = true;
                }
                else if (_isPersupplier == true)
                {
                    // Turning off PerSupplier when it was true means both become false
                    _isPersupplier = null;
                }
                // If value is false and _isPersupplier wasn't true, do nothing

                OnPropertyChanged(nameof(PerSupplier));
                OnPropertyChanged(nameof(PerFaktur));
            }
        }

        public bool PerFaktur
        {
            get => _isPersupplier == false;
            set
            {
                if (value)
                {
                    // Setting PerFaktur to true means PerSupplier must be false
                    _isPersupplier = false;
                }
                else if (_isPersupplier == false)
                {
                    // Turning off PerFaktur when it was true means both become false
                    _isPersupplier = null;
                }
                // If value is false and _isPersupplier wasn't false, do nothing

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
#endregion

    #region PrintOut Per-Supplier View Structure
    public class PrintPackingOrderPerSupplierView
    {
        public string Supplier { get; set; }
        public IEnumerable<PrintPackingOrderPerSupplierKategoriView> ListKategori { get; set; }

        public static IEnumerable<PrintPackingOrderPerSupplierView> CreateFrom(IEnumerable<PackingOrderModel> packingOrders)
        {
            if (packingOrders == null)
                return Enumerable.Empty<PrintPackingOrderPerSupplierView>();

            // Flatten all items from all packing orders and group by Supplier and Kategori
            var result = packingOrders
                .Where(po => po != null) // Skip null packing orders
                .SelectMany(po => po.ListItem.Select(item => new
                {
                    po.PackingOrderId,
                    po.PackingOrderDate,
                    item,
                    item.Brg.Supplier,
                    item.Brg.Kategori,
                    item.Brg.BrgId,
                    item.Brg.BrgCode,
                    item.Brg.BrgName,
                    QtyBesar = item.QtyBesar.Qty,
                    SatBesar = item.QtyBesar.Satuan,
                    QtyKecil = item.QtyKecil.Qty,
                    SatKecil = item.QtyKecil.Satuan
                }))
                .GroupBy(x => x.Supplier) // First group by Supplier
                .Select(supplierGroup => new PrintPackingOrderPerSupplierView
                {
                    Supplier = supplierGroup.Key,
                    ListKategori = supplierGroup
                        .GroupBy(x => x.Kategori) // Then group by Kategori within each Supplier
                        .Select(kategoriGroup => new PrintPackingOrderPerSupplierKategoriView
                        {
                            Kategori = kategoriGroup.Key,
                            ListBrg = kategoriGroup
                                .GroupBy(x => new { x.BrgId, x.BrgCode, x.BrgName, x.SatBesar, x.SatKecil }) // Group by item details for summation
                                .Select(brgGroup => new PrintPackingOrderPerSupplierBrgView
                                {
                                    BrgId = brgGroup.Key.BrgId,
                                    BrgCode = brgGroup.Key.BrgCode,
                                    BrgName = brgGroup.Key.BrgName,
                                    SatBesar = brgGroup.Key.SatBesar,
                                    SatKecil = brgGroup.Key.SatKecil,
                                    SumQtyBesar = brgGroup.Sum(x => x.QtyBesar),
                                    SumQtyKecil = brgGroup.Sum(x => x.QtyKecil)
                                })
                                .OrderBy(brg => brg.BrgCode) // Optional ordering
                                .ToList()
                        })
                        .OrderBy(kat => kat.Kategori) // Optional ordering
                        .ToList()
                })
                .OrderBy(supp => supp.Supplier) // Optional ordering
                .ToList();

            return result;
        }
    }

    public class PrintPackingOrderPerSupplierKategoriView
    {
        public string Kategori { get; set; }
        public IEnumerable<PrintPackingOrderPerSupplierBrgView> ListBrg { get; set; }
    }

    public class PrintPackingOrderPerSupplierBrgView
    {
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public int SumQtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int SumQtyKecil { get; set; }
        public string SatKecil { get; set; }
    }
#endregion

    #region PrintOut Per-Faktur View Structure
    public class PrintPackingOrderPerFakturView
    {
        public string FakturCode { get; set; }
        public string FakturDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public string Location { get; set; }
        public IEnumerable<PrintPackingOrderPerFakturBrgView> ListBrg { get; set; }

        public static IEnumerable<PrintPackingOrderPerFakturView> CreateFrom(IEnumerable<PackingOrderModel> packingOrders)
        {
            if (packingOrders == null)
                return Enumerable.Empty<PrintPackingOrderPerFakturView>();

            var culture = CultureInfo.InvariantCulture;

            var result = packingOrders
                .Where(po => po != null)
                .Select(po => new PrintPackingOrderPerFakturView
                {
                    FakturCode = po.Faktur?.FakturCode ?? "-",
                    FakturDate = po.Faktur?.FakturDate.ToString("dd-MM-yyyy") ?? "-",
                    CustomerCode = po.Customer?.CustomerCode ?? "-",
                    CustomerName = po.Customer?.CustomerName ?? "-",
                    Alamat = po.Customer?.Alamat ?? "-",
                    Location = GenerateMapLocation(po.Location, culture),
                    ListBrg = po.ListItem?
                        .Where(item => item?.Brg != null)
                        .Select(item => new PrintPackingOrderPerFakturBrgView
                        {
                            BrgId = item.Brg.BrgId ?? "-",
                            BrgCode = item.Brg.BrgCode ?? "-",
                            BrgName = item.Brg.BrgName ?? "-",
                            KategoriSupplier = $"{item.Brg.Kategori ?? "-"} - {item.Brg.Supplier ?? "-"}",
                            QtyBesar = $"{item.QtyBesar?.Qty ?? 0} {item.QtyBesar?.Satuan ?? "-"}",
                            QtyKecil = $"{item.QtyKecil?.Qty ?? 0} {item.QtyKecil?.Satuan ?? "-"}"
                        })
                        .OrderBy(brg => brg.BrgCode)
                        .ToList() ?? Enumerable.Empty<PrintPackingOrderPerFakturBrgView>()
                })
                .OrderBy(faktur => faktur.FakturCode)
                .ToList();

            return result;
        }

        private static string GenerateMapLocation(LocationReff location, CultureInfo culture)
        {
            if (location == null)
                return "-";

            // Check if location has valid coordinates (assuming 0,0 means no location)
            if (location.Latitude == 0 && location.Longitude == 0)
                return "-";

            try
            {
                // Create Google Maps link
                var mapUri = new Uri(string.Format(culture,
                    "https://www.google.com/maps?q={0},{1}",
                    location.Latitude,
                    location.Longitude));

                return mapUri.ToString();
            }
            catch
            {
                return "-";
            }
        }
    }

    public class PrintPackingOrderPerFakturBrgView
    {
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string KategoriSupplier { get; set; }
        public string QtyBesar { get; set; }
        public string QtyKecil { get; set; }
    }
    #endregion
}
