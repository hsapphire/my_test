package com.example.demo.calculate;

import java.util.Arrays;

public class InsertSort {
    public static void main(String[] args) {
        int[] arr={11,3,1};
        insertionSort1(arr);
        System.out.println(Arrays.toString(arr));
    }

    /**
       * 选择排序
       * @param array
       * @return
       */
    public static int[] insertionSort(int[]array) {
        if (array.length == 0)
            return array;
        int current;
        for (int i = 0; i < array.length - 1; i++) {
            current = array[i + 1];
            int preIndex = i;
            while (preIndex >= 0 && current < array[preIndex]) {
                array[preIndex + 1] = array[preIndex];
                preIndex--;
            }
            array[preIndex + 1] = current;
        }
        return array;
    }


    public static void insertionSort1(int[] arr) {
        for (int i = 1; i < arr.length; i++) {
            int j = i;
            while (j > 0 && arr[j] < arr[j - 1]) {
                int temp=arr[j];
                arr[j]=arr[j-1];
                arr[j-1]=temp;
                j--;
            }
        }
    }

}
