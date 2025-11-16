using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

public class ArrayManager
{
    private int[] dataArray;
    private int[] auxArray; // Mảng phụ cho Merge Sort
    private Random random = new Random();

    public ArrayManager()
    {
        dataArray = new int[0];
        auxArray = new int[0]; // Khởi tạo mảng phụ
    }

    public void ResetArray()
    {
        dataArray = new int[0];
        auxArray = new int[0]; // Reset mảng phụ
    }
    public int[] GetData() { return dataArray; }

    public void RestoreArrayFromSnapshot(int[] snapshot)
    {
        dataArray = (int[])snapshot.Clone();
        auxArray = new int[snapshot.Length]; // Reset mảng phụ
    }

    public void CreateArray(int size, bool isRandom)
    {
        if (size <= 0)
        {
            ResetArray();
            return;
        }
        dataArray = new int[size];
        auxArray = new int[size];
        if (isRandom)
        {
            for (int i = 0; i < size; i++)
            {
                dataArray[i] = random.Next(1, 100);
            }
        }
    }

    #region Insert/Delete/Search
    public void InsertElement(int value, int position)
    {
        if (position < 0 || position > dataArray.Length) return;
        int[] newArray = new int[dataArray.Length + 1];
        for (int i = 0; i < position; i++) newArray[i] = dataArray[i];
        newArray[position] = value;
        for (int i = position; i < dataArray.Length; i++) newArray[i + 1] = dataArray[i];
        dataArray = newArray;
        auxArray = new int[dataArray.Length]; // Cập nhật mảng phụ
    }
    public void DeleteElement(int position)
    {
        if (position < 0 || position >= dataArray.Length) return;
        int[] newArray = new int[dataArray.Length - 1];
        for (int i = 0, j = 0; i < dataArray.Length; i++)
        {
            if (i == position) continue;
            newArray[j] = dataArray[i];
            j++;
        }
        dataArray = newArray;
        auxArray = new int[dataArray.Length]; // Cập nhật mảng phụ
    }
    public void DeleteElementByValue(int value)
    {
        int position = SearchElement(value);
        if (position != -1) DeleteElement(position);
    }
    public int SearchElement(int value)
    {
        return Array.IndexOf(dataArray, value);
    }
    #endregion

    public IEnumerator BubbleSort(Action<int, Color> highlightCell, Action<int, int> swapCells, Action<int> highlightCodeLine, Color colorDefault, Color colorSorted)
    {
        int n = dataArray.Length;
        int lastUnsortedIndex = n;
        bool swapped;
        highlightCodeLine(1); // "1. do"
        yield return null;
        do
        {
            highlightCodeLine(2); // "2.   swapped = false"
            swapped = false;
            yield return null;
            highlightCodeLine(3); // "3.   for i = 0..."
            for (int i = 0; i < lastUnsortedIndex - 1; i++)
            {
                highlightCodeLine(4); // "4.     if (A[i] > A[i+1])"
                highlightCell(i, Color.Yellow);
                highlightCell(i + 1, Color.Yellow);
                yield return null;
                if (dataArray[i] > dataArray[i + 1])
                {
                    highlightCodeLine(5); // "5.       swap(A[i], A[i+1])"
                    highlightCell(i, Color.Red);
                    highlightCell(i + 1, Color.Red);
                    yield return null;
                    int temp = dataArray[i];
                    dataArray[i] = dataArray[i + 1];
                    dataArray[i + 1] = temp;
                    swapCells(i, i + 1);
                    highlightCodeLine(6); // "6.       swapped = true"
                    swapped = true;
                    yield return null;
                }
                highlightCell(i, colorDefault);
            }
            if (lastUnsortedIndex - 1 < n && lastUnsortedIndex - 1 >= 0)
            {
                highlightCell(lastUnsortedIndex - 1, colorSorted);
            }
            lastUnsortedIndex--;
            highlightCodeLine(7); // "7. while (swapped)"
            yield return null;
        } while (swapped);
        for (int i = 0; i < n; i++)
        {
            highlightCell(i, colorSorted);
        }
        highlightCodeLine(8); // "8. // End"
        yield return null;
    }

