[System.Serializable]
public struct ItemBundle
{
    public Item item;
    public int qty;

    public SerializedBundle Serialized()
    {
        return new SerializedBundle()
        {
            item = ItemCollection.Instance.IndexOf(item),
            qty = qty,
        };
    }

    static public ItemBundle Create(SerializedBundle serializedBundle)
    {
        return new ItemBundle()
        {
            item = ItemCollection.Instance.GetItem(serializedBundle.item),
            qty = serializedBundle.qty,
        };
    }

    [System.Serializable]
    public struct SerializedBundle
    {
        public int item;
        public int qty;
    }
}
