using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmLoadSeqSortOrder : Form
    {
        //CONSTANTS
        private const string CURRENTMODULE = "frmLoadSeqSortOrder";

        private bool blnMovingSeq = false;
        private frmVoyageAdmin frmCalling;

        public frmLoadSeqSortOrder(int intLastSequence, frmVoyageAdmin frmVoy)
        {
            try
            {
                InitializeComponent();

                Formops.SetFormBackground(this);

                frmCalling = frmVoy;

                dgSeq.AutoGenerateColumns = false;
                lblSequences.Text = "Sequences: " + intLastSequence.ToString();
                rbHighToLow.Checked = true;
                btnRemove.Enabled = false;

                FillCombos(intLastSequence);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmLoadSeqSortOrder", ex.Message);
            }
            
        }

        private void FillCombos(int intTotalSequences)
        {
            ComboboxItem cboItem;
            List<ComboboxItem> lsCBOItems = new List<ComboboxItem>();

            //Add items to cboSquence 
            cboItem = new ComboboxItem();
            cboItem.cboText = "<select>";
            cboItem.cboValue = "select";
            lsCBOItems.Add(cboItem);

           for (int intSeq = 1; intSeq<=intTotalSequences; intSeq++)
            {
                cboItem = new ComboboxItem();
                cboItem.cboText = intSeq.ToString();
                cboItem.cboValue = intSeq.ToString();
                lsCBOItems.Add(cboItem);
            }

            cboSequence.DataSource = lsCBOItems;
            cboSequence.DisplayMember = "cboText";
            cboSequence.ValueMember = "cboValue";
            cboSequence.SelectedIndex = 0;
        }

        private void rbHighToLow_CheckedChanged(object sender, EventArgs e)
        {
            lblSort.Text = "Change the Sort Low To High for Sequence:";
        }

        private void rbLowToHigh_CheckedChanged(object sender, EventArgs e)
        {
            lblSort.Text = "Change the Sort High To Low for Sequence:";
        }

        private void AddSequenceToDG()
        {
            ComboboxItem cboItem;
            List<ComboboxItem> lsCboDataSource = (List<ComboboxItem>) cboSequence.DataSource;
            List<LoadSeqItem> lsDGDataSource;
            LoadSeqItem objLoadSeqItem;

            try
            {
                if (blnMovingSeq) return;

                if (cboSequence.SelectedIndex == 0) return;

                //Set to avoid re-entering when cboSeq DataSource is changed
                blnMovingSeq = true;

                cboItem = (cboSequence.SelectedItem as ComboboxItem);
                cboSequence.DataSource = null;

                if (dgSeq.DataSource == null)
                {
                    lsDGDataSource = new List<LoadSeqItem>();
                }
                else
                {
                    lsDGDataSource = (List<LoadSeqItem>)dgSeq.DataSource;
                }

                //Add Sequence to dgSeq Datasource
                objLoadSeqItem = new LoadSeqItem();
                objLoadSeqItem.Sequence_string = cboItem.cboValue;
                lsDGDataSource.Add(objLoadSeqItem);

                //sort dgSeq Datasource
                lsDGDataSource.Sort((ldseqitem1, ldseqitem2) =>
                     ldseqitem1.Sequence_int.
                     CompareTo(ldseqitem2.Sequence_int));

                //Assigned new Datasource to dgSeq
                dgSeq.DataSource = null;
                dgSeq.DataSource = lsDGDataSource;

                //ID Item in lsCboDataSource to remove
                foreach (ComboboxItem Item in lsCboDataSource)
                    if (Item.cboValue == cboItem.cboValue) cboItem = Item;

                //Remove the item from lsCboDataSource
                lsCboDataSource.RemoveAll(item => item.cboValue ==
                        cboItem.cboValue);

                //Assign new lsCboDataSource to cboSeq
                cboSequence.DataSource = null;
                cboSequence.DataSource = lsCboDataSource;

                cboSequence.DisplayMember = "cboText";
                cboSequence.ValueMember = "cboValue";
                cboSequence.SelectedIndex = 0;

                btnRemove.Enabled = true;

                blnMovingSeq = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AddSequencetoDG",
                    ex.Message);
            }
        }

        private void cboSequence_SelectedIndexChanged(object sender, EventArgs e)
        { AddSequenceToDG(); }

        private void RemoveItem()
        {
            ComboboxItem cboItem = new ComboboxItem();
            List<ComboboxItem> lsCboDataSource;
            List<ComboboxItem> lsCboNewDataSource;
            List<LoadSeqItem> lsDGDataSource;
            LoadSeqItem objLoadSeqItem = null;
            string strSeq;

            try
            {
                blnMovingSeq = true;

                lsDGDataSource = (List<LoadSeqItem>)dgSeq.DataSource;
                if (cboSequence.Items.Count == 0)
                    lsCboDataSource = new List<ComboboxItem>();
                else
                    lsCboDataSource = (List<ComboboxItem>) cboSequence.DataSource;

                //If only 1 item in dgSeq, store as cboItem and clear dgSeq
                if (lsDGDataSource.Count == 1)
                {
                    cboItem.cboText = lsDGDataSource[0].Sequence_string;
                    cboItem.cboValue = cboItem.cboText;
                    dgSeq.DataSource = null;
                }
                else
                {
                    //Multiple items in dgSeq. Remove selected item
                    strSeq = dgSeq.SelectedRows[0].Cells["Sequence_string"].Value.ToString();
                    foreach (LoadSeqItem ldSeqItem in lsDGDataSource)
                        if (ldSeqItem.Sequence_string == strSeq) objLoadSeqItem = ldSeqItem;

                    //Create new cboItem for cboSeq with Sequence value
                    cboItem.cboText = objLoadSeqItem.Sequence_string;
                    cboItem.cboValue = cboItem.cboText;

                    lsDGDataSource.RemoveAll(item => item.Sequence_string ==
                        objLoadSeqItem.Sequence_string);

                    dgSeq.DataSource = null;
                    dgSeq.DataSource = lsDGDataSource;
                }

                //Add cboItem to lsCboDataSource, Sort, and reassign to cboSequence
                lsCboDataSource.Add(cboItem);

                //Store 1st item <select> in lsCboDataSource as cboItem
                cboItem = lsCboDataSource[0];

                //Remove the 1st item
                lsCboDataSource.RemoveAt(0);

                lsCboDataSource.Sort((cboitem1, cboitem2) =>
                     cboitem1.cboValue.
                     CompareTo(cboitem2.cboValue));

                //Create lsCboNewDataSource with <select> as first ite3m
                lsCboNewDataSource = new List<ComboboxItem>();
                lsCboNewDataSource.Add(cboItem);

                //Add sorted items in lsCboDataSource
                foreach (ComboboxItem Item in lsCboDataSource)
                    lsCboNewDataSource.Add(Item);

                cboSequence.DataSource = null;
                cboSequence.DataSource = lsCboNewDataSource;
                cboSequence.DisplayMember = "cboText";
                cboSequence.ValueMember = "cboValue";
                cboSequence.SelectedIndex = 0;

                blnMovingSeq = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RemoveItem", ex.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<LoadSeqItem> lsDGDataSource;

            //Store SortOrder in frmVoyageAdmin
            frmCalling.strSortOrder = "HL";
            if (rbLowToHigh.Checked) frmCalling.strSortOrder = "LH";

            //Store SortExceptions in frmVoyageAdmin
            frmCalling.lsSortExceptions.Clear();
            if (dgSeq.RowCount > 0)
            {
                lsDGDataSource = (List<LoadSeqItem>)dgSeq.DataSource;
                foreach (LoadSeqItem ldSeqItem in lsDGDataSource)
                    frmCalling.lsSortExceptions.Add(ldSeqItem.Sequence_string);
            }

            //Store only Cleared vehs in frmVoyageAdmin
            if (ckCleared.Checked)
                frmCalling.blnLoadSeqOnlyClearedVehs = true;
            else
                frmCalling.blnLoadSeqOnlyClearedVehs = false;

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {RemoveItem();}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmLoadSeqSortOrder_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
