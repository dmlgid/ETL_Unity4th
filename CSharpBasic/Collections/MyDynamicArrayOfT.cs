using System.Collections;

namespace Collections
{
    internal class MyDynamicArray<T> : IEnumerable<T>
        where T : IComparable<T> 
        //  where 제한자 : 타입을 제한하는 한정자 (T 에 넣을 타입은 IComparable<T> 로 공변 가능해야한다)
    {
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                _items[index] = value;
            }
        }

        //public int Count
        //{
        //    get
        //    {
        //        return _count;
        //    }
        //}

        public int Count => _count;
        public int Capacity => _items.Length;

        private int _count;
        private const int DEFAULT_SIZE = 1;
        private T[] _items = new T[DEFAULT_SIZE];

        public void Add(T item)
        {
            if (_count >= _items.Length)
            {
                T[] tmp = new T[_count * 2];
                Array.Copy(_items, tmp, _count);
                _items = tmp;
            }

            _items[_count++] = item;
        }

        public T Find(Predicate<T> match)
        {
            for (int i = 0; i < _count; i++)
            {
                if (match(_items[i]))
                    return _items[i];
            }
            return default;
        }

        public int FindIndex(Predicate<T> match)
        {
            for (int i = 0; i < _count; i++)
            {
                if (match(_items[i]))
                    return i;
            }
            return -1;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                // Default 비교연산 (C# 기본제공 비교연산자 쓸 때)
                if (Comparer<T>.Default.Compare(_items[i], item) == 0)
                    return true;

                // IComparable 비교연산.. (내가 비교연산내용을 직접 구현해서 쓸때)
                if (item.CompareTo(_items[i]) == 0)
                    return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _count--;
        }

        public bool Remove(T item)
        {
            int index = FindIndex(x => item.CompareTo(x) == 0);

            // 지우려는 대상 못찾으면 false 반환
            if (index < 0)
                return false;

            RemoveAt(index);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        // 책읽어주는자
        public struct Enumerator : IEnumerator<T>
        {
            // 현재 페이지 내용 읽기
            public T Current => _data[_index];

            object IEnumerator.Current => _data[_index];

            private MyDynamicArray<T> _data; // 책
            private int _index; // 책의 현재 페이지

            public Enumerator(MyDynamicArray<T> data)
            {
                _data = data;
                _index = -1; // 책 표지 덮은 상태로 시작
            }

            // 책읽을때 필요했던 자원들(리소스) 을 메모리에서 해제하는 내용을 구현하는 부분
            public void Dispose()
            {
            }

            // 다음 페이지로
            public bool MoveNext()
            {
                // 넘길 수 있는 다음장이 존재한다면 다음장으로 넘기고 true 반환
                if (_index < _data._count - 1)
                {
                    _index++;
                    return true;
                }

                return false;
            }

            // 책 덮기
            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
