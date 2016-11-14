using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.LinkList
{
    public class MyLinkList<T>
    {
        private Node<T> head;//头节点

        public Node<T> Head
        {
            get { return this.head; }
            set { this.head = value; }
        }

        public MyLinkList()
        {
            this.head = null;
        }

        //获取单链表长度
        public int GetLength()
        {
            Node<T> p = head;
            int len = 0;
            while (p != null)
            {
                len++;
                p = p.Next;
            }
            return len = 0;
        }

        //清空单链表
        public void Clear()
        {
            this.head = null;
        }

        //判断单链表是否为空
        public bool IsEmpty()
        {
            return this.head == null;
        }

        //在单链表末尾添加新元素
        //public void Append(T item)
        //{
        //    Node<T> q = new Node<T>(item);
        //    Node<T> p = new Node<T>();
        //    if (head == null)
        //    {
        //        head = q;
        //        return;
        //    }
        //    p = head;
        //    while(p != null)
        //    {
        //        p = p.Next;
        //    }
        //    p.Next = q;
        //}

        //在单链表的末尾添加新元素  
        public void Append(T item)
        {
            Node<T> q = new Node<T>(item);
            Node<T> p = new Node<T>();
            if (head == null)
            {
                head = q;
                return;
            }
            p = head;
            while (p.Next != null)
            {
                p = p.Next;
            }
            p.Next = q;
        }

        //在单链表的第i个结点的位置前插入一个值为item的结点  
        public void Insert(T item, int position)
        {
            if (IsEmpty() || position < 1 || position > GetLength())
            {
                Console.WriteLine("LinkList is empty or position is error");
                return;
            }
            if (position == 1)
            {
                Node<T> q = new Node<T>(item);
                q.Next = this.head;
                this.head = q;
                return;
            }
            Node<T> p = head;
            Node<T> r = new Node<T>();
            int j = 1;
            while (p.Next != null && j < position)
            {
                r = p;
                p = p.Next;
                ++j;
            }
            if (j == position)
            {
                Node<T> q = new Node<T>(item);
                q.Next = p;
                r.Next = q;
            }
        }

        //获得单链表的第i个数据元素  
        public T GetElem(int i)
        {
            if (IsEmpty() || i < 0)
            {
                Console.WriteLine("LinkList is empty or position is error! ");
                return default(T);
            }
            Node<T> p = new Node<T>();
            p = head;
            int j = 1;
            while (p.Next != null && j < i)
            {

                ++j;
                p = p.Next;
            }
            if (j == i)
            {
                return p.Data;
            }
            else
            {
                Console.WriteLine("The " + i + "th node is not exist!");
                return default(T);
            }
        }

        public MyLinkList<T> Reverse(MyLinkList<T> link)
        {
            MyLinkList<T> newList = new MyLinkList<T>();
            Node<T> temp;
            if (link == null)
            {
                return null;
            }
            newList = link;
            newList.Head = link.Head;
            //newList.Head.Next = null;

            while (link.Head.Next != null)
            {
                //tmp = newList->next;         //保存newList中的后续结点
                //newList->next = list->next;       //将list的第一个结点放到newList中
                //list->next = list->next->next;     //从list中摘除这个结点
                //newList->next->next = tmp;        //恢复newList中后续结点的指针
                temp = link.head.Next;
                newList.Head.Next = link.Head.Next;
                link.Head.Next = link.Head.Next.Next;
                newList.Head.Next.Next = temp;
            }

            return newList;
        }

        //显示链表  
        public void Display()
        {
            Node<T> p = new Node<T>();
            p = this.head;
            while (p != null)
            {
                Console.Write(p.Data + " ");
                p = p.Next;
            }
        }
    }
}
