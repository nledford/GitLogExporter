using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GitLogExporterCore.Extensions;

namespace GitLogExporterGUI {
    public partial class Main : Form {
        public Main() {
            InitializeComponent();
        }

        private DateTime _start;
        private DateTime _end;

        private void Main_Load(object sender, EventArgs e) {
            InitializeDates();

            dateFrom.Value = _start;
            dateTo.Value = _end;
        }

        private void InitializeDates() {
            _start = DateTime.Now.DayOfWeek == DayOfWeek.Monday
                         ? DateTime.Now
                         : DateTime.Today.Previous(DayOfWeek.Monday);

            _end = DateTime.Now.DayOfWeek == DayOfWeek.Saturday ? DateTime.Now : DateTime.Today.Next(DayOfWeek.Saturday);
        }
    }
}
