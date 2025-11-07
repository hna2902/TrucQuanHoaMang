using System;
using System.Drawing; // Cần cho Color
using System.Threading.Tasks; // Cần cho async/await

public class ArrayManager
{
    private int[] dataArray;
    private Random random = new Random();

    public ArrayManager()
    {
        dataArray = new int[0];
    }

    // --- CÁC HÀM LOGIC CƠ BẢN (Giữ nguyên) ---
    public void ResetArray()
    {
        dataArray = new int[0];
    }

    public int[] GetData()
    {
        return dataArray;
    }

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

    // --- CÁC HÀM SẮP XẾP PHIÊN BẢN WINFORMS (ĐÃ HOÀN CHỈNH) ---

    // Các biến 'Func' là cách để 'MainForm' "truyền" các hàm (HighlightCell, SwapCells)
    // vào trong "bộ não" này.
    public async Task BubbleSort(Func<int, Color, Task> highlightCell,
                                 Func<int, int, Task> swapCells,
                                 Func<Task<int>> getSpeed)
    {
        int n = dataArray.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                int delay = await getSpeed(); // Lấy tốc độ từ TrackBar

                // 1. So sánh (Tô vàng)
                await highlightCell(j, Color.Yellow);
                await highlightCell(j + 1, Color.Yellow);
                await Task.Delay(delay);

                if (dataArray[j] > dataArray[j + 1])
                {
                    // 2. Hoán vị (Tô đỏ)
                    await highlightCell(j, Color.Red);
                    await highlightCell(j + 1, Color.Red);
                    await Task.Delay(delay);

                    // Hoán vị dữ liệu
                    int temp = dataArray[j];
                    dataArray[j] = dataArray[j + 1];
                    dataArray[j + 1] = temp;

                    // Hoán vị giao diện
                    await swapCells(j, j + 1);
                    await Task.Delay(delay);
                }

                // 3. Dọn dẹp (Trả về màu mặc định)
                await highlightCell(j, Color.WhiteSmoke);
            }
            // 4. Đã sắp xếp (Tô xanh lá)
            await highlightCell(n - i - 1, Color.LightGreen);

            // Dọn dẹp ô bên cạnh nó
            if (n - i - 2 >= 0)
            {
                await highlightCell(n - i - 2, Color.WhiteSmoke);
            }
        }
        // Ô cuối cùng (ô 0) cũng đã được sắp xếp
        await highlightCell(0, Color.LightGreen);
    }

    public async Task InsertionSort(Func<int, Color, Task> highlightCell,
                                    Func<int, int, Task> swapCells,
                                    Func<Task<int>> getSpeed)
    {
        int n = dataArray.Length;

        // Ô đầu tiên mặc định là đã sắp xếp
        if (n > 0)
        {
            await highlightCell(0, Color.LightGreen);
        }

        for (int i = 1; i < n; i++)
        {
            int j = i;
            int delay = await getSpeed();

            // 1. Đánh dấu "key" (ô đang xét) bằng màu Vàng
            await highlightCell(j, Color.Yellow);
            await Task.Delay(delay);

            // 2. Di chuyển "key" về bên trái
            while (j > 0 && dataArray[j - 1] > dataArray[j])
            {
                delay = await getSpeed();

                // 3. Chuẩn bị hoán vị: Tô màu Đỏ
                await highlightCell(j, Color.Red);
                await highlightCell(j - 1, Color.Red);
                await Task.Delay(delay);

                // 4. Hoán vị dữ liệu
                int temp = dataArray[j];
                dataArray[j] = dataArray[j - 1];
                dataArray[j - 1] = temp;

                // 5. Hoán vị trên UI
                await swapCells(j, j - 1);
                await Task.Delay(delay);

                // 6. Dọn dẹp: Ô bên phải giờ là "đã sắp xếp" (xanh lá)
                await highlightCell(j, Color.LightGreen);
                // "Key" đã lùi về, tô vàng để so sánh tiếp
                await highlightCell(j - 1, Color.Yellow);

                j = j - 1;
                await Task.Delay(delay);
            }

            // 7. Vòng lặp while kết thúc, "key" đã ở đúng vị trí
            // Tô màu Xanh lá cho vị trí cuối cùng của "key"
            await highlightCell(j, Color.LightGreen);
        }
    }
}