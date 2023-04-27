using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    List<float> keys;
    List<T> values;
    public int Count { get { return keys.Count; } }

    public PriorityQueue()
    {
        keys = new List<float>();
        values = new List<T>();
    }

    public PriorityQueue(int count)
    {
        keys = new List<float>(count);
        values = new List<T>(count);
    }


    public void Enqueue(float key, T value)
    {
        keys.Add(key);
        values.Add(value);
        int i = Count - 1;

        while (i > 0)
        {
            int p = (i - 1) / 2;
            if (keys[p] <= key) break;

            keys[i] = keys[p];
            values[i] = values[p];
            i = p;
        }

        if (Count > 0)
        {
            keys[i] = key;
            values[i] = value;
        }
    }

    public T Dequeue()
    {
        float min = keys[0];
        T value = values[0];
        float root = keys[Count - 1];
        T rootValue = values[Count - 1];
        keys.RemoveAt(Count - 1);
        values.RemoveAt(Count);

        int i = 0;
        while (i * 2 + 1 < Count)
        {
            int a = i * 2 + 1;
            int b = i * 2 + 2;
            int c = b < Count && keys[b] < keys[a] ? b : a;

            if (keys[c] >= root) break;
            keys[i] = keys[c];
            values[i] = values[c];
            i = c;
        }

        if (Count > 0)
        {
            keys[i] = root;
            values[i] = rootValue;
        }
        return value;
    }

    public T Peek()
    {
        if (Count == 0) throw new InvalidOperationException("Queue is empty.");
        return values[0];
    }

    public void Clear()
    {
        keys.Clear();
        values.Clear();
    }

    public bool ContainsValue(T value) => values.Contains(value);

}
