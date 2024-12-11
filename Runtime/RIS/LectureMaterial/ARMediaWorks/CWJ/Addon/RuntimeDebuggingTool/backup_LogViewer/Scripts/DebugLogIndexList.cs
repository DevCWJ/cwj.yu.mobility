namespace CWJ.RuntimeDebugging
{
    public class DebugLogIndexList
    {
        private int[] indices;

        public int Count { get; private set; }
        public int this[int index] { get { return indices[index]; } }

        public DebugLogIndexList()
        {
            indices = new int[64];
            Count = 0;
        }

        public void Add(int index)
        {
            if (Count == indices.Length)
            {
                int[] indicesNew = new int[Count * 2];
                System.Array.Copy(indices, 0, indicesNew, 0, Count);
                indices = indicesNew;
            }

            indices[Count++] = index;
        }

        public void Clear()
        {
            Count = 0;
        }
    }
}