using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRangeUWP.Models
{
    class Problem
    {
        private List<int>[] table;
        private List<bool> used;
        private int objs;
        private int eles;
        private List<int> Result;
        private List<int> Header;
        /*
        根据要求创建一个二分匹配图对象。
        NumOfObjects: 等待配对的对象个数。
        NumOfElements: 等得被配对的元素个数。
        */
        public Problem(int NumOfObjects, int NumOfElements)
        {
            table = new List<int>[NumOfObjects];
            used = new List<bool>();
            Header = new List<int>();
            Result = new List<int>();
            objs = NumOfObjects;
            eles = NumOfElements;
            for (int i = 0; i < objs; ++i)
            {
                table[i] = new List<int>();
                Header.Add(-1);
            }
            for (int i = 0; i < eles; ++i)
            {
                used.Add(false);
                Result.Add(-1);
            }
        }

        public void NewRelative(int objectIndex, int elementIndex)
        {
            //注意：没有检查重复项
            table[objectIndex].Add(elementIndex);
        }

        public List<int> Solve()
        {
            int founded = 0;
            for (int j = 0; j < eles; ++j)
                Result.Add(-1);
            for (int i = 0; i < objs; ++i)
            {
                for (int j = 0; j < eles; ++j)
                    used[j] = false;
                if (findFree(i))
                    founded++;
            }
            if (founded >= objs)
                return Header;
            else
                throw new ArgumentException("No solution found.");
        }

        private bool findFree(int x)
        {
            for (int j = 0; j < table[x].Count(); ++j)
            {
                int y = table[x][j];
                if (!used[y])
                {
                    used[y] = true;
                    if (Result[y] == -1 || findFree(Result[y]))
                    {
                        Result[y] = x;
                        Header[x] = y;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