    public IEnumerator InsertionSort(Action<int, Color> highlightCell, Action<int, int> swapCells, Action<int> highlightCodeLine, Color colorDefault, Color colorSorted)
    {
        int n = dataArray.Length;
        highlightCodeLine(1); // "1. mark first element as sorted"
        if (n > 0) highlightCell(0, colorSorted);
        yield return null;
        highlightCodeLine(2); // "2. for each unsorted element X"
        for (int i = 1; i < n; i++)
        {
            highlightCodeLine(3); // "3.   'extract' the element X"
            int key = dataArray[i];
            int j = i - 1;
            highlightCell(i, Color.Yellow);
            yield return null;
            highlightCodeLine(4); // "4.   for j = lastSortedIndex down to 0"
            highlightCodeLine(5); // "5.     if current element j > X"
            while (j >= 0 && dataArray[j] > key)
            {
                highlightCodeLine(6); // "6.       move sorted element to the right by 1"
                highlightCell(j, Color.Red);
                highlightCell(j + 1, Color.Red);
                yield return null;
                dataArray[j + 1] = dataArray[j];
                swapCells(j, j + 1);
                highlightCell(j, colorSorted);
                highlightCell(j + 1, Color.Yellow);
                yield return null;
                j = j - 1;
                if (j >= 0)
                {
                    highlightCodeLine(4);
                    highlightCodeLine(5);
                }
            }
            highlightCodeLine(7); // "7.   break loop and insert X here"
            dataArray[j + 1] = key;
            highlightCell(j + 1, colorSorted);
            swapCells(j + 1, j + 1);
            yield return null;
        }
        highlightCodeLine(8); // "8. // End"
        yield return null;
    }

    public IEnumerator SelectionSort(Action<int, Color> highlightCell, Action<int, int> swapCells, Action<int> highlightCodeLine, Color colorDefault, Color colorSorted)
    {
        int n = dataArray.Length;
        highlightCodeLine(1); // "1. repeat (numOfElements - 1) times"
        yield return null;
        for (int i = 0; i < n - 1; i++)
        {
            highlightCodeLine(2); // "2.   set the first unsorted element as the minimum"
            int min_idx = i;
            highlightCell(i, Color.Red);
            yield return null;
            highlightCodeLine(3); // "3.   for each of the unsorted elements"
            for (int j = i + 1; j < n; j++)
            {
                highlightCodeLine(4); // "4.     if element < currentMinimum"
                highlightCell(j, Color.Yellow);
                yield return null;
                if (dataArray[j] < dataArray[min_idx])
                {
                    highlightCodeLine(5); // "5.       set element as new minimum"
                    highlightCell(min_idx, colorDefault);
                    min_idx = j;
                    highlightCell(min_idx, Color.Red);
                    yield return null;
                }
                else
                {
                    highlightCell(j, colorDefault);
                }
            }
            highlightCodeLine(6); // "6.   swap minimum with first unsorted position"
            int temp = dataArray[min_idx];
            dataArray[min_idx] = dataArray[i];
            dataArray[i] = temp;
            swapCells(min_idx, i);
            highlightCell(min_idx, colorDefault);
            highlightCell(i, colorSorted);
            yield return null;
        }
        if (n > 0) highlightCell(n - 1, colorSorted);
        highlightCodeLine(7); // "7. // End"
        yield return null;
    }


    public IEnumerator MergeSort(Action<int, Color> highlightCell, Action<int, int> swapCells, Action<int> highlightCodeLine, Color colorDefault, Color colorSorted)
    {
        int n = dataArray.Length;
        // Bắt đầu với kích thước (size) là 1, sau đó 2, 4, 8...
        for (int curr_size = 1; curr_size <= n - 1; curr_size = 2 * curr_size)
        {
            // Lấy các cặp mảng con
            for (int left_start = 0; left_start < n - 1; left_start += 2 * curr_size)
            {
                int mid = Math.Min(left_start + curr_size - 1, n - 1);
                int right_end = Math.Min(left_start + 2 * curr_size - 1, n - 1);

                // Tạo một sub-iterator cho hàm Merge
                IEnumerator mergeEnumerator = Merge(left_start, mid, right_end, highlightCell, swapCells, highlightCodeLine, colorDefault, colorSorted);

                // Chạy sub-iterator cho đến khi nó xong
                while (mergeEnumerator.MoveNext())
                {
                    yield return mergeEnumerator.Current; // "yield" từ bên trong Merge
                }
            }
        }
    }

