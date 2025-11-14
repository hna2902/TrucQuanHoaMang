using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections; // <-- Thêm thư viện này

namespace TrucQuanHoaMang
{
    public partial class Form1 : Form
    {
        // --- 1. BIẾN TOÀN CỤC ---
        private ArrayManager arrayManager;
        private List<Label> visualCells = new List<Label>();
        private int manualInsertPosition = 0;
        private int[] arraySnapshot;

        // --- BIẾN TRẠNG THÁI SẮP XẾP MỚI ---
        private IEnumerator sortingIterator; // "Cuộn phim"
        private bool isAutoSorting = false;  // Cờ báo đang chạy tự động

        // (Biến màu giữ nguyên)
        private Color colorDefault = Color.WhiteSmoke;
        private Color colorCompare = Color.Yellow;
        private Color colorSwap = Color.Red;
        private Color colorSorted = Color.LightGreen;

        public Form1()
        {
            InitializeComponent();
            arrayManager = new ArrayManager();

            // --- 2. KHỞI TẠO (ĐÃ CẬP NHẬT) ---
            Algorithm_Sort.SelectedIndex = 0;
            input_PositionManual.ReadOnly = true;
            input_PositionManual.Text = "0";

            UpdateCreateButtonsState(true);
            UpdateMainButtonsState(false);
            GroupBox_Algorithm.Enabled = false;
        }

        // --- 3. CÁC HÀM XỬ LÝ SỰ KIỆN (NÚT BẤM) ---

