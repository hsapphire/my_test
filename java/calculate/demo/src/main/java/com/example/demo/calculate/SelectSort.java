package com.example.demo.calculate;

import java.util.Arrays;

public class SelectSort {
    public static void main(String[] args) {
        int[] arr={11,3,1};
        selectionSort(arr);
        System.out.println(Arrays.toString(arr));
    }

    /**
       * 选择排序
       * @param array
       * @return
       */
 public static int[] selectionSort(int[]array) {
   if(array.length ==0)
     return array;
    for(int i  =0; i <array.length; i++) {
     int minIndex = i;
     for(int j = i; j <array.length; j++) {
       if(array[j] <array[minIndex])//找到最小的数
         minIndex = j;//将最小数的索引保存
     }
     int temp =array[minIndex];
     array[minIndex] =array[i];
     array[i] = temp;
   }
   return array;
 }


    /**
     * 冒泡排序
     *
     * @param arr
     */
    public static void bubbleSort1(int[] arr) {
        for (int i = 0; i < arr.length - 1; i++) {
            boolean flag = true;//设定一个标记，若为true，则表示此次循环没有进行交换，也就是待排序列已经有序，排序已然完成。
            for (int j = 0; j < arr.length - 1 - i; j++) {
                if (arr[j] > arr[j + 1]) {
                    swap(arr,j,j+1);
                    flag = false;
                }
            }
            if (flag) {
                break;
            }
        }
    }

    public static void swap(int []arr,int a,int b){
        arr[a] = arr[a]+arr[b];
        arr[b] = arr[a]-arr[b];
        arr[a] = arr[a]-arr[b];
    }

}