    // Đây là hàm "trộn" (Merge) trực quan hóa
    private IEnumerator Merge(int l, int m, int r,
                            Action<int, Color> highlightCell, Action<int, int> swapCells, Action<int> highlightCodeLine,
                            Color colorDefault, Color colorSorted)
    {
        int i, j, k;
        int n1 = m - l + 1;
        int n2 = r - m;
        highlightCodeLine(1);
        highlightCodeLine(2);
        for (int idx = l; idx <= m; idx++) highlightCell(idx, Color.Orange);
        for (int idx = m + 1; idx <= r; idx++) highlightCell(idx, Color.SkyBlue);
        yield return null;

        // Sao chép dataArray vào mảng phụ auxArray[]
        for (int idx = l; idx <= r; idx++)
        {
            auxArray[idx] = dataArray[idx];
        }

        i = l; // Vị trí bắt đầu của mảng con thứ nhất (trong auxArray)
        j = m + 1; // Vị trí bắt đầu của mảng con thứ hai (trong auxArray)
        k = l; // Vị trí bắt đầu của mảng đã trộn (trong dataArray)

        highlightCodeLine(3); // "3. for i = leftPartIdx to rightPartIdx"
        while (i <= m && j <= r)
        {
            highlightCodeLine(4); // "4.   if leftPartHeadValue <= rightPartHeadValue"
            highlightCell(i, Color.Yellow);
            highlightCell(j, Color.Yellow);
            yield return null;

            if (auxArray[i] <= auxArray[j])
            {
                highlightCodeLine(5); // "5.     copy leftPartHeadValue"
                dataArray[k] = auxArray[i];
                highlightCell(k, Color.Red); // Tô đỏ vị trí sẽ chèn vào
                yield return null;
                swapCells(k, k); // Cập nhật text
                highlightCell(k, colorSorted); // Đã sắp xếp (trong bước này)
                highlightCell(i, colorDefault); // Reset màu cam
                i++;
            }
            else
            {
                highlightCodeLine(6); // "6.   else: copy rightPartHeadValue"
                dataArray[k] = auxArray[j];
                highlightCell(k, Color.Red);
                yield return null;
                swapCells(k, k);
                highlightCell(k, colorSorted);
                highlightCell(j, colorDefault); // Reset màu xanh da trời
                j++;
            }
            k++;
            yield return null;
        }

        // Mã giả: "copy elements back to original array"
        highlightCodeLine(7);

        // Copy các phần tử còn lại của mảng con 1 (nếu có)
        while (i <= m)
        {
            dataArray[k] = auxArray[i];
            highlightCell(k, Color.Red);
            yield return null;
            swapCells(k, k);
            highlightCell(k, colorSorted);
            highlightCell(i, colorDefault);
            i++;
            k++;
        }

        // Copy các phần tử còn lại của mảng con 2 (nếu có)
        while (j <= r)
        {
            dataArray[k] = auxArray[j];
            highlightCell(k, Color.Red);
            yield return null;
            swapCells(k, k);
            highlightCell(k, colorSorted);
            highlightCell(j, colorDefault);
            j++;
            k++;
        }

        // Reset lại màu của toàn bộ đoạn đã merge về sorted
        for (int idx = l; idx <= r; idx++) highlightCell(idx, colorSorted);
        yield return null;
    }
    public IEnumerator QuickSort(Action<int, Color> highlightCell, Action<int, int> swapCells, Action<int> highlightCodeLine, Color colorDefault, Color colorSorted)
    {
        int n = dataArray.Length;
        if (n <= 1)
        {
            if (n == 1) highlightCell(0, colorSorted);
            yield break; // Mảng đã được sắp xếp
        }

        // Stack để mô phỏng đệ quy
        Stack<int> stack = new Stack<int>();

        highlightCodeLine(1); // "1. Tạo Stack"
        yield return null;

        // Đẩy cặp chỉ số (low, high) ban đầu vào stack
        stack.Push(0);
        stack.Push(n - 1);
        highlightCodeLine(2); // "2. Đẩy (0, n-1) vào Stack"
        yield return null;

        highlightCodeLine(3); // "3. trong_khi (Stack không rỗng)"
        while (stack.Count > 0)
        {
            // Pop 'high' và 'low'
            int high = stack.Pop();
            int low = stack.Pop();
            highlightCodeLine(4); // "4.   lấy high, low từ Stack"
            yield return null;

            // (Dựa trên mã giả của bạn)

            int pivotIndex = low;
            int pivotValue = dataArray[pivotIndex];
            highlightCodeLine(5); // "5.   pivot = a[low]"
            highlightCell(pivotIndex, Color.Blue); // Tô màu pivot
            yield return null;

            int storeIndex = pivotIndex + 1;
            highlightCodeLine(6); // "6.   storeIndex = low + 1"
            yield return null;

            highlightCodeLine(7); // "7.   với i = low + 1 đến high"
            for (int i = low + 1; i <= high; i++)
            {
                highlightCodeLine(8); // "8.     nếu (a[i] < pivot)"
                highlightCell(i, Color.Yellow); // Tô màu phần tử đang xét
                yield return null;

                if (dataArray[i] < pivotValue)
                {
                    highlightCodeLine(9); // "9.       hoán_đổi(a[i], a[storeIndex])"
                    highlightCell(i, Color.Red);
                    highlightCell(storeIndex, Color.Red);
                    yield return null;

                    // Hoán đổi dữ liệu
                    int temp = dataArray[i];
                    dataArray[i] = dataArray[storeIndex];
                    dataArray[storeIndex] = temp;
                    // Hoán đổi hình ảnh
                    swapCells(i, storeIndex);

                    highlightCell(i, colorDefault);
                    highlightCell(storeIndex, colorDefault);
                    yield return null;

                    highlightCodeLine(10); // "10.      tăng storeIndex"
                    storeIndex++;
                }
                else
                {
                    // Chỉ reset màu nếu không hoán đổi
                    highlightCell(i, colorDefault);
                }
            }

            // Đưa pivot về đúng vị trí
            int finalPivotPosition = storeIndex - 1;
            highlightCodeLine(11); // "11.  hoán_đổi(a[low], a[storeIndex - 1])"

            // (Chỉ hoán đổi nếu pivot không ở đúng vị trí)
            if (pivotIndex != finalPivotPosition)
            {
                highlightCell(pivotIndex, Color.Red);
                highlightCell(finalPivotPosition, Color.Red);
                yield return null;

                int tempPivot = dataArray[pivotIndex];
                dataArray[pivotIndex] = dataArray[finalPivotPosition];
                dataArray[finalPivotPosition] = tempPivot;
                swapCells(pivotIndex, finalPivotPosition);

                highlightCell(pivotIndex, colorDefault);
            }

            // Đánh dấu pivot là đã sắp xếp
            highlightCell(finalPivotPosition, colorSorted);
            yield return null;

            // Đẩy 2 mảng con mới vào stack

            // 1. Mảng con bên trái
            if (finalPivotPosition - 1 > low)
            {
                stack.Push(low);
                stack.Push(finalPivotPosition - 1);
                highlightCodeLine(12); // "12.  Đẩy mảng con (trái) vào Stack"
                yield return null;
            }

            // 2. Mảng con bên phải
            if (finalPivotPosition + 1 < high)
            {
                stack.Push(finalPivotPosition + 1);
                stack.Push(high);
                highlightCodeLine(13); // "13.  Đẩy mảng con (phải) vào Stack"
                yield return null;
            }

            highlightCodeLine(3); // Quay về đầu vòng lặp while
        }

        highlightCodeLine(14); // "14. // Kết thúc"
        // (Tô màu lại toàn bộ cho chắc, vì QuickSort không tô tuần tự)
        for (int i = 0; i < n; i++)
        {
            highlightCell(i, colorSorted);
        }
        yield return null;
    }

