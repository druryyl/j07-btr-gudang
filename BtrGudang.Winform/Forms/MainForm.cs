using BtrGudang.Winform.BtrGudang.Winform.Services;
using PackingOrderDownloader;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BtrGudang.Winform
{
    public partial class MainForm : Form
    {
        private readonly IFormFactory _formFactory;

        public MainForm(IFormFactory formFactory)
        {
            InitializeComponent();

            _formFactory = formFactory;
        }

        private void PT1DownloadPackingOrderMenu_Click(object sender, EventArgs e)
        {
            if (BringMdiChildToFrontIfLoaded<DownloadPackingOrder2Form>())
                return;

            var form = _formFactory.CreateForm<DownloadPackingOrder2Form>();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MdiParent = this;
            form.Show();
        }
        private bool BringMdiChildToFrontIfLoaded<T>() where T : Form
        {
            var loadedForm = this.MdiChildren.OfType<T>().FirstOrDefault();
            if (loadedForm != null)
            {
                loadedForm.BringToFront();
                return true;
            }
            return false;
        }
    }
}
