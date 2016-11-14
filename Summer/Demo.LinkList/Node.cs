using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.LinkList
{
    public class Node<T>
    {
        private T data;//数据域,当前结点的数据  
        private Node<T> next;//引用域,即下一结点  

        //构造器：数据域+引用域，普通结点  
        public Node(T item, Node<T> p)
        {
            this.data = item;
            this.next = p;
        }

        //构造器：引用域，头结点  
        public Node(Node<T> p)
        {
            this.next = p;
        }

        //构造器：数据域，尾节点
        public Node(T item)
        {
            this.data = item;
            this.next = null;
        }

        //构造器：无参数  
        public Node()
        {
            data = default(T);
            next = null;
        }

        //数据域属性
        public T Data
        {
            get { return data; }
            set { this.data = value; }
        }

        //引用域属性
        public Node<T> Next
        {
            get { return this.next; }
            set { this.next = value; }
        }
    }
}