        #region Create and Modify Buttons
        // (Tất cả các hàm này giữ nguyên)
        private void btnCreate_Random_Click(object sender, EventArgs e)
        {
            int size = (int)input_SizeRandom.Value;
            if (size > 0)
            {
                arrayManager.CreateArray(size, true);
                DrawArray(arrayManager.GetData());
                UpdateMainButtonsState(true);
                UpdateCreateButtonsState(false);
                arraySnapshot = (int[])arrayManager.GetData().Clone();
            }
        }
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
                arraySnapshot = (int[])arrayManager.GetData().Clone();
            }
        }
        private void btnCreate_Array_Click(object sender, EventArgs e)
        {
            UpdateCreateButtonsState(false);
            UpdateMainButtonsState(true);
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_ValueAct.Text, out int value) && int.TryParse(input_PositionAct.Text, out int position))
            {
                arrayManager.InsertElement(value, position);
                DrawArray(arrayManager.GetData());
                lblStatus.Text = $"Đã chèn giá trị {value} vào vị trí {position}.";
                arraySnapshot = (int[])arrayManager.GetData().Clone();
            }
            else { lblStatus.Text = "Lỗi: Giá trị hoặc vị trí không hợp lệ."; }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_PositionAct.Text, out int position))
            {
                arrayManager.DeleteElement(position);
                DrawArray(arrayManager.GetData());
                lblStatus.Text = $"Đã xóa phần tử tại vị trí {position}.";
                arraySnapshot = (int[])arrayManager.GetData().Clone();
            }
            else { lblStatus.Text = "Lỗi: Vị trí không hợp lệ."; }
        }
        
        // SỬA LẠI: Nút tìm kiếm giờ sẽ gọi "HighlightCellAsync"
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(input_ValueAct.Text, out int value))
            {
                int position = arrayManager.SearchElement(value);
                if (position != -1)
                {
                    lblStatus.Text = $"Tìm thấy giá trị {value} tại vị trí {position}.";
                    await HighlightCellAsync(position, Color.Blue); // Đổi tên hàm
                    await Task.Delay(500);
                    await HighlightCellAsync(position, colorDefault); // Đổi tên hàm
                }
                else { lblStatus.Text = $"Không tìm thấy giá trị {value}."; }
            }
        }
        
        // SỬA LẠI: Nút xóa mảng gọi hàm Reset tổng
        private void btnClearArray_Click(object sender, EventArgs e)
        {
            arrayManager.ResetArray();
            DrawArray(arrayManager.GetData());
            lblStatus.Text = "Đã xóa mảng. Sẵn sàng tạo mảng mới.";
            ResetSortControls(true); // Reset về trạng thái ban đầu
        }
        #endregion

        #region Sorting Control Buttons (LOGIC MỚI)
        // Nút "Duyệt" (Tự động)
        private void btn_AutoSort_Click(object sender, EventArgs e)
        {
            isAutoSorting = true;
            if (sortingIterator == null) // Nếu là một lượt chạy mới
            {
                InitializeSortingIterator(Algorithm_Sort.SelectedItem.ToString());
                ToggleMainControls(false); // Khóa các nút chính
            }
            
            // Cập nhật giao diện nút bấm
            btn_AutoSort.Enabled = false;
            btn_StopSort.Enabled = true;
            btn_Back.Enabled = false;
            btn_ContinueSort.Enabled = false;
            
            RunAutoSortDriver(); // Bắt đầu chạy tự động
        }

        // Nút "Ngưng"
        private void btn_StopSort_Click(object sender, EventArgs e)
        {
            isAutoSorting = false; // Dừng vòng lặp tự động

            // Cập nhật giao diện nút bấm
            btn_AutoSort.Enabled = true; // Nút "Duyệt" trở thành "Tiếp tục tự động"
            btn_StopSort.Enabled = false;
            btn_Back.Enabled = true; // Cho phép quay lại
            btn_ContinueSort.Enabled = true; // Cho phép đi từng bước
        }

        // Nút "Quay lại" (Reset lại quá trình sort)
        private void btn_Back_Click(object sender, EventArgs e)
        {
            arrayManager.RestoreArrayFromSnapshot(arraySnapshot);
            DrawArray(arrayManager.GetData());
            ResetSortControls(false); // Reset nút sort, mở khóa nút chính
        }

        // Nút "Tiếp tục" (Duyệt từng bước)
        private void btn_ContinueSort_Click(object sender, EventArgs e)
        {
            isAutoSorting = false; // Chuyển sang chế độ thủ công
            
            if (sortingIterator == null) // Nếu là lần chạy đầu tiên
            {
                InitializeSortingIterator(Algorithm_Sort.SelectedItem.ToString());
                ToggleMainControls(false); // Khóa các nút chính
            }
            
            // Cập nhật giao diện nút (cho phép chuyển qua lại)
            btn_AutoSort.Enabled = true; 
            btn_StopSort.Enabled = false;
            btn_Back.Enabled = true;
            btn_ContinueSort.Enabled = true;

            // Chạy 1 bước của "cuộn phim"
            if (sortingIterator.MoveNext())
            {
                lblStatus.Text = "Đã thực hiện 1 bước.";
            }
            else
            {
                HandleSortCompletion(); // "Cuộn phim" đã hết
            }
        }
        #endregion

        #region Sorting Logic (LOGIC MỚI)
        // Hàm này khởi tạo "cuộn phim"
        private void InitializeSortingIterator(string algorithmName)
        {
            arrayManager.RestoreArrayFromSnapshot(arraySnapshot);
            DrawArray(arrayManager.GetData());
            ResetAllCellColors();
            lblStatus.Text = "Bắt đầu sắp xếp...";

            // Hàm helper giờ là Action (void)
            Action<int, Color> highlight = HighlightCell;
            Action<int, int> swap = SwapCells;

            if (algorithmName == "Bubble Sort")
                sortingIterator = arrayManager.BubbleSort(highlight, swap);
            else if (algorithmName == "Insertion Sort")
                sortingIterator = arrayManager.InsertionSort(highlight, swap);
        }

        // TRÌNH ĐIỀU KHIỂN TỰ ĐỘNG
        private async Task RunAutoSortDriver()
        {
            if (sortingIterator == null) return;
            int delay = GetAnimationSpeed();

            while (isAutoSorting && sortingIterator.MoveNext())
            {
                await Task.Delay(delay);
            }

            if (isAutoSorting) // Nếu dừng do hết phim
            {
                HandleSortCompletion();
            }
            // Nếu dừng do isAutoSorting = false (nhấn "Ngưng"), thì không làm gì
        }

        // HÀM XỬ LÝ KHI SẮP XẾP XONG
        private void HandleSortCompletion()
        {
            lblStatus.Text = "Sắp xếp hoàn tất!";
            bool isCreateEnabled = (arrayManager.GetData().Length == 0);
            ResetSortControls(isCreateEnabled); // Reset nút sort, mở khóa nút chính

            DialogResult result = MessageBox.Show(
                $"Đã duyệt xong.\nBạn có muốn xem lại không?",
                "Hoàn tất Sắp xếp",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                btn_AutoSort_Click(this, EventArgs.Empty);
            }
        }
        #endregion
        
        #region Visualization Helpers (ĐÃ SỬA)
        // --- 4. CÁC HÀM "HELPER" ĐỂ TRỰC QUAN HÓA ---
        private void DrawArray(int[] arrayData)
        {
            panelVisualizer.Controls.Clear();
            visualCells.Clear();
            int boxSize = 40, margin = 10, startX = 15, startY = 15;

            for (int i = 0; i < arrayData.Length; i++)
            {
                Label cell = new Label
                {
                    Text = arrayData[i].ToString(),
                    Size = new Size(boxSize, boxSize),
                    Location = new Point(startX + i * (boxSize + margin), startY),
                    BackColor = colorDefault,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BorderStyle = BorderStyle.FixedSingle
                };
                panelVisualizer.Controls.Add(cell);
                visualCells.Add(cell);
            }
        }

        // Hàm tô màu (void) - Dùng cho Sắp xếp
        private void HighlightCell(int index, Color color)
        {
            if (index >= 0 && index < visualCells.Count)
            {
                visualCells[index].BackColor = color;
                visualCells[index].Refresh(); // Buộc UI cập nhật ngay
            }
        }
        
        // Hàm tô màu (async) - Dùng cho Tìm kiếm (vì nó độc lập)
        private async Task HighlightCellAsync(int index, Color color)
        {
            if (index >= 0 && index < visualCells.Count)
            {
                visualCells[index].BackColor = color;
            }
        }

        // Hàm hoán vị (void) - Dùng cho Sắp xếp
        private void SwapCells(int index1, int index2)
        {
            if (index1 >= 0 && index1 < visualCells.Count && index2 >= 0 && index2 < visualCells.Count)
            {
                string temp = visualCells[index1].Text;
                visualCells[index1].Text = visualCells[index2].Text;
                visualCells[index2].Text = temp;
                visualCells[index1].Refresh();
                visualCells[index2].Refresh();
            }
        }

        // Hàm lấy tốc độ (int)
        private int GetAnimationSpeed()
        {
            // (tbSpeed là tên TrackBar của bạn)
            return (tbSpeed.Maximum - tbSpeed.Value + tbSpeed.Minimum) * 100;
        }

        private void ResetAllCellColors()
        {
            foreach (Label cell in visualCells)
            {
                cell.BackColor = colorDefault;
            }
        }
        #endregion
        
        #region UI State Management (ĐÃ SỬA)
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
            btnClearArray.Enabled = isEnabled;
            GroupBox_Algorithm.Enabled = isEnabled;
        }

        // Hàm mới: Khóa các nút chính (Tạo, Thao tác, Xóa)
        private void ToggleMainControls(bool isEnabled, bool? isCreateEnabled = null)
        {
            bool createStatus = isCreateEnabled ?? isEnabled;
            UpdateCreateButtonsState(createStatus);
            UpdateMainButtonsState(isEnabled);
        }
        
        // HÀM MỚI: Reset lại toàn bộ trạng thái của GroupBox Sắp xếp
        private void ResetSortControls(bool isCreateEnabled)
        {
            isAutoSorting = false;
            sortingIterator = null; 

            // Bật/tắt các nút chính (isCreateEnabled quyết định nút Tạo mảng)
            ToggleMainControls(true, isCreateEnabled);
            
            // Cập nhật các nút điều khiển duyệt về trạng thái ban đầu
            btn_AutoSort.Enabled = true;
            btn_StopSort.Enabled = false;
            btn_Back.Enabled = false;
            btn_ContinueSort.Enabled = true;
        }
        #endregion
    }
}