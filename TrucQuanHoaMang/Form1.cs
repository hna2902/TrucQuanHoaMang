using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrucQuanHoaMang
{
    public partial class Form1 : Form
    {
        // --- 1. KHAI BÁO CÁC BIẾN TOÀN CỤC ---
        private ArrayManager arrayManager;
        private List<Label> visualCells = new List<Label>(); // Danh sách để lưu các ô Label
        private int manualInsertPosition = 0;

        // Định nghĩa các màu sắc
        private Color colorDefault = Color.WhiteSmoke;
        private Color colorCompare = Color.Yellow;
        private Color colorSwap = Color.Red;
        private Color colorSorted = Color.LightGreen;

        public Form1()
        {
            InitializeComponent();

            // --- 2. KHỞI TẠO ---
            arrayManager = new ArrayManager();

            // Thiết lập trạng thái nút bấm ban đầu
            UpdateCreateButtonsState(true);
            UpdateMainButtonsState(false);

            // Chọn mục đầu tiên trong ComboBox làm mặc định
            Algorithm_Sort.SelectedIndex = 0;

            // --- THIẾT LẬP CHO TẠO THỦ CÔNG ---
            // Làm cho ô "Vị trí" trong nhóm "Khởi tạo mảng" không thể sửa được
            input_PositionManual.ReadOnly = true;
            input_PositionManual.Text = "0"; // Đặt giá trị ban đầu
            btnCreate_Array.Enabled = false; // Vô hiệu hóa nút "Hoàn tất"
        }

        // --- 3. CÁC HÀM XỬ LÝ SỰ KIỆN (NÚT BẤM) ---

        // Nút "Tạo Mảng" (Ngẫu nhiên)
        private void btnCreate_Random_Click(object sender, EventArgs e)
        {
            int size = (int)input_SizeRandom.Value;
            if (size > 0)
            {
                arrayManager.CreateArray(size, true);
                DrawArray(arrayManager.GetData());
                UpdateMainButtonsState(true);
                UpdateCreateButtonsState(false);
            }
        }

        // Nút "Thêm" (Tự tạo)
        private void btnCreate_Index_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_ValueManual.Text, out int value))
            {
                arrayManager.InsertElement(value, manualInsertPosition);
                DrawArray(arrayManager.GetData());

                manualInsertPosition++;
                input_PositionManual.Text = manualInsertPosition.ToString();
                input_ValueManual.Text = ""; // Xóa text
                input_ValueManual.Focus();   // Đặt con trỏ chuột lại vào ô

                UpdateMainButtonsState(true);
                btnCreate_Array.Enabled = true; // Kích hoạt nút "Hoàn tất"
            }
        }

        // Nút "Hoàn tất" (Tự tạo)
        private void btnCreate_Array_Click(object sender, EventArgs e)
        {
            UpdateCreateButtonsState(false); // Vô hiệu hóa cả 2 nhóm tạo mảng
        }

        // Nút "Thêm" (Thao tác)
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_ValueAct.Text, out int value) && int.TryParse(input_PositionAct.Text, out int position))
            {
                arrayManager.InsertElement(value, position);
                DrawArray(arrayManager.GetData());
                lblStatus.Text = $"Đã chèn giá trị {value} vào vị trí {position}.";
            }
            else
            {
                lblStatus.Text = "Lỗi: Giá trị hoặc vị trí không hợp lệ.";
            }
        }

        // Nút "Xóa" (Thao tác)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_PositionAct.Text, out int position))
            {
                arrayManager.DeleteElement(position);
                DrawArray(arrayManager.GetData());
                lblStatus.Text = $"Đã xóa phần tử tại vị trí {position}.";
            }
            else
            {
                lblStatus.Text = "Lỗi: Vị trí không hợp lệ.";
            }
        }

        // Nút "Tìm" (Thao tác)
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_ValueAct.Text, out int value))
            {
                int position = arrayManager.SearchElement(value);
                if (position != -1)
                {
                    lblStatus.Text = $"Tìm thấy giá trị {value} tại vị trí {position}.";
                    // Hiệu ứng nháy xanh cho ô tìm thấy
                    await HighlightCell(position, Color.Blue);
                    await Task.Delay(500);
                    await HighlightCell(position, colorDefault);
                }
                else
                {
                    lblStatus.Text = $"Không tìm thấy giá trị {value}.";
                }
            }
        }

        // Nút "Duyệt" (Sắp xếp) - PHẢI THÊM "async"
        private async void btnSort_Click(object sender, EventArgs e)
        {
            string algorithmName = Algorithm_Sort.SelectedItem.ToString();

            // Khóa các nút bấm lại
            ToggleAllControls(false);
            lblStatus.Text = "Đang sắp xếp...";

            // Reset màu sắc
            ResetAllCellColors();

            // Lấy các hàm "helper" để truyền vào "bộ não"
            Func<int, Color, Task> highlight = HighlightCell;
            Func<int, int, Task> swap = SwapCells;
            Func<Task<int>> getSpeed = GetAnimationSpeed;

            if (algorithmName == "Bubble Sort")
            {
                await arrayManager.BubbleSort(highlight, swap, getSpeed);
            }
            else if (algorithmName == "Insertion Sort")
            {
                await arrayManager.InsertionSort(highlight, swap, getSpeed);
            }

            // Mở khóa các nút bấm
            ToggleAllControls(true);
            // Kích hoạt lại các nút tạo mảng (vì đã có nút Xóa Mảng)
            // UpdateCreateButtonsState(true); 
            lblStatus.Text = "Sắp xếp hoàn tất!";
        }

        // --- 4. CÁC HÀM "HELPER" ĐỂ TRỰC QUAN HÓA ---

        // Hàm vẽ mảng lên panelVisualizer
        private void DrawArray(int[] arrayData)
        {
            panelVisualizer.Controls.Clear();
            visualCells.Clear();

            int boxSize = 40;
            int margin = 10;
            int startX = 15;
            int startY = 15;

            for (int i = 0; i < arrayData.Length; i++)
            {
                Label cell = new Label();
                cell.Text = arrayData[i].ToString();
                cell.Size = new Size(boxSize, boxSize);
                cell.Location = new Point(startX + i * (boxSize + margin), startY);
                cell.BackColor = colorDefault;
                cell.ForeColor = Color.Black;
                cell.Font = new Font("Arial", 12, FontStyle.Bold);
                cell.TextAlign = ContentAlignment.MiddleCenter;
                cell.BorderStyle = BorderStyle.FixedSingle;

                panelVisualizer.Controls.Add(cell);
                visualCells.Add(cell);
            }
        }

        private async Task HighlightCell(int index, Color color)
        {
            if (index >= 0 && index < visualCells.Count)
            {
                visualCells[index].BackColor = color;
            }
        }

        private async Task SwapCells(int index1, int index2)
        {
            if (index1 >= 0 && index1 < visualCells.Count && index2 >= 0 && index2 < visualCells.Count)
            {
                string temp = visualCells[index1].Text;
                visualCells[index1].Text = visualCells[index2].Text;
                visualCells[index2].Text = temp;
            }
        }

        private async Task<int> GetAnimationSpeed()
        {
            int delay = (tbSpeed.Maximum - tbSpeed.Value + tbSpeed.Minimum) * 100;
            return delay;
        }

        private void ResetAllCellColors()
        {
            foreach (Label cell in visualCells)
            {
                cell.BackColor = colorDefault;
            }
        }

        // --- 5. CÁC HÀM QUẢN LÝ TRẠNG THÁI GIAO DIỆN ---

        private void UpdateCreateButtonsState(bool isEnabled)
        {
            GroupBox_CreateRandom.Enabled = isEnabled;
            GroupBox_CreateManual.Enabled = isEnabled;
        }

        private void UpdateMainButtonsState(bool isEnabled)
        {
            GroupBox_Act.Enabled = isEnabled;
            GroupBox_Sort.Enabled = isEnabled;
            // Nút "Xóa Mảng" chưa có, nên tôi tạm thời bỏ qua
        }

        private void ToggleAllControls(bool isEnabled)
        {
            UpdateCreateButtonsState(isEnabled);
            UpdateMainButtonsState(isEnabled);
        }

        // --- CÁC HÀM TRỐNG DO BẠN DOUBLE-CLICK NHẦM (XÓA ĐI CHO GỌN) ---
        // private void Form1_Load(object sender, EventArgs e) { }
        // private void label1_Click(object sender, EventArgs e) { }
        // private void textBox1_TextChanged(object sender, EventArgs e) { }
        // private void label3_Click(object sender, EventArgs e) { }
        // private void groupBox1_Enter(object sender, EventArgs e) { }
        // private void groupBox3_Enter(object sender, EventArgs e) { }
        // private void button2_Click(object sender, EventArgs e) { }
        // private void groupBox4_Enter(object sender, EventArgs e) { }
        // private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        // private void Label_algorithm_Click(object sender, EventArgs e) { }
        // private void label1_Click_1(object sender, EventArgs e) { }
        // private void tbSpeed_Scroll(object sender, EventArgs e) { }
        // private void input_ValueManual_TextChanged(object sender, EventArgs e) { }
    }
}