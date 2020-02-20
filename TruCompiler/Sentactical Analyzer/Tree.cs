using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Sentactical_Analyzer
{
    public class TreeNode<T>
    {
        private T _value;
        private List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public TreeNode()
        {
        }
        public TreeNode(T value)
        {
            _value = value;
        }

        public TreeNode(TreeNode<T> subtree)
        {
            subtree.Parent = this;
            AddChild(subtree);
        }

        public TreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }

        public TreeNode<T> Parent { get; private set; }

        public T Value { get { return _value; } set { _value = value; } }

        public List<TreeNode<T>> Children
        {
            get { return _children; }
        }

        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) { Parent = this };
            _children.Add(node);
            return this;
        }

        public TreeNode<T> AddChild(TreeNode<T> node)
        {
            if (node.Value == null && node.Children.Count > 0)
            {
                foreach(var subtree in node.Children)
                {
                    subtree.Parent = this;
                    this.AddChild(subtree);
                }
                return this;
            } else
            {
                node.Parent = this;
                _children.Add(node);
                return this;
            }
        }

        public List<TreeNode<T>> AddChildren(List<TreeNode<T>> values)
        {
            Children.AddRange(values);
            return Children;
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            return _children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
        }
    }
}
