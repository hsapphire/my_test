package com.example.demo.calculate;

import java.util.Arrays;

public class ShellSort {
    public static void main(String[] args) {
        int[] arr={8,9,1,7,2,3,5,4,6,0};
        ShellSort(arr);
        System.out.println(Arrays.toString(arr));
    }


         public static int[] ShellSort(int[]array) {
             int len = array.length;
             int temp, gap = len / 2;
             while (gap > 0) {
                 for (int i = gap; i < len; i++) {
                     temp = array[i];
                     int preIndex = i - gap;
                     while (preIndex >= 0 && array[preIndex] > temp) {
                         array[preIndex + gap] = array[preIndex];
                         preIndex -= gap;
                     }
                     array[preIndex + gap] = temp;
                 }
                 gap /= 2;
             }
             return array;
         }


}
