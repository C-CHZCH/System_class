namespace ConsoleApp;

public class LFUCache
{
    //双哈希表以达到O(1)
    private readonly Dictionary<int, LinkedList<Item>> _values; //频率作为key，每个频率下维护一个链表

    private readonly Dictionary<int, LinkedListNode<Item>> _nodeTracker; //存放节点

    private readonly int _capacity;

    private int _minFreq; //整个表中最小的频率

    private int _time;

    public LFUCache(int capacity)
    {
        _capacity = capacity;
        _values = new Dictionary<int, LinkedList<Item>>();
        _nodeTracker = new Dictionary<int, LinkedListNode<Item>>();
        _values.Add(1, new LinkedList<Item>());
    }

    public void Process(string nums)
    {
        if (nums == null) throw new ArgumentNullException(nameof(nums));
        int index = 0;
        for (; index < nums.Length; index++)
        {
            Put(nums[index] - '0',nums[index] - '0');
        }
        Print();
    }
    private void PromoteNode(LinkedListNode<Item> node) //增加节点的使用频率，将此节点移动到属于更高频率的链表中去
    {
        int currFreq = node.Value.Freq;
        int newFreq = currFreq + 1;

        _values[currFreq].Remove(node);
        if (_minFreq == currFreq && _values[currFreq].Count == 0)
        {
            _minFreq += 1;
        }

        if (!_values.ContainsKey(newFreq)) //若不存在此频率
        {
            _values.Add(newFreq, new LinkedList<Item>());
        }

        node.ValueRef.Freq = newFreq;
        _values[newFreq].AddLast(node);
    }

    private Item CreateItem(int key, int value, int freq) //作节点
    {
        Item item = new Item();
        item.Key = key;
        item.Value = value;
        item.Freq = freq;
        return item;
    }

    public int Get(int key) //使用一次此节点
    {
        if (_nodeTracker.ContainsKey(key))
        {
            PromoteNode(_nodeTracker[key]);
            return _nodeTracker[key].Value.Value;
        }
        else
        {
            return -1;
        }
    }

    public void Put(int key, int value)
    {
        if (_capacity == 0)
        {
            return;
        }

        LinkedListNode<Item> node;
        if (!_nodeTracker.ContainsKey(key))//若此节点之前并未被插入
        {
            if (_nodeTracker.Count == _capacity)
            {
                Console.WriteLine($"发生缺页且当前表的容量已满，淘汰{_values[_minFreq].First.Value.Key}");
                _nodeTracker.Remove(_values[_minFreq].First.Value.Key);//移除最少使用的节点
                _values[_minFreq].RemoveFirst();//更新最小频率对应的链表
                _time++;
            }

            node = new LinkedListNode<Item>(CreateItem(key, value, 1));
            _nodeTracker.Add(key, node);
            _values[1].AddLast(node);
            _minFreq = 1;
        }
        else
        {
            Console.WriteLine($"该页面仍存在nodeTracker中，只需对其进行频率加一操作");
            node = _nodeTracker[key];
            node.ValueRef.Value = value;
            PromoteNode(node);
        }
    }

    public void Print()
    {
        Console.WriteLine($"总缺页次数为{_time}");
    }
}

class Item
{
    public int Key;
    public int Value;
    public int Freq;
}