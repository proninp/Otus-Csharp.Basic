using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork08
{
    public class BinaryTree
    {
        private TreeNode? _node;
        public int Count { get; private set; } = 0;
        public void Insert(Employee value)
        {
            if (_node == null)
                _node = new TreeNode(value);
            else
                _node.Insert(value);
            Count++;
        }
        public TreeNode? Find(int value)
        {
            if (_node == null)
                return null;
            return _node.Find(value);
        }
        public TreeNode? FindRecoursive(int value)
        {
            if (_node == null)
                return null;
            return _node.FindRecoursive(value);
        }
        public void InOrderTraversal()
        {
            if (_node != null)
                _node.InOrderTraversal();
        }
        public void PreOrderTraversal()
        {
            if (_node != null)
                _node.PreOrderTraversal();
        }
        public void PostOrderTraversal()
        {
            if (_node != null)
                _node.PostOrderTraversal();
        }
    }
}
