using System;
using System.Drawing;
using System.Collections; // <-- THAY ĐỔI: Thêm thư viện này
//using System.Threading.Tasks; // <-- THAY ĐỔI: Xóa thư viện này

public class ArrayManager
{
    private int[] dataArray;
    private Random random = new Random();

    public ArrayManager()
    {
        dataArray = new int[0];
    }

    // --- CÁC HÀM CƠ BẢN (Giữ nguyên) ---
    public void ResetArray() { dataArray = new int[0]; }
    public int[] GetData() { return dataArray; }
    public void RestoreArrayFromSnapshot(int[] snapshot) { dataArray = (int[])snapshot.Clone(); }
    public void CreateArray(int size, bool isRandom) { /* ... (Giữ nguyên) ... */ }
    public void InsertElement(int value, int position) { /* ... (Giữ nguyên) ... */ }
    public void DeleteElement(int position) { /* ... (Giữ nguyên) ... */ }
    public void DeleteElementByValue(int value) { /* ... (Giữ nguyên) ... */ }
    public int SearchElement(int value) { return Array.IndexOf(dataArray, value); }

    // --- THAY ĐỔI CÁC HÀM SẮP XẾP ---

    // Đổi từ "async Task" thành "IEnumerator"
    // Đổi tham số "Func<...Task>" thành "Action<...>" (hàm void)
    // Bỏ tham số "getSpeed"
    public IEnumerator BubbleSort(Action<int, Color> highlightCell,
                                 Action<int, int> swapCells)
    {
        int n = dataArray.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                highlightCell(j, Color.Yellow);
                highlightCell(j + 1, Color.Yellow);
                yield return null; // Tạm dừng 1 bước

                if (dataArray[j] > dataArray[j + 1])
                {
                    highlightCell(j, Color.Red);
                    highlightCell(j + 1, Color.Red);
                    yield return null; // Tạm dừng 1 bước

                    int temp = dataArray[j];
                    dataArray[j] = dataArray[j + 1];
                    dataArray[j + 1] = temp;
                    swapCells(j, j + 1);
                    yield return null; // Tạm dừng 1 bước
                }

                highlightCell(j, Color.WhiteSmoke);
            }
            highlightCell(n - i - 1, Color.LightGreen);
            if (n - i - 2 >= 0)
                highlightCell(n - i - 2, Color.WhiteSmoke);
        }
        if (n > 0) highlightCell(0, Color.LightGreen);
    }

    public IEnumerator InsertionSort(Action<int, Color> highlightCell,
                                     Action<int, int> swapCells)
    {
        int n = dataArray.Length;
        if (n > 0) highlightCell(0, Color.LightGreen);

        for (int i = 1; i < n; i++)
        {
            int j = i;
            highlightCell(j, Color.Yellow);
            yield return null; // Tạm dừng 1 bước

            while (j > 0 && dataArray[j - 1] > dataArray[j])
            {
                highlightCell(j, Color.Red);
                highlightCell(j - 1, Color.Red);
                yield return null; // Tạm dừng 1 bước

                int temp = dataArray[j];
                dataArray[j] = dataArray[j - 1];
                dataArray[j - 1] = temp;
                swapCells(j, j - 1);
                yield return null; // Tạm dừng 1 bước

                highlightCell(j, Color.LightGreen);
                highlightCell(j - 1, Color.Yellow);
                j = j - 1;
                yield return null; // Tạm dừng 1 bước
            }
            highlightCell(j, Color.LightGreen);
        }
    }
}