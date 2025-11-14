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
    public void CreateArray(int size, bool isRandom)
    {
        if (size <= 0)
        {
            dataArray = new int[0];
            return;
        }

        dataArray = new int[size];

        if (isRandom)
        {
            for (int i = 0; i < size; i++)
            {
                dataArray[i] = random.Next(1, 100);
            }
        }
    }

    public void InsertElement(int value, int position)
    {
        if (position < 0 || position > dataArray.Length)
        {
            return;
        }

        int[] newArray = new int[dataArray.Length + 1];
        for (int i = 0; i < position; i++)
        {
            newArray[i] = dataArray[i];
        }
        newArray[position] = value;

        for (int i = position; i < dataArray.Length; i++)
        {
            newArray[i + 1] = dataArray[i];
        }

        dataArray = newArray;
    }

    public void DeleteElement(int position)
    {
        if (position < 0 || position >= dataArray.Length)
        {
            return;
        }

        int[] newArray = new int[dataArray.Length - 1];
        for (int i = 0, j = 0; i < dataArray.Length; i++)
        {
            if (i == position)
            {
                continue;
            }
            newArray[j] = dataArray[i];
            j++;
        }

        dataArray = newArray;
    }

    public void DeleteElementByValue(int value)
    {
        int position = SearchElement(value);
        if (position != -1)
        {
            DeleteElement(position);
        }
    }

    public int SearchElement(int value)
    {
        return Array.IndexOf(dataArray, value);
    }

    // --- THAY ĐỔI CÁC HÀM SẮP XẾP ---
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