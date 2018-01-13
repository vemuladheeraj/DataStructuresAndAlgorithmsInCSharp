﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace DataStructuresAndAlgorithms
{
    /*
    In an given N X M matrix all rows and column values are increasing order.
    1. Diagnols doesnt matter. 
    2. Need not to be continuous.
    
    E.g.

    1   2   3   4   8
    2   3   4   5   9
    3   4   5   6   10
    4   5   6   7   11
    6   8   9   10  12
    9   10  11  12  13

    Refer find kth Smallest in sorted array from here https://leetcode.com/problems/kth-smallest-element-in-a-sorted-matrix/
    http://stackoverflow.com/a/2458113/2466650
    Selection Algo http://stackoverflow.com/questions/5000836/selection-algorithms-on-sorted-matrix?rq=1
    http://stackoverflow.com/questions/23875945/want-to-know-time-complexity-while-using-binary-search-logic-for-sorted-matrix
    http://stackoverflow.com/questions/3723353/how-to-efficiently-search-in-an-ordered-matrix?rq=1
    http://en.wikipedia.org/wiki/Z-order_curve
    http://twistedoakstudios.com/blog/Post5365_searching-a-sorted-matrix-faster
    
    */
    partial class MatrixOperations
    {
        //O(n) Solution
        public bool SearchMatrixBruteForce(int[,] matrix, int target)
        {
            bool foundFlag = false;
            for (int lpRCnt = matrix.GetLength(0) - 1; lpRCnt >= 0; )
            {
                for (int lpCCnt = 0 ; lpCCnt < matrix.GetLength(0);)
                {
                    if (matrix[lpRCnt, lpCCnt] == target)
                    {
                        foundFlag = true; 
                        break;
                    }
                    //else
                } 
            } return foundFlag;

        }

        //240 O(M+N) Top Right to Bottom Left https://leetcode.com/problems/search-a-2d-matrix-ii/description/
        public bool SearchMatrix(int[,] matrix, int target)
        {
            if (matrix == null || matrix.Length == 0)
                return false;

            int rowIndx = 0;
            int rowLength = matrix.GetLength(0) - 1;
            int colIndx = matrix.GetLength(1) - 1;

            while (rowIndx <= rowLength && colIndx >= 0)
            {
                if (matrix[rowIndx, colIndx] == target)
                {
                    return true;
                }
                else if (matrix[rowIndx, colIndx] > target)
                {
                    --colIndx;
                }
                else
                {
                    ++rowIndx;
                }
            }

            return false;
        }

        //Non-Optimal - but best one in worst case.
        public void SearchSortedMatrixInNLogN(int[,] matrix, int elementToSearch)
        {
            /*
            O(m log n) Approach
            Repeat one loop for row or column which ever is shorter in lenght (In the above matrix it is column).
            And use binary search technique for other row or column loop.
            */
            int shortLpLen = 0;
            bool isRowShorter = false;

            int binSearchLpCnt = 0;
            int binSearchLpLen = 0;

            int midElementVal = 0;
            int midElementInd = 0;

            if (matrix.Length > matrix.GetLength(1))
            {
                shortLpLen = matrix.GetLength(1);
                binSearchLpLen = matrix.Length;
                isRowShorter = true;
            }
            else
            {
                shortLpLen = matrix.GetLength(0);
                binSearchLpLen = matrix.GetLength(1);
            }

            for (int lpShrtrLpCnt = 0; lpShrtrLpCnt < shortLpLen; lpShrtrLpCnt++)
            {
                while (binSearchLpCnt < binSearchLpLen)
                {
                    midElementInd = binSearchLpLen / 2;
                    midElementVal = (isRowShorter == true) ? matrix[lpShrtrLpCnt, midElementInd] : matrix[midElementInd, lpShrtrLpCnt];

                }
            }
        }
        
        /*
        Time Complexity has to be calculated correctly.

        1,    4,      7,      10,     13,
        2,    5,      8,      11,     14,
        3,    6,      9,      12,     15,
        16,   17,     18,     19,     20,
        21,   22,     23,     24,     25,
        26,   27,     28,     29,     30

        Used the logic from Stack Over flow. http://stackoverflow.com/questions/4137986/most-efficient-way-to-search-a-sorted-matrix/4138854#4138854

        a ..... b ..... c
        . .     . .     .
        .   1   .   2   .
        .     . .     . .
        d ..... e ..... f
        . .     . .     .
        .   3   .   4   .
        .     . .     . .
        g ..... h ..... i

        a, c, g < i --> if not, there is no element 

        a, b, d < e
        b, c, e < f
        d, e, g < h
        e, f, h < i
        
        Left Top    :   Right Top
        -----------Mid--------------
        Left Bottom :   Right Bottom


        */
        //Non-Optimal
        public int SearchSortedMatrixInLogNByM(int[,] SortedMatix, int elementToSearch, int rowStPos, int colStPos, int rowEndPos, int colEndPos)
        {
            // Step 0. Check for basic validations.
            if (SortedMatix == null)
            {
                throw new Exception("Matrix is empty");
            }

            // Step 1. Use divide and conquer and to get the middle element.
            int resultNode = 0;
            int rowMidPos = (rowStPos + rowEndPos) / 2;
            int colMidPos = (colStPos + colEndPos) / 2;

            // Step 2. Mid element in Recursive Sub Matrix. For e.g in above example if it is 'e', 'f','h','i' then found.
            if (SortedMatix[rowMidPos, colMidPos] == elementToSearch || SortedMatix[rowMidPos, colEndPos] == elementToSearch ||
                SortedMatix[rowEndPos, colMidPos] == elementToSearch || SortedMatix[rowEndPos, colEndPos] == elementToSearch)
            {
                return elementToSearch;
            }

            // Step 3. Terminate the sub matrix iteration when the element is not found in the 2 X 2 matrix. Because we might checked all the elements above.
            if (rowStPos == rowMidPos || colStPos == colMidPos)
            {
                return 0;
            }

            // Step 4. Left Top Sub Matrix.
            if (resultNode == 0 && elementToSearch < SortedMatix[rowMidPos, colMidPos] && elementToSearch <= SortedMatix[rowMidPos, colStPos] || elementToSearch <= SortedMatix[rowStPos, colMidPos])
            {
                resultNode = SearchSortedMatrixInLogNByM(SortedMatix, elementToSearch, rowStPos, colStPos, rowMidPos, colMidPos);
            }

            // Step 5. Right Top Sub Matrix.
            if (resultNode == 0 && elementToSearch < SortedMatix[rowMidPos, colEndPos])
            {
                resultNode = SearchSortedMatrixInLogNByM(SortedMatix, elementToSearch, rowStPos, colMidPos, rowMidPos, colEndPos);
            }

            // Step 6. Left Bottom Sub Matrix.
            if (resultNode == 0 && elementToSearch < SortedMatix[rowEndPos, colMidPos])
            {
                resultNode = SearchSortedMatrixInLogNByM(SortedMatix, elementToSearch, rowMidPos, colStPos, rowEndPos, colMidPos);
            }

            // Step 7. Right Bottom Sub Matrix.
            if (resultNode == 0 && elementToSearch < SortedMatix[rowEndPos, colEndPos])
            {
                resultNode = SearchSortedMatrixInLogNByM(SortedMatix, elementToSearch, rowMidPos, colMidPos, rowEndPos, colEndPos);
            }

            return resultNode;
        }

        // Matrix Sorted top to bottom in assending order. I.e. Each row starting coulmn value is less than last column value of previous row.
        /*  [01, 03, 05, 07],
            [10, 11, 16, 20],
            [23, 30, 34, 50]         */

        public bool SearchMatrixByBinarySearch(int[,] matrix, int target)
        {
            if (matrix == null || matrix.Length == 0)
                return false;

            if (matrix.Length == 1)
                return matrix[0, 0] == target;

            int stIndx = 0;
            int endIndx = matrix.Length - 1;
            int colLen = matrix.GetLength(1);

            if (target < matrix[0, 0] || target > matrix[matrix.GetLength(0) - 1,colLen - 1])
                return false;

            while (stIndx <= endIndx)
            {
                int midIndx = stIndx + (endIndx - stIndx) / 2;

                int rowIndx = midIndx / colLen;
                int colIndx = midIndx % colLen;

                if (target == matrix[rowIndx, colIndx])
                {
                    return true;
                }
                else if (matrix[rowIndx, colIndx] < target)
                {
                    stIndx = midIndx + 1;
                }
                else
                {
                    endIndx = midIndx - 1;
                }
            }
            return false;
        }

        public void SearchSortedMatrixTest()
        {
            int[,] SortedMatix = {{ 1,    4,      7,      10,     13,},
                                  { 2,    5,      8,      11,     14,},
                                  { 3,    6,      9,      12,     15,},
                                  { 16,   17,     18,     19,     20,},
                                  { 21,   22,     23,     24,     25,},
                                  { 26,   27,     28,     29,     30}};

            //SearchSortedMatrixInNLogN(AssendMatix, 21);

            StringBuilder strBldr = new StringBuilder();

            strBldr.Append("\n 1  : " + SearchSortedMatrixInLogNByM(SortedMatix, 1, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 2  : " + SearchSortedMatrixInLogNByM(SortedMatix, 2, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 3  : " + SearchSortedMatrixInLogNByM(SortedMatix, 3, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 4  : " + SearchSortedMatrixInLogNByM(SortedMatix, 4, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 5  : " + SearchSortedMatrixInLogNByM(SortedMatix, 5, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 6  : " + SearchSortedMatrixInLogNByM(SortedMatix, 6, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 7  : " + SearchSortedMatrixInLogNByM(SortedMatix, 7, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 8  : " + SearchSortedMatrixInLogNByM(SortedMatix, 8, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 9  : " + SearchSortedMatrixInLogNByM(SortedMatix, 9, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 10 : " + SearchSortedMatrixInLogNByM(SortedMatix, 10, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));

            strBldr.Append("\n 11 : " + SearchSortedMatrixInLogNByM(SortedMatix, 11, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 12 : " + SearchSortedMatrixInLogNByM(SortedMatix, 12, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 13 : " + SearchSortedMatrixInLogNByM(SortedMatix, 13, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 14 : " + SearchSortedMatrixInLogNByM(SortedMatix, 14, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 15 : " + SearchSortedMatrixInLogNByM(SortedMatix, 15, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 16 : " + SearchSortedMatrixInLogNByM(SortedMatix, 16, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 17 : " + SearchSortedMatrixInLogNByM(SortedMatix, 17, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 18 : " + SearchSortedMatrixInLogNByM(SortedMatix, 18, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 19 : " + SearchSortedMatrixInLogNByM(SortedMatix, 19, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 20 : " + SearchSortedMatrixInLogNByM(SortedMatix, 20, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));

            strBldr.Append("\n 21 : " + SearchSortedMatrixInLogNByM(SortedMatix, 21, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 22 : " + SearchSortedMatrixInLogNByM(SortedMatix, 22, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 23 : " + SearchSortedMatrixInLogNByM(SortedMatix, 23, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 24 : " + SearchSortedMatrixInLogNByM(SortedMatix, 24, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 25 : " + SearchSortedMatrixInLogNByM(SortedMatix, 25, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 26 : " + SearchSortedMatrixInLogNByM(SortedMatix, 26, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 27 : " + SearchSortedMatrixInLogNByM(SortedMatix, 27, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 28 : " + SearchSortedMatrixInLogNByM(SortedMatix, 28, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 29 : " + SearchSortedMatrixInLogNByM(SortedMatix, 29, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 30 : " + SearchSortedMatrixInLogNByM(SortedMatix, 30, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));

            strBldr.Append("\n\n Example 2:\n");

            SortedMatix = new int[,] {{ 1,    4,      7,},
                                      { 2,    5,      8,},
                                      { 3,    6,      9}};

            strBldr.Append("\n 1  : " + SearchSortedMatrixInLogNByM(SortedMatix, 1, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 2  : " + SearchSortedMatrixInLogNByM(SortedMatix, 2, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 3  : " + SearchSortedMatrixInLogNByM(SortedMatix, 3, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 4  : " + SearchSortedMatrixInLogNByM(SortedMatix, 4, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 5  : " + SearchSortedMatrixInLogNByM(SortedMatix, 5, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 6  : " + SearchSortedMatrixInLogNByM(SortedMatix, 6, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 7  : " + SearchSortedMatrixInLogNByM(SortedMatix, 7, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 8  : " + SearchSortedMatrixInLogNByM(SortedMatix, 8, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 9  : " + SearchSortedMatrixInLogNByM(SortedMatix, 9, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));

            strBldr.Append("\n\n Example 3:\n");
            SortedMatix = new int[,] {{ 1,    3,},
                                      { 2,    4}};

            strBldr.Append("\n 1  : " + SearchSortedMatrixInLogNByM(SortedMatix, 1, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 2  : " + SearchSortedMatrixInLogNByM(SortedMatix, 2, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 3  : " + SearchSortedMatrixInLogNByM(SortedMatix, 3, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 7  : " + SearchSortedMatrixInLogNByM(SortedMatix, 7, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));

            strBldr.Append("\n\n Example 4:\n");
            SortedMatix = new int[,] { { 1, 2 } };
 
            strBldr.Append("\n 1  : " + SearchSortedMatrixInLogNByM(SortedMatix, 1, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 2  : " + SearchSortedMatrixInLogNByM(SortedMatix, 2, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));

            strBldr.Append("\n\n Example 5:\n");
            SortedMatix = new int[,] { { 1 } };

            strBldr.Append("\n 1  : " + SearchSortedMatrixInLogNByM(SortedMatix, 1, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
      
            strBldr.Append("\n\n Example 6:\n");
      
            SortedMatix = new int[,] {{ 1,  2,  4,  10},
                                       {3,  6,  7,  12},
                                       {5,  8,  9,  14},
                                       {11, 13, 15, 16}};

            strBldr.Append("\n 5 : " + SearchSortedMatrixInLogNByM(SortedMatix, 5, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 4  : " + SearchSortedMatrixInLogNByM(SortedMatix, 4, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 9  : " + SearchSortedMatrixInLogNByM(SortedMatix, 9, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 10  : " + SearchSortedMatrixInLogNByM(SortedMatix, 10, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));
            strBldr.Append("\n 11  : " + SearchSortedMatrixInLogNByM(SortedMatix, 11, 0, 0, SortedMatix.GetLength(0) - 1, SortedMatix.GetLength(1) - 1));

            MessageBox.Show("Element(s) Search in Sorted Matrix and the result(s) are " + strBldr.ToString());

        }

        // http://www.geeksforgeeks.org/mean-median-matrix/
        public double FindMean(int[,] matrix)
        {
            int sum = 0;
            int rowLen = matrix.GetLength(0);

            for (int rIndx = 0; rIndx < rowLen; rIndx++)
            {
                for (int cIndx = 0; cIndx < rowLen; cIndx++)
                {
                    sum += matrix[rIndx, cIndx];
                }
            }

            return (double)sum / (rowLen * rowLen);
        }

        // http://www.geeksforgeeks.org/mean-median-matrix/
        public double FindMedian(int[,] matrix)
        {
            int rowLen = matrix.GetLength(0);

            if (rowLen % 2 != 0)
            {
                return matrix[rowLen / 2, rowLen / 2];
            }
            else
            {
                return (matrix[(rowLen - 2) / 2, rowLen - 1] + matrix[rowLen / 2, 0]) / 2.0;
            }
        }

        //LC 378 https://leetcode.com/problems/kth-smallest-element-in-a-sorted-matrix/description/
        //Input:      1   3    5
        //            2   4    7
        //            6   8    9
        //http://stackoverflow.com/questions/19028613/kth-smallest-element-in-a-matrix-with-sorted-rows-and-columns
        //Kth Largest :
        //http://www.dsalgo.com/2013/02/find-kth-largest-element-in-sorted.html
        //http://www.geeksforgeeks.org/forums/topic/kth-smallest-element-in-a-mxn-matrix/

        //// O(n) linear time

        //Binary Search!
        //[ 1,  5,  9],
        //[10, 11, 13],
        //[12, 13, 15]
        //8

        //(1 + 15) / 2 = 8, we are searching where 8th smallest is;
        //We start from 12, 
        //Case 1: 12 > 8, so move to 10, then to 1 (Target is smaller, move to UP!)
        //Case 2: 1 < 8, move to 5 (res += 0 + 1), and move to 9 (res += 1 + 1); (Target is larger, move right, but add i + 1 (add the col!))

        //return 2; 2 < 8, that means our guess is too small!

        public int KthSmallest(int[,] matrix, int k)
        {
            int rowLen = matrix.GetLength(0) - 1;
            int colLen = matrix.GetLength(1) - 1;

            int loIndx = matrix[0, 0];
            int hiIndx = matrix[rowLen, colLen];

            while (loIndx < hiIndx)
            {
                int midIndx = loIndx + (hiIndx - loIndx) / 2;
                int kIndx = 0;
                int colIndx = colLen;

                for (int rowIndx = 0; rowIndx <= rowLen; rowIndx++)
                {
                    while (colIndx >= 0 && matrix[rowIndx, colIndx] > midIndx)
                    {
                        colIndx--;
                    }
                    kIndx += (colIndx + 1);
                }

                if (kIndx < k)
                {
                    loIndx = midIndx + 1;
                }
                else
                {
                    hiIndx = midIndx;
                }
            }

            return loIndx;
        }

        // http://www.geeksforgeeks.org/check-given-matrix-sparse-not/
        // Given a matrix of 0s and 1s find the row that contains maximum number of 1s.
        public void TBD()
        {

        }

        // http://www.geeksforgeeks.org/cholesky-decomposition-matrix-decomposition/

        public void IsStringExistsInMatrixTest()
        {
            char[,] stringMatrix1 = {{'A','C','P','R','C'},
                                    {'X','S','O','P','C'},
                                    {'V','O','V','N','I'},
                                    {'W','G','F','M','N'},
                                    {'Q','A','T','I','T'}};

            char[,] stringMatrix2 = {{'Y','A','A','O'},
                                     {'A','O','A','O'},
                                     {'Y','A','O','O'},
                                     {'H','A','A','O'},
                                     {'Y','A','H','O'}};

            string str2Find1 = "MICROSOFT";
            //string str2Find2 = "YAHOO";

            char[] charArr2Find = str2Find1.ToCharArray();
            bool IsStringExists = IsStringExistsInMatrix(stringMatrix1, charArr2Find, 0, 0, 0);

        }

        //Assume array is 100 X 100. Some logic optimization.
        public bool IsStringExistsInMatrix(char[,] squareMatrix, char[] charArr2Find, int nextCharPos, int xPosition, int yPosition)
        {
            try
            {
                if (squareMatrix[xPosition, yPosition] == 1)
                {
                    return false;
                }
                squareMatrix[xPosition, yPosition] = '2';

                if (xPosition < 99 && squareMatrix[xPosition + 1, yPosition] != 1)
                {
                    IsStringExistsInMatrix(squareMatrix, charArr2Find, 0, xPosition + 1, yPosition);
                }
                if (xPosition > 0 && squareMatrix[xPosition - 1, yPosition] != 1)
                {
                    IsStringExistsInMatrix(squareMatrix, charArr2Find, 0, xPosition - 1, yPosition);
                }
                if (yPosition < 99 && squareMatrix[xPosition, yPosition + 1] != 1)
                {
                    IsStringExistsInMatrix(squareMatrix, charArr2Find, 0, xPosition, yPosition + 1);
                }
                if (yPosition > 0 && squareMatrix[xPosition, yPosition - 1] != 1)
                {
                    IsStringExistsInMatrix(squareMatrix, charArr2Find, 0, xPosition, yPosition - 1);
                }
                return false;
            }
            catch (Exception exception)
            {
                throw new Exception("Something went wrong : " + exception.Message + "\n" + exception.StackTrace.ToString());
            }
        }
    }
}