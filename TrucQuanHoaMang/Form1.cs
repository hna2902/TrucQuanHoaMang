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
        private Stack<ArrayStateSnapshot> undoStack = new Stack<ArrayStateSnapshot>();

        // --- BIẾN TRẠNG THÁI SẮP XẾP MỚI ---
        private IEnumerator sortingIterator; // "Cuộn phim"
        private bool isAutoSorting = false;  // Cờ báo đang chạy tự động

        // (Biến màu giữ nguyên)
        private Color colorDefault = Color.WhiteSmoke;
        private Color colorCompare = Color.Yellow;
        private Color colorSwap = Color.Red;
        private Color colorSorted = Color.LightGreen;

        // --- HÀM KHỞI TẠO (ĐÃ SỬA) ---
        public Form1()
        {
            InitializeComponent();
            undoStack.Clear();
            arrayManager = new ArrayManager();

            Algorithm_Sort.SelectedIndex = 0;
            input_PositionManual.ReadOnly = true;
            input_PositionManual.Text = "0";

            // Gọi DefaultState để thiết lập trạng thái ban đầu chính xác
            DefaultState();
        }

        // --- 3. CÁC HÀM XỬ LÝ SỰ KIỆN (NÚT BẤM) ---

        #region Create and Modify Buttons

        private void btnCreate_Random_Click(object sender, EventArgs e)
        {
            int size = (int)input_SizeRandom.Value;
            if (size > 0)
            {
                arrayManager.CreateArray(size, true);
                DrawArray(arrayManager.GetData());
                arraySnapshot = (int[])arrayManager.GetData().Clone();

                // Cập nhật UI
                CreateGroup(false);
                ActGroup(true);
                SortGroup(true); // Gọi hàm SortGroup đã sửa
                GroupBox_Algorithm.Enabled = true;
                btnClearArray.Enabled = true;
                btnCreate_Array.Enabled = false;
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
                arraySnapshot = (int[])arrayManager.GetData().Clone();

                // Cập nhật UI (Đang tạo thủ công)
                ActGroup(false);
                SortGroup(false);
                GroupBox_Algorithm.Enabled = false;
                btnCreate_Array.Enabled = true; // Bật nút "Hoàn tất"
                btnClearArray.Enabled = false;
            }
        }

        private void btnCreate_Array_Click(object sender, EventArgs e)
        {
            // Cập nhật UI (Hoàn tất tạo thủ công)
            CreateGroup(false);
            ActGroup(true);
            SortGroup(true); // Gọi hàm SortGroup đã sửa
            GroupBox_Algorithm.Enabled = true;
            btnClearArray.Enabled = true;
            btnCreate_Array.Enabled = true;
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
            string valueText = input_ValueAct.Text;
            string posText = input_PositionAct.Text;
            bool hasValue = !string.IsNullOrEmpty(valueText);
            bool hasPos = !string.IsNullOrEmpty(posText);
            bool deleted = false; // Cờ để kiểm tra xem đã xóa hay chưa

            // --- KỊCH BẢN 1: Nhập cả hai (Phải khớp mới xóa) ---
            if (hasValue && hasPos)
            {
                if (int.TryParse(valueText, out int value) && int.TryParse(posText, out int pos))
                {
                    int[] data = arrayManager.GetData();
                    if (pos >= 0 && pos < data.Length)
                    {
                        if (data[pos] == value) // Phải khớp
                        {
                            arrayManager.DeleteElement(pos);
                            lblStatus.Text = $"Đã xóa giá trị {value} tại vị trí {pos}.";
                            deleted = true;
                        }
                        else
                        {
                            lblStatus.Text = $"Lỗi: Giá trị tại vị trí {pos} không khớp. Không xóa.";
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Lỗi: Vị trí không hợp lệ.";
                    }
                }
                else
                {
                    lblStatus.Text = "Lỗi: Giá trị hoặc vị trí không phải là số.";
                }
            }
            // --- KỊCH BẢN 2: Chỉ nhập Giá trị ---
            else if (hasValue && !hasPos)
            {
                if (int.TryParse(valueText, out int value))
                {
                    int position = arrayManager.SearchElement(value); // Tìm vị trí
                    if (position != -1)
                    {
                        arrayManager.DeleteElement(position);
                        lblStatus.Text = $"Đã xóa giá trị {value} (tìm thấy tại vị trí {position}).";
                        deleted = true;
                    }
                    else
                    {
                        lblStatus.Text = $"Lỗi: Không tìm thấy giá trị {value} để xóa.";
                    }
                }
                else
                {
                    lblStatus.Text = "Lỗi: Giá trị không phải là số.";
                }
            }
            // --- KỊCH BẢN 3: Chỉ nhập Vị trí ---
            else if (!hasValue && hasPos)
            {
                if (int.TryParse(posText, out int pos))
                {
                    // Kiểm tra vị trí hợp lệ trước khi xóa
                    if (pos >= 0 && pos < arrayManager.GetData().Length)
                    {
                        arrayManager.DeleteElement(pos);
                        lblStatus.Text = $"Đã xóa phần tử tại vị trí {pos}.";
                        deleted = true;
                    }
                    else
                    {
                        lblStatus.Text = "Lỗi: Vị trí không hợp lệ.";
                    }
                }
                else
                {
                    lblStatus.Text = "Lỗi: Vị trí không phải là số.";
                }
            }
            // --- KỊCH BẢN 4: Không nhập gì cả ---
            else
            {
                lblStatus.Text = "Vui lòng nhập giá trị hoặc vị trí để xóa.";
            }

            // --- KIỂM TRA SAU KHI XÓA ---
            if (deleted)
            {
                DrawArray(arrayManager.GetData()); // Vẽ lại mảng
                arraySnapshot = (int[])arrayManager.GetData().Clone();

                // Kiểm tra xem mảng có rỗng sau khi xóa không
                if (arrayManager.GetData().Length == 0)
                {
                    lblStatus.Text = "Mảng rỗng. Đã reset về trạng thái ban đầu.";
                    DefaultState(); // Gọi hàm reset
                }
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string valueText = input_ValueAct.Text;
            string posText = input_PositionAct.Text;
            bool hasValue = !string.IsNullOrEmpty(valueText);
            bool hasPos = !string.IsNullOrEmpty(posText);

            // --- KỊCH BẢN 1: Nhập cả hai ---
            if (hasValue && hasPos)
            {
                if (int.TryParse(valueText, out int value) && int.TryParse(posText, out int pos))
                {
                    int[] data = arrayManager.GetData();
                    if (pos >= 0 && pos < data.Length)
                    {
                        if (data[pos] == value)
                        {
                            lblStatus.Text = $"Chính xác! Giá trị {value} được tìm thấy tại vị trí {pos}.";
                            await HighlightCellAsync(pos, Color.Blue);
                            await Task.Delay(500);
                            await HighlightCellAsync(pos, colorDefault);
                        }
                        else
                        {
                            lblStatus.Text = $"Sai. Giá trị tại vị trí {pos} là {data[pos]}, không phải {value}.";
                            await HighlightCellAsync(pos, Color.Red); // Tô màu đỏ cho sai
                            await Task.Delay(500);
                            await HighlightCellAsync(pos, colorDefault);
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Lỗi: Vị trí không hợp lệ.";
                    }
                }
                else
                {
                    lblStatus.Text = "Lỗi: Giá trị hoặc vị trí không phải là số.";
                }
            }
            // --- KỊCH BẢN 2: Chỉ nhập Giá trị ---
            else if (hasValue && !hasPos)
            {
                if (int.TryParse(valueText, out int value))
                {
                    int position = arrayManager.SearchElement(value);
                    if (position != -1)
                    {
                        lblStatus.Text = $"Tìm thấy giá trị {value} tại vị trí {position}.";
                        await HighlightCellAsync(position, Color.Blue);
                        await Task.Delay(500);
                        await HighlightCellAsync(position, colorDefault);
                    }
                    else
                    {
                        lblStatus.Text = $"Không tìm thấy giá trị {value}.";
                    }
                }
                else
                {
                    lblStatus.Text = "Lỗi: Giá trị không phải là số.";
                }
            }
            // --- KỊCH BẢN 3: Chỉ nhập Vị trí ---
            else if (!hasValue && hasPos)
            {
                if (int.TryParse(posText, out int pos))
                {
                    int[] data = arrayManager.GetData();
                    if (pos >= 0 && pos < data.Length)
                    {
                        int valueFound = data[pos];
                        lblStatus.Text = $"Giá trị tại vị trí {pos} là {valueFound}.";
                        await HighlightCellAsync(pos, Color.Blue);
                        await Task.Delay(500);
                        await HighlightCellAsync(pos, colorDefault);
                    }
                    else
                    {
                        lblStatus.Text = "Lỗi: Vị trí không hợp lệ.";
                    }
                }
                else
                {
                    lblStatus.Text = "Lỗi: Vị trí không phải là số.";
                }
            }
            // --- KỊCH BẢN 4: Không nhập gì cả ---
            else
            {
                lblStatus.Text = "Vui lòng nhập giá trị hoặc vị trí để tìm.";
            }
        }

        private void btnClearArray_Click(object sender, EventArgs e)
        {
            arrayManager.ResetArray();
            DrawArray(arrayManager.GetData());
            lblStatus.Text = "Đã xóa mảng. Sẵn sàng tạo mảng mới.";
            DefaultState();
        }
        #endregion

        #region Sorting Control Buttons

        // (ĐÃ SỬA: Thay thế ToggleMainControls)
        private void btn_AutoSort_Click(object sender, EventArgs e)
        {
            isAutoSorting = true;
            if (sortingIterator == null) // Nếu là một lượt chạy mới
            {
                InitializeSortingIterator(Algorithm_Sort.SelectedItem.ToString());
                undoStack.Push(GetCurrentArrayStateSnapshot()); // Lưu trạng thái ban đầu

                // Khóa các nút chính
                CreateGroup(false);
                ActGroup(false);
                GroupBox_Algorithm.Enabled = false;
                btn_Back.Enabled = false; 
            }

            // Cập nhật giao diện nút bấm
            btn_AutoSort.Enabled = false;
            btn_StopSort.Enabled = true;
            btn_Back.Enabled = false;
            btn_ContinueSort.Enabled = false;
            btnClearArray.Enabled = false;
            RunAutoSortDriver(); // Bắt đầu chạy tự động
        }

        private void btn_StopSort_Click(object sender, EventArgs e)
        {
            isAutoSorting = false; // Dừng vòng lặp tự động

            // Cập nhật giao diện nút bấm
            btn_AutoSort.Enabled = true;
            btn_StopSort.Enabled = false;
            btn_Back.Enabled = (undoStack.Count > 1);
            btn_ContinueSort.Enabled = true;
            btnClearArray.Enabled = true;
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 1)
            {
                undoStack.Pop(); // Bỏ trạng thái HIỆN TẠI

                ArrayStateSnapshot previousState = undoStack.Peek(); // Lấy trạng thái TRƯỚC ĐÓ

                arrayManager.RestoreArrayFromSnapshot(previousState.Data);
                DrawArray(arrayManager.GetData());
                ApplyColorsToVisualCells(previousState.Colors);

                lblStatus.Text = "Đã quay lại 1 bước.";

                if (undoStack.Count == 1)
                {
                    btn_Back.Enabled = false;
                }

                sortingIterator = null;
                isAutoSorting = false;
                btn_AutoSort.Enabled = true;
                btn_StopSort.Enabled = false;
                btn_ContinueSort.Enabled = true;
            }
            else if (undoStack.Count == 1)
            {
                btn_Back.Enabled = false;
            }
        }

        // (ĐÃ SỬA: Thay thế ToggleMainControls)
        private void btn_ContinueSort_Click(object sender, EventArgs e)
        {
            isAutoSorting = false;

            if (sortingIterator == null)
            {
                InitializeSortingIterator(Algorithm_Sort.SelectedItem.ToString());
                undoStack.Push(GetCurrentArrayStateSnapshot());

                // Khóa các nút chính
                CreateGroup(false);
                ActGroup(false);
                GroupBox_Algorithm.Enabled = false;
                btnClearArray.Enabled = false;
            }

            // Cập nhật giao diện nút
            btn_AutoSort.Enabled = true;
            btn_StopSort.Enabled = false;
            btn_Back.Enabled = (undoStack.Count > 1);
            btn_ContinueSort.Enabled = true;

            if (sortingIterator.MoveNext())
            {
                lblStatus.Text = "Đã thực hiện 1 bước.";
                undoStack.Push(GetCurrentArrayStateSnapshot());
                btn_Back.Enabled = true;
            }
            else
            {
                HandleSortCompletion();
            }
        }
        #endregion

        #region Sorting Logic

        private void InitializeSortingIterator(string algorithmName)
        {
            undoStack.Clear();
            arrayManager.RestoreArrayFromSnapshot(arraySnapshot);
            DrawArray(arrayManager.GetData());
            ResetAllCellColors();
            lblStatus.Text = "Bắt đầu sắp xếp...";

            Action<int, Color> highlight = HighlightCell;
            Action<int, int> swap = SwapCells;

            if (algorithmName == "Bubble Sort")
                sortingIterator = arrayManager.BubbleSort(highlight, swap);
            else if (algorithmName == "Insertion Sort")
                sortingIterator = arrayManager.InsertionSort(highlight, swap);
        }

        private async Task RunAutoSortDriver()
        {
            // (Logic if (sortingIterator == null) đã được chuyển lên btn_AutoSort_Click)

            int delay = GetAnimationSpeed();

            while (isAutoSorting && sortingIterator.MoveNext())
            {
                undoStack.Push(GetCurrentArrayStateSnapshot());

                await Task.Delay(delay);
            }

            if (isAutoSorting) // Nếu dừng do hết phim
            {
                HandleSortCompletion();
            }
        }

        // (ĐÃ SỬA: Thay thế ToggleMainControls)
        private void HandleSortCompletion()
        {
            lblStatus.Text = "Sắp xếp hoàn tất!";

            bool isCreateEnabled = (arrayManager.GetData().Length == 0);

            DialogResult result = MessageBox.Show(
                $"Đã duyệt xong.\nBạn có muốn duyệt lại từ đầu không?",
                "Hoàn tất Sắp xếp",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                btn_AutoSort_Click(this, EventArgs.Empty);
            }
            else
            {
                // Nếu "No" -> Mở khóa các nút chính
                CreateGroup(isCreateEnabled);
                ActGroup(true);
                GroupBox_Algorithm.Enabled = true;
                btnClearArray.Enabled = true;

                // Tắt các nút điều khiển duyệt
                btn_AutoSort.Enabled = false;
                btn_ContinueSort.Enabled = false;
                btn_StopSort.Enabled = false;
                btn_Back.Enabled = (undoStack.Count > 1); // Vẫn cho phép Undo
            }
        }
        #endregion

        #region Visualization Helpers

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

        private void HighlightCell(int index, Color color)
        {
            if (index >= 0 && index < visualCells.Count)
            {
                visualCells[index].BackColor = color;
                visualCells[index].Refresh();
            }
        }

        private async Task HighlightCellAsync(int index, Color color)
        {
            if (index >= 0 && index < visualCells.Count)
            {
                visualCells[index].BackColor = color;
            }
        }

        // (ĐÃ SỬA: Xóa lỗi "img src")
        private void SwapCells(int index1, int index2)
        {
            if (index1 >= 0 && index1 < visualCells.Count && index2 >= 0 && index2 < visualCells.Count)
            {
                string temp = visualCells[index1].Text;
                visualCells[index1].Text = visualCells[index2].Text;
                visualCells[index2].Text = temp;
                visualCells[index1].Refresh();
                visualCells[index2].Refresh(); // <-- Đã sửa lỗi
            }
        }

        private int GetAnimationSpeed()
        {
            return (tbSpeed.Maximum - tbSpeed.Value + tbSpeed.Minimum) * 100;
        }

        private void ResetAllCellColors()
        {
            foreach (Label cell in visualCells)
            {
                cell.BackColor = colorDefault;
            }
        }

        private ArrayStateSnapshot GetCurrentArrayStateSnapshot()
        {
            int[] currentData = arrayManager.GetData();
            Color[] currentColors = new Color[visualCells.Count];
            for (int i = 0; i < visualCells.Count; i++)
            {
                currentColors[i] = visualCells[i].BackColor;
            }
            return new ArrayStateSnapshot(currentData, currentColors);
        }

        private void ApplyColorsToVisualCells(Color[] colors)
        {
            for (int i = 0; i < visualCells.Count; i++)
            {
                if (i < colors.Length)
                {
                    visualCells[i].BackColor = colors[i];
                }
                else
                {
                    visualCells[i].BackColor = colorDefault;
                }
                visualCells[i].Refresh();
            }
        }
        #endregion

        #region UI Management (ĐÃ SỬA)
        private void DefaultState()
        {
            CreateGroup(true);
            ActGroup(false);
            SortGroup(false);
            GroupBox_Algorithm.Enabled = false;
            btnClearArray.Enabled = false;
            btnCreate_Array.Enabled = false;
            manualInsertPosition = 0;
            input_PositionManual.Text = "0";
        }

        private void CreateGroup(bool isEnabled)
        {
            GroupBox_CreateRandom.Enabled = isEnabled;
            GroupBox_CreateManual.Enabled = isEnabled;
        }

        private void ActGroup(bool isEnabled)
        {
            GroupBox_Act.Enabled = isEnabled;
        }

        private void SortGroup(bool isEnabled)
        {
            GroupBox_Sort.Enabled = isEnabled;

            if (isEnabled)
            {
                btn_AutoSort.Enabled = true;
                btn_ContinueSort.Enabled = true;
                btn_StopSort.Enabled = false;
                btn_Back.Enabled = (undoStack.Count > 1);
            }
        }
        #endregion
    }
}