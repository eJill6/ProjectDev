using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSolution.Solution
{
    public class No19_RemoveNthFromEnd : BaseSolution
    {
        public override object GetResult()
        {
            ListNode head = new ListNode(1,
                new ListNode(2,
                new ListNode(3,
                new ListNode(4,
                new ListNode(5, null)))));

            return new Solution().RemoveNthFromEnd(head, 2);
            //return null;
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
                var nodes = new List<ListNode>();
                ListNode addnode = head;

                while (addnode != null)
                {
                    nodes.Add(addnode);
                    addnode = addnode.next;
                }

                int removeIndex = nodes.Count - n;

                if (removeIndex == 0)
                {
                    return head.next;
                }

                nodes[removeIndex - 1].next = nodes[removeIndex].next;

                return head;
            }

                //public ListNode RemoveNthFromEnd(ListNode head, int n)
                //{
                //    ListNode first = new ListNode();
                //    ListNode current = first;

                //    current.next = head;

                //    for (int i = 0; i < n; i++)
                //    {
                //        head = head.next;
                //    }

                //    while (head != null)
                //    {
                //        head = head.next;
                //        current = current.next;
                //    }

                //    current.next = current.next.next;

                //    return first.next;
                //}
            }
    }
}
