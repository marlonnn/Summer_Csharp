using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.LinkList
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MyLinkList<string> myLinkList = new MyLinkList<string>(); //实例化一个单链表  
                Console.WriteLine(myLinkList.GetLength());   //获取长度  

                //添加元素  
                myLinkList.Append("good");
                myLinkList.Append("monring");
                myLinkList.Append("lwk");
                myLinkList.Display();  //显示链表元素  
                MyLinkList<string> newLinkList = myLinkList.Reverse(myLinkList);
                //Console.WriteLine(); //显示链表长度 
                newLinkList.Display();
                Console.Read();
            }
            catch (Exception ee)
            {


            }
        }
    }
}
