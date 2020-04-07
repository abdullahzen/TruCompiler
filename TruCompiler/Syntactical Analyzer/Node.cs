using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TruCompiler.Semantic_Analyzer;
using TruCompiler.Semantic_Analyzer.SymbolTableClasses;
using static TruCompiler.Lexical_Analyzer.Tokens;

namespace TruCompiler.Syntactical_Analyzer
{
    public class Node<T>
    {
        private T _value;
        private List<Node<T>> _children = new List<Node<T>>();
        private Node<T> _parent;
        public SymbolTable SymbolTable { get; set; }
        public Entry Entry { get; set; }
        public string TempVarName { get; set; }
        public string Type { get; set; }
        public Node()
        {
        }
        public Node(T value)
        {
            _value = value;
        }

        public Node(Node<T> subtree)
        {
            subtree.Parent = this;
            AddChild(subtree);
        }

        public Node(Node<T> subtree, Node<T> parent, T value)
        {
            subtree.Parent = this;
            AddChild(subtree);
            this.Parent = parent;
            this.Value = value;
        }

        protected Node(Node<T> parent, Node<T> current)
        {
            this.Parent = parent;
            this.Value = current.Value;
        }

        public Node<T> this[int i]
        {
            get { return _children[i]; }
        }

        public Node<T> Parent { get { return _parent; } set { _parent = value; } }

        public T Value { get { return _value; } set { _value = value; } }

        public List<Node<T>> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public Node<T> AddChild(T value, bool returnChild = false)
        {
            var node = new Node<T>(value) { Parent = this };
            _children.Add(node);
            if (returnChild)
            {
                return node;
            } else
            {
                return this;
            }
        }

        public Node<T> AddChild(Node<T> node, bool returnChild)
        {
            node.Parent = this;
            _children.Add(node);
            if (returnChild)
            {
                return node;
            }
            else
            {
                return this;
            }
        }

        public Node<T> AddChild(Node<T> node)
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

        public List<Node<T>> AddChildren(List<Node<T>> values)
        {
            Children.AddRange(values);
            return Children;
        }

        public bool RemoveChild(Node<T> node)
        {
            return _children.Remove(node);
        }

        public void RemoveAt(int num)
        {
            _children.RemoveAt(num);
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

        public virtual void accept(Visitor<T> visitor)
        {
            visitor.visit(this);
        }
    }
}
