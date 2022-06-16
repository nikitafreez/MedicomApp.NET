using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedicomApp.NET
{
    public class ExcelHelper
    {
        public void ExcelExport(ref DataGrid dataGrid)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); ;
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Sheets[1];

            ExcelWorkBook.Title = "Больница";

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                //ExcelWorkSheet.Range["A1"].Offset[0, i].Value = gridView.Columns[i].ToString();
                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.Cells[1, i + 1];
                range.Value2 = dataGrid.Columns[i].Header;
            }

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                for (int j = 0; j < dataGrid.Items.Count; j++)
                {
                    TextBlock b = dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }

            ExcelWorkSheet.Columns.AutoFit();
            ExcelApp.Visible = true;
            ExcelApp.UserControl = true;
        }
    }
}
