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
            Console.WriteLine(string.Join("", minDistancePoints.Skip(1).Select(p => $"[{p.Item1[0]}, {p.Item1[1]}] -> [{p.Item2[0]}, {p.Item2[1]}]\n")));
        }
        public static List<(int[], int[])> MinDistanceConnectPoints(int[][] points)
        {
            var n = points.Length;
            var dictionary = new Dictionary<int, List<Tuple<int, Tuple<int, int>>>>();
            var output = new List<(int[], int[])>(n);

            if (n == 1)
                return new List<(int[], int[])>() { (points[0], points[0]) };

            for (var i = 0; i < n; i++)
                dictionary.Add(i, new List<Tuple<int, Tuple<int, int>>>()); // В Tuple - расстояние, индекс

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

                    dictionary[j].Add(Tuple.Create(dist, Tuple.Create(i, j)));
                    dictionary[i].Add(Tuple.Create(dist, Tuple.Create(j, i)));
                }
            }

            // Алгоритм Прима
            var visited = new HashSet<int>();
            var minHeap = new PriorityQueue<(int, Tuple<int, int>), int>();
            var prevPoint = 0;
            minHeap.Enqueue((0, Tuple.Create(0, prevPoint)), 0);
            
            while (minHeap.Count > 0)
            {
                var pointsPair = minHeap.Dequeue().Item2;
                if (visited.Contains(pointsPair.Item1))
                    continue;
                prevPoint = pointsPair.Item2;
                output.Add((points[prevPoint], points[pointsPair.Item1]));
                visited.Add(pointsPair.Item1);
                if (visited.Count == n)
                    break;
                var adj = dictionary[pointsPair.Item1];

                for (var i = 0; i < adj.Count; i++)
                {
                    if (!visited.Contains(adj[i].Item2.Item1))
                        minHeap.Enqueue((adj[i].Item1, adj[i].Item2), adj[i].Item1);
                }
            }
            return output;
        }
    }
}