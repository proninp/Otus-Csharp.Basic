namespace PrimsAlgorithm
{
    public class Program
    {
        static void Main(string[] args)
        {
            var points = new int[][] 
            { 
                new int[] { 0, 0 },
                new int[] { 2, 2 },
                new int[] { 3, 10 },
                new int[] { 5, 2 },
                new int[] { 7, 0 } 
            };
            var minDistancePoints = MinDistanceConnectPoints(points);
            Console.WriteLine(string.Join(" ", minDistancePoints.Select(p => $"[{p[0]}, {p[1]}]")));
        }
        public static List<int[]> MinDistanceConnectPoints(int[][] points)
        {
            var n = points.Length;
            var dictionary = new Dictionary<int, List<Tuple<int, int>>>();
            var output = new List<int[]>(n);

            if (n == 1)
                return new List<int[]>() { points[0] };

            for (var i = 0; i < n; i++)
                dictionary.Add(i, new List<Tuple<int, int>>()); // В Tuple - расстояние, индекс

            // Для каждой точки собираем смежные точки и считаем расстояние
            for (var i = 0; i < n; i++)
            {
                var x1 = points[i][0];
                var y1 = points[i][1];

                for (var j = i + 1; j < n; j++)
                {
                    var x2 = points[j][0];
                    var y2 = points[j][1];

                    var dist = Math.Abs(x2 - x1) + Math.Abs(y1 - y2);

                    dictionary[j].Add(new Tuple<int, int>(dist, i));
                    dictionary[i].Add(new Tuple<int, int>(dist, j));
                }
            }

            // Алгоритм Прима
            var visited = new HashSet<int>();
            var minHeap = new PriorityQueue<(int, int), int>();
            minHeap.Enqueue((0, 0), 0);
            
            while (minHeap.Count > 0)
            {
                var (dist, point) = minHeap.Dequeue();
                if (visited.Contains(point))
                    continue;
                output.Add(points[point]);
                visited.Add(point);
                var adj = dictionary[point];

                for (var i = 0; i < adj.Count; i++)
                {
                    if (!visited.Contains(adj[i].Item2))
                        minHeap.Enqueue((adj[i].Item1, adj[i].Item2), adj[i].Item1);
                }
            }
            return output;
        }
    }
}