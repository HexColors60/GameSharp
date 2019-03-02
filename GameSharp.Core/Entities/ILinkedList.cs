using System;
using System.Collections.Generic;

namespace GameSharp.Core.Entities
{
    public abstract class WriteLinkedListEntity<TValue> : EventArgs,
        IWriteLinkedListEntity<TValue>
        where TValue : INode<TValue>
    {
        protected readonly LinkedList<TValue> LinkedList = new LinkedList<TValue>();

        private void Add(TValue value)
        {
            if (LinkedList.Count <= 0)
            {
                LinkedList.AddFirst(value);
                LinkedList.AddLast(value);
                return;
            }

            LinkedList.Last.Value.Next = value; // Save the reference to the next element in the Db
            LinkedList.AddAfter(LinkedList.Last, value);
            LinkedList.AddLast(value);
        }

        public void AddRange(IEnumerable<TValue> values)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }
    }

    public interface INode<TEntity>
    {
        TEntity Next { get; set; }
    }

    internal interface IEnumeratorEntity<out TValue>
        where TValue : INode<TValue>
    {
        TValue CurrentEntity { get; }
        bool Next();
    }

    internal interface IWriteLinkedListEntity<in TValue>
        where TValue : INode<TValue>
    {
        void AddRange(IEnumerable<TValue> values);
    }
}