    public IEnumerator RandomQuickSort(Action<int, Color> highlightCell, Action<int, int> swapCells, Action<int> highlightCodeLine, Color colorDefault, Color colorSorted)
    {
        int n = dataArray.Length;
        if (n <= 1)
        {
            if (n == 1) highlightCell(0, colorSorted);
            yield break; // Mảng đã được sắp xếp
        }

        // Stack để mô phỏng đệ quy
        Stack<int> stack = new Stack<int>();

        highlightCodeLine(1); // "1. Tạo Stack"
        yield return null;

        // Đẩy cặp chỉ số (low, high) ban đầu vào stack
        stack.Push(0);
        stack.Push(n - 1);
        highlightCodeLine(2); // "2. Đẩy (0, n-1) vào Stack"
        yield return null;

        highlightCodeLine(3); // "3. trong_khi (Stack không rỗng)"
        while (stack.Count > 0)
        {
            // Pop 'high' và 'low'
            int high = stack.Pop();
            int low = stack.Pop();
            highlightCodeLine(4); // "4.   lấy high, low từ Stack"
            yield return null;

            int randIndex = random.Next(low, high + 1);
            highlightCodeLine(5); // "5.   Chọn pivot ngẫu nhiên"
            yield return null;

            // Hoán đổi pivot ngẫu nhiên về vị trí 'low'
            int tempRand = dataArray[randIndex];
            dataArray[randIndex] = dataArray[low];
            dataArray[low] = tempRand;
            swapCells(randIndex, low);
            highlightCodeLine(6); // "6.   Hoán đổi pivot về đầu"
            yield return null;


            int pivotIndex = low;
            int pivotValue = dataArray[pivotIndex];
            highlightCodeLine(5); // "7.   pivot = a[low]"
            highlightCell(pivotIndex, Color.Blue); // Tô màu pivot
            yield return null;

            int storeIndex = pivotIndex + 1;
            highlightCodeLine(6); // "8.   storeIndex = low + 1"
            yield return null;

            highlightCodeLine(7); // "9.   với i = low + 1 đến high"
            for (int i = low + 1; i <= high; i++)
            {
                highlightCodeLine(8); // "10.     nếu (a[i] < pivot)"
                highlightCell(i, Color.Yellow); // Tô màu phần tử đang xét
                yield return null;

                if (dataArray[i] < pivotValue)
                {
                    highlightCodeLine(9); // "11.       hoán_đổi(a[i], a[storeIndex])"
                    highlightCell(i, Color.Red);
                    highlightCell(storeIndex, Color.Red);
                    yield return null;

                    // Hoán đổi dữ liệu
                    int temp = dataArray[i];
                    dataArray[i] = dataArray[storeIndex];
                    dataArray[storeIndex] = temp;
                    // Hoán đổi hình ảnh
                    swapCells(i, storeIndex);

                    highlightCell(i, colorDefault);
                    highlightCell(storeIndex, colorDefault);
                    yield return null;

                    highlightCodeLine(10); // "12.      tăng storeIndex"
                    storeIndex++;
                }
                else
                {
                    // Chỉ reset màu nếu không hoán đổi
                    highlightCell(i, colorDefault);
                }
            }

            // Đưa pivot về đúng vị trí
            int finalPivotPosition = storeIndex - 1;
            highlightCodeLine(11); // "11.  hoán_đổi(a[low], a[storeIndex - 1])"

            // (Chỉ hoán đổi nếu pivot không ở đúng vị trí)
            if (pivotIndex != finalPivotPosition)
            {
                highlightCell(pivotIndex, Color.Red);
                highlightCell(finalPivotPosition, Color.Red);
                yield return null;

                int tempPivot = dataArray[pivotIndex];
                dataArray[pivotIndex] = dataArray[finalPivotPosition];
                dataArray[finalPivotPosition] = tempPivot;
                swapCells(pivotIndex, finalPivotPosition);

                highlightCell(pivotIndex, colorDefault);
            }

            // Đánh dấu pivot là đã sắp xếp
            highlightCell(finalPivotPosition, colorSorted);
            yield return null;

            // Đẩy 2 mảng con mới vào stack

            // 1. Mảng con bên trái
            if (finalPivotPosition - 1 > low)
            {
                stack.Push(low);
                stack.Push(finalPivotPosition - 1);
                highlightCodeLine(12); // "12.  Đẩy mảng con (trái) vào Stack"
                yield return null;
            }

            // 2. Mảng con bên phải
            if (finalPivotPosition + 1 < high)
            {
                stack.Push(finalPivotPosition + 1);
                stack.Push(high);
                highlightCodeLine(13); // "13.  Đẩy mảng con (phải) vào Stack"
                yield return null;
            }

            highlightCodeLine(3); // Quay về đầu vòng lặp while
        }

        highlightCodeLine(14); // "14. // Kết thúc"
        // (Tô màu lại toàn bộ cho chắc, vì QuickSort không tô tuần tự)
        for (int i = 0; i < n; i++)
        {
            highlightCell(i, colorSorted);
        }
        yield return null;
    }
}