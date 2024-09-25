using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLY_QUANAN
{
    internal class XuatHoaDon
    {
        public static bool xuatHoaDon(string content, System.Data.DataTable dataTable, string billId, string customerName, string orderDate, float fullValue, string tenNv)
        {
            try
            {
                //Tạo các đối tượng Excel cần thiết để thao tác với file Excel: Application, Workbooks, Sheets, Workbook, và Worksheet
                Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks oBooks;
                Microsoft.Office.Interop.Excel.Sheets oSheets;
                Microsoft.Office.Interop.Excel.Workbook oBook;
                Microsoft.Office.Interop.Excel.Worksheet oSheet;

                //Tạo mới một Excel WorkBook với một sheet
                //Đặt tên sheet theo content.
                oExcel.Visible = true;
                oExcel.DisplayAlerts = false;
                oExcel.Application.SheetsInNewWorkbook = 1;
                oBooks = oExcel.Workbooks;
                oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));
                oSheets = oBook.Worksheets;
                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);

                string sheetName = content;
                string title = "Đơn hàng " + content;

                oSheet.Name = sheetName;

                //Tiêu đề định dạng từ cột A1 đến E1
                Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "E1");
                head.MergeCells = true;
                head.Value2 = title;
                head.Font.Bold = true;
                head.Font.Name = "Times New Roman";
                head.Font.Size = "20";
                head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                //Điền các thông tin về mã hóa đơn, tên khách hàng, ngày đặt, tổng giá trị hóa đơn, và tên nhân viên vào các ô tương ứng trong bảng Excel.
                int billIdRow = 1;
                Microsoft.Office.Interop.Excel.Range billIdLabelCell = oSheet.get_Range("A" + billIdRow, "A" + billIdRow);
                billIdLabelCell.Value2 = "Mã hóa đơn: " + billId;

                //Thông tin
                int customerRow = 2;
                Microsoft.Office.Interop.Excel.Range customerNameLabelCell = oSheet.get_Range("A" + customerRow, "A" + customerRow);
                customerNameLabelCell.Value2 = "Tên khách hàng:";
                Microsoft.Office.Interop.Excel.Range customerNameValueCell = oSheet.get_Range("B" + customerRow, "B" + customerRow);
                customerNameValueCell.Value2 = customerName;
                customerNameValueCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                int orderDateRow = customerRow + 1;
                Microsoft.Office.Interop.Excel.Range orderDateLabelCell = oSheet.get_Range("A" + orderDateRow, "A" + orderDateRow);
                orderDateLabelCell.Value2 = "Ngày đặt:";
                Microsoft.Office.Interop.Excel.Range orderDateValueCell = oSheet.get_Range("B" + orderDateRow, "B" + orderDateRow);
                orderDateValueCell.Value2 = orderDate;

                int totalBillValueRow = orderDateRow + 1;
                Microsoft.Office.Interop.Excel.Range totalBillValueLabelCell = oSheet.get_Range("A" + totalBillValueRow, "A" + totalBillValueRow);
                totalBillValueLabelCell.Value2 = "Tổng giá trị hóa đơn:";
                Microsoft.Office.Interop.Excel.Range totalBillValueValueCell = oSheet.get_Range("B" + totalBillValueRow, "B" + totalBillValueRow);
                totalBillValueValueCell.Value2 = fullValue.ToString();

                int tenNhanVienRow = totalBillValueRow + 1;
                Microsoft.Office.Interop.Excel.Range tenNhanVienLabelCell = oSheet.get_Range("A" + tenNhanVienRow, "A" + tenNhanVienRow);
                tenNhanVienLabelCell.Value2 = "Người tạo bill:";
                Microsoft.Office.Interop.Excel.Range tenNhanVienValueCell = oSheet.get_Range("B" + tenNhanVienRow, "B" + tenNhanVienRow);
                tenNhanVienValueCell.Value2 = tenNv;
                tenNhanVienValueCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;


                //Thiết lập các tiêu đề cột cho bảng chi tiết hóa đơn (thứ tự bill, tên món ăn, đơn giá, số lượng, tổng giá) và căn giữa các tiêu đề
                int columnRow = tenNhanVienRow + 1;

                //Tiêu đề bảng


                Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A" + columnRow, "A" + columnRow);
                cl1.Value2 = "STT món ăn";
                cl1.ColumnWidth = 12;

                Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B" + columnRow, "B" + columnRow);
                cl2.Value2 = "Tên món ăn";
                cl2.ColumnWidth = 30.29;

                Microsoft.Office.Interop.Excel.Range cl3 = oSheet.get_Range("C" + columnRow, "C" + columnRow);
                cl3.Value2 = "Đơn giá";
                cl3.ColumnWidth = 14;

                Microsoft.Office.Interop.Excel.Range cl4 = oSheet.get_Range("D" + columnRow, "D" + columnRow);
                cl4.Value2 = "Số lượng";
                cl4.ColumnWidth = 23.71;

                Microsoft.Office.Interop.Excel.Range cl5 = oSheet.get_Range("E" + columnRow, "E" + columnRow);
                cl5.Value2 = "Tổng giá";
                cl5.ColumnWidth = 10.71;

                Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A" + columnRow, "E" + columnRow);


                // Thiết lập màu nền
                int size = dataTable.Columns.Count;


                rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // Tạo một mảng hai chiều để chứa dữ liệu từ DataTable.

                string[,] arr = new string[dataTable.Rows.Count, dataTable.Columns.Count];

                //Chuyển dữ liệu từ DataTable vào mảng đối tượng

                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    DataRow dataRow = dataTable.Rows[row];

                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        arr[row, col] = dataRow[col].ToString();
                    }
                }

                //Thiết lập vùng điền dữ liệu

                int rowStart = 7;

                int columnStart = 1;

                int rowEnd = rowStart + dataTable.Rows.Count - 1;

                int columnEnd = dataTable.Columns.Count;

                // Ô bắt đầu điền dữ liệu

                Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, columnStart];

                // Ô kết thúc điền dữ liệu

                Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnEnd];

                // Lấy về vùng điền dữ liệu

                Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);

                //Điền dữ liệu vào vùng đã thiết lập

                range.Value2 = arr;

                // Kẻ viền

                range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                // Căn giữa cột mã nhân viên

                Microsoft.Office.Interop.Excel.Range c3 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnStart];

                Microsoft.Office.Interop.Excel.Range c4 = oSheet.get_Range(c1, c3);

                //Căn giữa cả bảng 
                oSheet.get_Range(c1, c2).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }
    }
}
