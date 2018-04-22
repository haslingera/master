using System.Collections;
using System.Collections.Generic;

public class SlidingBuffer<T> : IEnumerable<T> {

	private readonly Queue<T> _queue;
	private int _maxCount;
	
	public int Count
	{
		get { return _queue.Count; }
	}
	
	public SlidingBuffer(int maxCount)
	{
		_maxCount = maxCount;
		_queue = new Queue<T>();
	}

	public void Clear()
	{
		_queue.Clear();
	}
	
	public void Push(T item)
	{
		if (_maxCount != -1 && _queue.Count == _maxCount)
		{
			_queue.Dequeue();
		}
		_queue.Enqueue(item);
	}
	
	public T Dequeue()
	{
		return _queue.Dequeue();
	}

	public T Peek()
	{
		return _queue.Peek();
	}

	public IEnumerator<T> GetEnumerator()
	{
		return _queue.GetEnumerator();
	}

	public void SetMaxCount(int maxCount)
	{
		_maxCount = maxCount;
	}

	public int GetMaxCount()
	{
		return _maxCount;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	
}
