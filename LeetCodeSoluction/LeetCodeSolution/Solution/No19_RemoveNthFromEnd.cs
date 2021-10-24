using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSoluction.Solution
{
    public class No19_RemoveNthFromEnd : BaseSolution
    {
        public override object GetResult()
        {
            //return new Solution().RemoveNthFromEnd("237");
            return null;
        }

        //Definition for singly-linked list.
        public class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int val = 0, ListNode next = null)
            {
                this.val = val;
                this.next = next;
            }
        }

        public class Solution
        {
            public ListNode RemoveNthFromEnd(ListNode head, int n)
            {
                return new ListNode();
            }
        }
    }
}
