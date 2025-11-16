using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace TrucQuanHoaMang
{
    public partial class Form1 : Form
    {
        private ArrayManager arrayManager;
        private List<Label> visualCells = new List<Label>();
        private int manualInsertPosition = 0;
        private int[] arraySnapshot;
        private Stack<ArrayStateSnapshot> undoStack = new Stack<ArrayStateSnapshot>();

        // Biến trạng thái sắp xếp mới
        private IEnumerator sortingIterator; // "Cuộn phim"
        private bool isAutoSorting = false;  // Cờ báo đang chạy tự động
        private Color colorDefault = Color.WhiteSmoke;
        private Color colorCompare = Color.Yellow;
        private Color colorSwap = Color.Red;
        private Color colorSorted = Color.LightGreen;

        public Form1()
        {
            InitializeComponent();
            undoStack.Clear();
            arrayManager = new ArrayManager();

            Algorithm_Sort.SelectedIndex = 0;
            input_PositionManual.ReadOnly = true;
            input_PositionManual.Text = "0";
            panelVisualizer.AutoScroll = true;

            // DefaultState dùng để thiết lập trạng thái ban đầu
            DefaultState();
        }

        // Xử lý nút bấm
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
            SortGroup(true);
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
            GroupBox_Algorithm.Enabled = true;
            SortGroup(true);
            sortingIterator = null;
            undoStack.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string valueText = input_ValueAct.Text;
            string posText = input_PositionAct.Text;
            bool hasValue = !string.IsNullOrEmpty(valueText);
            bool hasPos = !string.IsNullOrEmpty(posText);
            bool deleted = false; // Cờ để kiểm tra xem đã xóa hay chưa

            // Nhập cả hai (Phải khớp mới xóa)
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
            // Chỉ nhập Giá trị
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
            // Chỉ nhập Vị trí
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
            // Không nhập gì cả
            else
            {
                lblStatus.Text = "Vui lòng nhập giá trị hoặc vị trí để xóa.";
            }

            // Kiểm tra sau khi xóa
            if (deleted)
            {
                DrawArray(arrayManager.GetData()); // Vẽ lại mảng
                arraySnapshot = (int[])arrayManager.GetData().Clone();
                sortingIterator = null;
                undoStack.Clear();
                // Kiểm tra xem mảng có rỗng sau khi xóa không
                if (arrayManager.GetData().Length == 0)
                {
                    lblStatus.Text = "Mảng rỗng. Đã reset về trạng thái ban đầu.";
                    DefaultState(); // Gọi hàm reset
                }
            }
            GroupBox_Algorithm.Enabled = true;
            SortGroup(true);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string valueText = input_ValueAct.Text;
            string posText = input_PositionAct.Text;
            bool hasValue = !string.IsNullOrEmpty(valueText);
            bool hasPos = !string.IsNullOrEmpty(posText);

            // Nhập cả hai 
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
            // Chỉ nhập Giá trị
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
            // Chỉ nhập Vị trí
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
            // Không nhập gì cả
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
            sortingIterator = null;
            undoStack.Clear();
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
            ActGroup(true);
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

            LoadPseudocodeToDisplay(algorithmName);

            // Tạo 3 delegate
            Action<int, Color> highlight = HighlightCell;
            Action<int, int> swap = SwapCells;
            Action<int> highlightLine = HighlightCodeLine;

            // --- SỬA LẠI TOÀN BỘ CÁC LỆNH GỌI HÀM ---
            // (Chúng ta truyền 5 tham số, bao gồm 2 màu)
            if (algorithmName == "Bubble Sort")
            {
                sortingIterator = arrayManager.BubbleSort(highlight, swap, highlightLine, colorDefault, colorSorted);
            }
            else if (algorithmName == "Insertion Sort")
            {
                sortingIterator = arrayManager.InsertionSort(highlight, swap, highlightLine, colorDefault, colorSorted);
            }
            else if (algorithmName == "Selection Sort")
            {
                sortingIterator = arrayManager.SelectionSort(highlight, swap, highlightLine, colorDefault, colorSorted);
            }
            else if (algorithmName == "Merge Sort")
            {
                sortingIterator = arrayManager.MergeSort(highlight, swap, highlightLine, colorDefault, colorSorted);
            }
            else if (algorithmName == "Quick Sort")
            {
                sortingIterator = arrayManager.QuickSort(highlight, swap, highlightLine, colorDefault, colorSorted);
            }
            else if (algorithmName == "Random Quick Sort")
            {
                sortingIterator = arrayManager.QuickSort(highlight, swap, highlightLine, colorDefault, colorSorted);
            }
        }

        private async Task RunAutoSortDriver()
        {
            // (Logic if (sortingIterator == null) đã được chuyển lên btn_AutoSort_Click)
            while (isAutoSorting && sortingIterator.MoveNext())
            {
                int delay = GetAnimationSpeed();
                undoStack.Push(GetCurrentArrayStateSnapshot());

                await Task.Delay(delay);
            }

            if (isAutoSorting) // Nếu dừng do hết phim
            {
                HandleSortCompletion();
            }
        }

        private void HandleSortCompletion()
        {
            lblStatus.Text = "Sắp xếp hoàn tất!";

            bool isCreateEnabled = (arrayManager.GetData().Length == 0);
            MessageBox.Show(
                "Đã duyệt xong.", // Thông báo đơn giản
                "Hoàn tất Sắp xếp",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            CreateGroup(isCreateEnabled);
            ActGroup(true);
            GroupBox_Algorithm.Enabled = true;
            btnClearArray.Enabled = true;
            btn_AutoSort.Enabled = false;
            btn_ContinueSort.Enabled = false;
            btn_StopSort.Enabled = false;
            btn_Back.Enabled = (undoStack.Count > 1); // Vẫn cho phép Undo
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

        private void SwapCells(int index1, int index2)
        {
            if (index1 >= 0 && index1 < visualCells.Count && index2 >= 0 && index2 < visualCells.Count)
            {
                // TRƯỜNG HỢP 1: "Set Value" (index1 == index2)
                // Dùng cho MergeSort và InsertionSort để cập nhật giá trị
                if (index1 == index2)
                {
                    int index = index1;

                    // Lấy giá trị ĐÚNG từ data model (vì dataArray đã được cập nhật)
                    // Đây là bước mấu chốt!
                    int correctValue = arrayManager.GetData()[index];

                    visualCells[index].Text = correctValue.ToString();
                    visualCells[index].Refresh();
                }
                // TRƯỜNG HỢP 2: "Swap" (index1 != index2)
                // Dùng cho BubbleSort và SelectionSort
                else
                {
                    string temp = visualCells[index1].Text;
                    visualCells[index1].Text = visualCells[index2].Text;
                    visualCells[index2].Text = temp;
                    visualCells[index1].Refresh();
                    visualCells[index2].Refresh();
                }

                // *** KẾT THÚC SỬA LỖI ***
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
        private void LoadPseudocodeToDisplay(string algorithmName)
        {
            rtbCodeDisplay.Text = ""; // Xóa code cũ

            if (algorithmName == "Bubble Sort")
            {
                rtbCodeDisplay.Text =
                    "1. Bắt đầu vòng lặp do-while\n" +
                    "2.    Đặt biến 'swapped' = false\n" +
                    "3.    Duyệt mảng với i = 0 đến N-2\n" +
                    "4.       Nếu (A[i] > A[i+1])\n" +
                    "5.          Hoán_đổi(A[i], A[i+1])\n" +
                    "6.          Đặt 'swapped' = true\n" +
                    "7. Lặp lại (while) nếu 'swapped' == true\n" +
                    "8. // Kết thúc";
            }
            else if (algorithmName == "Insertion Sort")
            {
                rtbCodeDisplay.Text =
                    "1. Đánh dấu A[0] là đã_sắp_xếp\n" +
                    "2. Lặp với i = 1 đến N-1\n" +
                    "3.    Lấy giá trị 'key' = A[i]\n" +
                    "4.    Đặt j = i - 1\n" +
                    "5.    trong_khi (j >= 0 và A[j] > key)\n" +
                    "6.       Dời A[j] sang phải: A[j+1] = A[j]\n" +
                    "7.       Giảm j (j--)\n" +
                    "8.    Chèn 'key' vào vị trí đúng: A[j+1] = key\n" +
                    "9. // Kết thúc";
            }
            else if (algorithmName == "Selection Sort")
            {
                rtbCodeDisplay.Text =
                    "1. Lặp với i = 0 đến N-2\n" +
                    "2.    Đặt 'min_idx' = i\n" +
                    "3.    Lặp với j = i + 1 đến N-1\n" +
                    "4.       Nếu (A[j] < A[min_idx])\n" +
                    "5.          Đặt 'min_idx' = j\n" +
                    "6.    (Sau khi lặp j) Hoán_đổi(A[i], A[min_idx])\n" +
                    "7. // Kết thúc";
            }
            else if (algorithmName == "Merge Sort")
            {
                rtbCodeDisplay.Text =
                    "1. Lặp với 'curr_size' = 1, 2, 4, 8...\n" +
                    "2.    Lặp với 'left_start' = 0, 0 + 2*'curr_size'...\n" +
                    "3.       (Bắt đầu hàm Merge(l, m, r))\n" +
                    "4.       Sao chép A[l...r] vào mảng 'auxArray'[l...r]\n" +
                    "5.       Đặt i = l, j = m + 1, k = l\n" +
                    "6.       trong_khi (i <= m và j <= r)\n" +
                    "7.          Nếu ('auxArray'[i] <= 'auxArray'[j])\n" +
                    "8.             A[k] = 'auxArray'[i]; tăng i; tăng k\n" +
                    "9.          Ngược lại: A[k] = 'auxArray'[j]; tăng j; tăng k\n" +
                    "10.      Sao chép phần còn lại (nếu có) từ 'auxArray' về A\n" +
                    "11. // Kết thúc";
            }
            else if (algorithmName == "Quick Sort")
            {
                // Đây là bản gốc (pivot = A[low])
                rtbCodeDisplay.Text =
                    "1. Tạo một Stack\n" +
                    "2. Đẩy (0, n-1) vào Stack\n" +
                    "3. trong_khi (Stack không rỗng)\n" +
                    "4.    Pop high, low từ Stack\n" +
                    "5.    Chọn 'pivotIndex' = low, 'pivotValue' = A[pivotIndex]\n" +
                    "6.    Đặt 'storeIndex' = low + 1\n" +
                    "7.    Lặp với i = low + 1 đến high\n" +
                    "8.       Nếu (A[i] < 'pivotValue')\n" +
                    "9.          Hoán_đổi(A[i], A[storeIndex])\n" +
                    "10.         Tăng 'storeIndex'\n" +
                    "11.   Hoán_đổi(A[pivotIndex], A[storeIndex - 1])\n" +
                    "12.   Đẩy đoạn mảng con (trái) vào Stack\n" +
                    "13.   Đẩy đoạn mảng con (phải) vào Stack\n" +
                    "14. // Kết thúc";
            }
            else if (algorithmName == "Random Quick Sort")
            {
                // Đây là bản ngẫu nhiên
                rtbCodeDisplay.Text =
                    "1. Tạo một Stack\n" +
                    "2. Đẩy (0, n-1) vào Stack\n" +
                    "3. trong_khi (Stack không rỗng)\n" +
                    "4.    Pop high, low từ Stack\n" +
                    "5.    Chọn 'randIndex' = ngẫu_nhiên(low, high)\n" +
                    "6.    Hoán_đổi(A[randIndex], A[low])\n" +
                    "7.    Chọn 'pivotIndex' = low, 'pivotValue' = A[pivotIndex]\n" +
                    "8.    Đặt 'storeIndex' = low + 1\n" +
                    "9.    Lặp với i = low + 1 đến high\n" +
                    "10.      Nếu (A[i] < 'pivotValue')\n" +
                    "11.         Hoán_đổi(A[i], A[storeIndex])\n" +
                    "12.         Tăng 'storeIndex'\n" +
                    "13.   Hoán_đổi(A[pivotIndex], A[storeIndex - 1])\n" +
                    "14.   Đẩy đoạn mảng con (trái) vào Stack\n" +
                    "15.   Đẩy đoạn mảng con (phải) vào Stack\n" +
                    "16. // Kết thúc";
            }
            // Đặt màu nền mặc định cho tất cả
            rtbCodeDisplay.SelectAll();
            rtbCodeDisplay.SelectionBackColor = rtbCodeDisplay.BackColor;
            rtbCodeDisplay.SelectionColor = Color.Black; // Đảm bảo chữ luôn đen
            rtbCodeDisplay.DeselectAll();
        }

        // Hàm mới: Tô sáng 1 dòng code
        private void HighlightCodeLine(int lineNumber)
        {
            // 1. Reset tất cả màu nền VÀ MÀU CHỮ
            rtbCodeDisplay.SelectAll();
            rtbCodeDisplay.SelectionBackColor = rtbCodeDisplay.BackColor;
            rtbCodeDisplay.SelectionColor = Color.Black; // <-- LUÔN ĐỂ CHỮ MÀU ĐEN

            // 2. Tìm dòng cần tô
            if (lineNumber > 0 && lineNumber <= rtbCodeDisplay.Lines.Length)
            {
                int lineIndex = lineNumber - 1;
                int start = rtbCodeDisplay.GetFirstCharIndexFromLine(lineIndex);
                string lineText = rtbCodeDisplay.Lines[lineIndex];

                // 3. Chọn và tô màu dòng đó
                if (start >= 0 && lineText.Length > 0)
                {
                    rtbCodeDisplay.Select(start, lineText.Length);
                    rtbCodeDisplay.SelectionBackColor = Color.FromArgb(255, 60, 60); // Màu nền tối
                    rtbCodeDisplay.SelectionColor = Color.White; 
                }
            }
            rtbCodeDisplay.DeselectAll(); // Bỏ chọn
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

        private void rtbCodeDisplay_TextChanged(object sender, EventArgs e)
        {

        }
    }
}