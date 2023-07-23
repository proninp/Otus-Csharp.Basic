using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork08
{
    public class TreeNode
    {
        public Employee Data { get; set; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }
        
        public TreeNode(Employee employee)
        {
            Data = employee;
        }
        public void Insert(Employee employee)
        {
            if (Data > employee)
            {
                if (Left == null)
                    Left = new TreeNode(employee);
                else
                    Left.Insert(employee);
            }
            else
            {
                if (Right == null)
                    Right = new TreeNode(employee);
                else
                    Right.Insert(employee);
            }
        }
        public TreeNode? Find(int value)
        {
            TreeNode node = this;
            while(node != null)
            {
                if (node.Data == value)
                    return node;
                if (node.Data >= value)
                    node = node.Left;
                else
                    node = node.Right;
            }
            return null;
        }
        public TreeNode? FindRecoursive(int value)
        {
            if (Data == value)
                return this;
            if (Data >= value && Left != null)
                return Left.FindRecoursive(value);
            if (Data < value && Right != null)
                return Right.FindRecoursive(value);
            return null;
        }
        public void InOrderTraversal()
        {
            Left?.InOrderTraversal();
            Console.WriteLine(Data);
            Right?.InOrderTraversal();
        }
        public void PreOrderTraversal()
        {
            Console.WriteLine(Data);
            Left?.InOrderTraversal();
            Right?.InOrderTraversal();
        }
        public void PostOrderTraversal()
        {
            Left?.InOrderTraversal();
            Right?.InOrderTraversal();
            Console.WriteLine(Data);
        }

    }
}
