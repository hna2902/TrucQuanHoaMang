using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrucQuanHoaMang
{
    public partial class Form1 : Form
    {
        // --- 1. KHAI BÁO CÁC BIẾN TOÀN CỤC ---
        private ArrayManager arrayManager;
        private List<Label> visualCells = new List<Label>();
        private int manualInsertPosition = 0;
        private int[] arraySnapshot; // Biến để lưu mảng gốc

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
            UpdateMainButtonsState(false); // Sẽ tự động vô hiệu hóa btnClearArray

            Algorithm_Sort.SelectedIndex = 0;

            input_PositionManual.ReadOnly = true;
            input_PositionManual.Text = "0";
            btnCreate_Array.Enabled = false;
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
                arraySnapshot = (int[])arrayManager.GetData().Clone(); // ĐÃ SỬA: Đặt bên trong IF
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
                input_ValueManual.Text = "";
                input_ValueManual.Focus();
                UpdateMainButtonsState(true);
                btnCreate_Array.Enabled = true;
                arraySnapshot = (int[])arrayManager.GetData().Clone(); // ĐÃ SỬA: Đặt bên trong IF
            }
        }

        // Nút "Hoàn tất" (Tự tạo)
        private void btnCreate_Array_Click(object sender, EventArgs e)
        {
            UpdateCreateButtonsState(false);
            UpdateMainButtonsState(true); // <-- THÊM MỚI: Bật các nút thao tác
        }

        // Nút "Thêm" (Thao tác)
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_ValueAct.Text, out int value) && int.TryParse(input_PositionAct.Text, out int position))
            {
                arrayManager.InsertElement(value, position);
                DrawArray(arrayManager.GetData());
                lblStatus.Text = $"Đã chèn giá trị {value} vào vị trí {position}.";
                arraySnapshot = (int[])arrayManager.GetData().Clone(); // ĐÃ SỬA: Đặt bên trong IF
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
                arraySnapshot = (int[])arrayManager.GetData().Clone(); // ĐÃ SỬA: Đặt bên trong IF
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

        // Nút "Duyệt" (Sắp xếp)
        private async void btnSort_Click(object sender, EventArgs e)
        {
            string algorithmName = Algorithm_Sort.SelectedItem.ToString();
            bool replay = true;

            while (replay)
            {
                replay = false;
                arrayManager.RestoreArrayFromSnapshot(arraySnapshot);
                DrawArray(arrayManager.GetData());

                ToggleAllControls(false);
                lblStatus.Text = "Đang sắp xếp...";
                ResetAllCellColors();

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

                ToggleAllControls(true);
                lblStatus.Text = "Sắp xếp hoàn tất!";

                DialogResult result = MessageBox.Show(
                    $"Đã duyệt xong bằng {algorithmName}.\nBạn có muốn xem lại không?",
                    "Hoàn tất Sắp xếp",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    replay = true;
                }
            }
        }

        // Nút "Xóa Mảng"
        private void btnClearArray_Click(object sender, EventArgs e)
        {
            arrayManager.ResetArray();
            panelVisualizer.Controls.Clear();
            visualCells.Clear();
            UpdateMainButtonsState(false); // Tắt các nút thao tác (bao gồm cả nút này)
            UpdateCreateButtonsState(true); // Bật lại các nút tạo
            lblStatus.Text = "Đã xóa mảng. Sẵn sàng tạo mảng mới.";
        }


        // --- 4. CÁC HÀM "HELPER" ĐỂ TRỰC QUAN HÓA ---

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
            btnClearArray.Enabled = isEnabled; // <-- ĐÃ THÊM NÚT XÓA MẢNG VÀO ĐÂY
        }

        private void ToggleAllControls(bool isEnabled)
        {
            UpdateCreateButtonsState(isEnabled);
            UpdateMainButtonsState(isEnabled);
        }

        // Các hàm trống do double-click nhầm (bạn có thể xóa chúng đi)
        // private void Form1_Load(object sender, EventArgs e) { }
        // ...
    }
}