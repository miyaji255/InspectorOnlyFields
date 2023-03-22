using InspectorOnlyFields;
using UnityEngine;

namespace ConsoleApp
{
    [Serializable]
    internal class Program
    {
        public int NormalField = 0;

        [InspectorOnly]
        public int GoodField1;
        [SerializeField, InspectorOnly]
        internal int GoodField2;
        [SerializeField, InspectorOnly]
        private int GoodField3;
        [InspectorOnly]
        public int BadField1 = 0;
        [SerializeField, InspectorOnly]
        internal int BadField2 = 0;
        [InspectorOnly]
        private int BadField3;

        [field: SerializeField, InspectorOnly]
        public int GoodProperty1 { get; set; }
        [field: SerializeField, InspectorOnly]
        internal int GoodProperty2 { get; set; }
        [field: SerializeField, InspectorOnly]
        private int GoodProperty3 { get; set; }
        [field: InspectorOnly]
        public int BadProperty1 { get; set; }
        [field: SerializeField, InspectorOnly]
        internal int BadProperty2 { get; set; } = 0;
        [field: InspectorOnly]
        private int BadProperty3 { get; set; }

        static void Main(string[] args)
        {
            var p = new Program();

            // bad example
            p.GoodField1 = 1;
            p.GoodField2 = 1;
            p.GoodField3 = 1;

            p.GoodProperty1 = 1;
            p.GoodProperty2 = 1;
            p.GoodProperty3 = 1;

            (p.GoodField1, p.GoodField2) = (2, 3);
            (p.GoodField1, (p.GoodField2, p.GoodField3, (p.GoodProperty1, p.GoodProperty2))) = (1, (2, 3, (4, 5)));
        }
    }
}
