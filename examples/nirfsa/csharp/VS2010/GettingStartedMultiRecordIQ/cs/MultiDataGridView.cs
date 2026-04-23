using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NationalInstruments;

namespace NationalInstruments.Examples.GettingStartedMultiRecordIQ
{
    public partial class MultiDataGridView : UserControl
    {

        ComplexDouble[,] dataSource;
        int currentIndex;
        bool updateData;

        public MultiDataGridView()
        {
            InitializeComponent();
        }

        private void BindData(int currentIndex)
        {

            this.dataGridView.DataSource = null;
            this.dataGridView.Columns.Clear();
            ComplexDouble[] data = new ComplexDouble[dataSource.GetLength(1)];
            for (int ii = 0; ii < dataSource.GetLength(1); ii++)
            {
                data[ii] = dataSource[currentIndex, ii];
            }

            this.dataGridView.DataSource = data;
            this.dataGridView.Refresh();

        }


        public void SetData(ComplexDouble[,] data)
        {
            dataSource = data;
            currentIndex = 0;

            updateData = false;
            this.recordLabel2.Text = String.Format("of {0}", dataSource.GetLength(0));
            this.recordNumberNumericUpDown.Enabled = true;            
            this.recordNumberNumericUpDown.Value = currentIndex + 1;
            this.recordNumberNumericUpDown.Minimum = currentIndex + 1;
            this.recordNumberNumericUpDown.Maximum = dataSource.GetLength(0);

            updateData = true;
            BindData(0);
        }

        private void recordNumberNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (updateData)
            {
                int value = decimal.ToInt32(recordNumberNumericUpDown.Value);

                if (value < 1 || currentIndex > dataSource.GetLength(0))
                    currentIndex = 1;
                else
                    currentIndex = value - 1;

                BindData(currentIndex);
            }
        }
    }
}
