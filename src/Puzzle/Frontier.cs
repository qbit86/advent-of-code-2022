using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AdventOfCode2022;

internal sealed class Frontier<TNode> : IProducerConsumerCollection<TNode>
{
    private readonly PriorityQueue<TNode, TNode> _priorityQueue;

    private Frontier(PriorityQueue<TNode, TNode> priorityQueue) => _priorityQueue = priorityQueue;

    internal Frontier(IComparer<TNode> comparer) : this(new PriorityQueue<TNode, TNode>(comparer)) { }

    public IEnumerator<TNode> GetEnumerator() => throw new NotSupportedException();

    IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();

    public void CopyTo(Array array, int index) => throw new NotSupportedException();

    public int Count => _priorityQueue.Count;

    public bool IsSynchronized => throw new NotSupportedException();

    public object SyncRoot => throw new NotSupportedException();

    public void CopyTo(TNode[] array, int index) => throw new NotSupportedException();

    public TNode[] ToArray() => throw new NotSupportedException();

    public bool TryAdd(TNode item)
    {
        _priorityQueue.Enqueue(item, item);
        return true;
    }

    public bool TryTake(out TNode item) => _priorityQueue.TryDequeue(out item!, out _);
}
