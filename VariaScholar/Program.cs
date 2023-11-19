#define FirstTask
#define SecondTask
#define ThirdTask
#define FourthTask

#if FirstTask
Console.WriteLine("First Task:");
Console.WriteLine("=================================================");
Console.WriteLine(SecToString(3600));
Console.WriteLine(SecToString(64));
Console.WriteLine("=================================================");
Console.WriteLine(Environment.NewLine);
#endif

#if SecondTask
Console.WriteLine("Second Task:");
Console.WriteLine("=================================================");
var knightPosition = "a1";
Console.WriteLine($@"Knight possible moves from ""{knightPosition}"": {PossibleMoves(knightPosition)}");
Console.WriteLine("=================================================");
Console.WriteLine(Environment.NewLine);
#endif

#if ThirdTask
Console.WriteLine("Third Task:");
Console.WriteLine("=================================================");
var customList = new NaiveList();
customList.add(new string("e0-head"));
customList.add(new string("e1"));
customList.add(new string("e3"));
customList.insertAdd(new string("e2"), 2);
customList.add(new string("e4"));
customList.add(new object());
customList.add(13);
customList.add(false);
customList.add(new string("e8-tail"));
customList.print();
Console.WriteLine();
Console.WriteLine(string.Format("First list element: {0}", customList.elementAt(0)));
Console.WriteLine(string.Format("Ninth list element: {0}", customList.elementAt(8)));
Console.WriteLine($"{Environment.NewLine}Clearing...{Environment.NewLine}");
customList.clear();
Console.WriteLine($"List is empty: {customList.isEmpty()}");
customList.add(new string("i0-head"));
customList.print();
Console.WriteLine($"List is empty: {customList.isEmpty()}");
Console.WriteLine("=================================================");
Console.WriteLine(Environment.NewLine);
#endif

#if FourthTask
Console.WriteLine("Fourth Task:");
Console.WriteLine("=================================================");
var dataMesh =
    """
    00000001
    00011101
    10011000
    11010000
    10011100
    00000011
    """;
Console.WriteLine("Larges island: " + Columbuse(dataMesh));
Console.WriteLine("=================================================");
Console.WriteLine(Environment.NewLine);
#endif

#if FirstTask
static string SecToString(int secInt)
{
    const int startThreshold = 0;
    const int endThreshold = 86399;
    if (secInt < startThreshold || secInt > endThreshold)
    {
        throw new InvalidDataException($"Range should be between {startThreshold} and {endThreshold}");
    }
    var timeSpan = TimeSpan.FromSeconds(secInt);
    return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
}
#endif

#if SecondTask
static int PossibleMoves(string position)
{
    if (position.Length != 2)
    {
        throw new InvalidDataException($"Coordinates should have two elements: one letter and one digit. Input is invalid: {position}");
    }

    var xChar = position.ToCharArray()[0];
    if (!char.IsLetter(xChar))
    {
        throw new InvalidDataException($"First coordinate should be a letter. Input is invalid: {xChar}");
    }

    var yChar = position.ToCharArray()[1];
    if (!char.IsDigit(yChar))
    {
        throw new InvalidDataException($"Second coordinate should be a digit. Input is invalid: {yChar}");
    }

    var yPosition = (int)char.GetNumericValue(yChar);
    if (yPosition < 1 && yPosition > 8)
    {
        throw new InvalidDataException($"Second coordinate should be a digit beetwen '1' and '8'. Input is invalid: {yChar}");
    }

    yPosition -= 1;

    var xPosition = xChar switch
    {
        'a' or 'A' => 0,
        'b' or 'B' => 1,
        'c' or 'C' => 2,
        'd' or 'D' => 3,
        'e' or 'E' => 4,
        'f' or 'F' => 5,
        'g' or 'G' => 6,
        'h' or 'H' => 7,
        _ => throw new InvalidDataException($"First coordinate should be a letter beetwen 'a' and 'h' or between 'A' and 'H'. Input is invalid: {xChar}"),
    };

    var hooperCoordinates = new[] { xPosition, yPosition };

    var oneHourMove = new int[] { 1, 2 };
    var twoHourMove = new int[] { 2, 1 };
    var fourHourMove = new int[] { 2, -1 };
    var fiveHourMove = new int[] { 1, -2 };
    var sevenHourMove = new int[] { -1, -2 };
    var eightHourMove = new int[] { -2, -1 };
    var tenHourMove = new int[] { -2, 1 };
    var elevenHourMove = new int[] { -1, 2 };

    var hooperClockFace = new int[][]
    {
        oneHourMove, twoHourMove,
        fourHourMove, fiveHourMove,
        sevenHourMove, eightHourMove,
        tenHourMove, elevenHourMove
    };

    var totalMoves = 0;
    foreach (var hourMove in hooperClockFace)
    {
        var coordinateX = hooperCoordinates[0] + hourMove[0];
        var coordinateY = hooperCoordinates[1] + hourMove[1];

        if (coordinateX >= 0 && coordinateX <= 7 && coordinateY >= 0 && coordinateY <= 7)
        {
            totalMoves++;
        }
    }

    return totalMoves;
}
#endif

#if FourthTask
int Columbuse(string dataMesh)
{
    bool[][] logicMesh = CreateLogicMesh(dataMesh);

    int topDog = 0;
    var islands = new Dictionary<int[], List<Terrain>>();
    var isFreeDrifting = false;
    do
    {
        isFreeDrifting = false;
        var isEmbarked = false;
        for (int yIndex = 0; yIndex < logicMesh.Length && !isEmbarked; yIndex++)
        {
            for (int xIndex = 0; xIndex < logicMesh[yIndex].Length && !isEmbarked; xIndex++)
            {
                if (logicMesh[yIndex][xIndex])
                {
                    var locationKey = new[] { yIndex, xIndex };
                    islands.Add(locationKey, new List<Terrain>());
                    var newTerrain = new Terrain { Coordinates = locationKey, Type = Terrain.TerrianType.Land };
                    islands[locationKey].Add(newTerrain);

                    var isProbing = true;
                    do
                    {
                        isProbing = Probe(islands[locationKey].Where(t => !t.IsInvestigated).ToList(), newTerrain.Coordinates, logicMesh, islands);
                    }
                    while (isProbing);
                    foreach (var island in islands[locationKey])
                    {
                        var yCoordinate = island.Coordinates[0];
                        var xCoordinate = island.Coordinates[1];
                        logicMesh[yCoordinate][xCoordinate] = false;
                    }
                    isFreeDrifting = true;
                    isEmbarked = true;
                }
            }
        }
    }
    while (isFreeDrifting);
    topDog = islands.Keys.Any() ? islands.Values.Max(i => i.Count) : 0;
    return topDog;
}

bool Probe(List<Terrain> terrains, int[] locationKey, bool[][] logicMesh, Dictionary<int[], List<Terrain>> islands)
{
    var isProbed = false;
    if (terrains is null || terrains.Count == 0)
    {
        return isProbed;
    }
    foreach (Terrain terrian in terrains)
    {
        terrian.Right ??= SeekTowardsRight(terrian.Coordinates, logicMesh);
        terrian.Left ??= SeekTowardsLeft(terrian.Coordinates, logicMesh);
        terrian.Bottom ??= SeekTowardsBottom(terrian.Coordinates, logicMesh);
        terrian.Top ??= SeekTowardsTop(terrian.Coordinates, logicMesh);
        isProbed |= MakeCartography(terrian, locationKey, islands);
    }
    return isProbed;
}

bool MakeCartography(Terrain oldTerrain, int[] locationKey, Dictionary<int[], List<Terrain>> islands)
{
    var isNewChart = false;
    Terrain? newTerrain = null;
    if (oldTerrain.Right == Terrain.TerrianType.Land &&
        !islands[locationKey].Any(t => t.Coordinates[0] == oldTerrain.Coordinates[0] && t.Coordinates[1] == oldTerrain.Coordinates[1] + 1))
    {
        newTerrain = new Terrain { Coordinates = new int[] { oldTerrain.Coordinates[0], oldTerrain.Coordinates[1] + 1 }, Type = Terrain.TerrianType.Land };
        islands[locationKey].Add(newTerrain);
        isNewChart = true;
    }
    if (oldTerrain.Left == Terrain.TerrianType.Land &&
        !islands[locationKey].Any(t => t.Coordinates[0] == oldTerrain.Coordinates[0] && t.Coordinates[1] == oldTerrain.Coordinates[1] - 1))
    {
        newTerrain = new Terrain { Coordinates = new int[] { oldTerrain.Coordinates[0], oldTerrain.Coordinates[1] - 1 }, Type = Terrain.TerrianType.Land };
        islands[locationKey].Add(newTerrain);
        isNewChart = true;
    }
    if (oldTerrain.Bottom == Terrain.TerrianType.Land &&
        !islands[locationKey].Any(t => t.Coordinates[0] == oldTerrain.Coordinates[0] + 1 && t.Coordinates[1] == oldTerrain.Coordinates[1]))
    {
        newTerrain = new Terrain { Coordinates = new int[] { oldTerrain.Coordinates[0] + 1, oldTerrain.Coordinates[1] }, Type = Terrain.TerrianType.Land };
        islands[locationKey].Add(newTerrain);
        isNewChart = true;
    }
    if (oldTerrain.Top == Terrain.TerrianType.Land &&
        !islands[locationKey].Any(t => t.Coordinates[0] == oldTerrain.Coordinates[0] - 1 && t.Coordinates[1] == oldTerrain.Coordinates[1]))
    {
        newTerrain = new Terrain { Coordinates = new int[] { oldTerrain.Coordinates[0] - 1, oldTerrain.Coordinates[1] }, Type = Terrain.TerrianType.Land };
        islands[locationKey].Add(newTerrain);
        isNewChart = true;
    }
    oldTerrain.IsInvestigated = true;
    return isNewChart;
}

Terrain.TerrianType SeekTowardsRight(int[] coordinates, bool[][] map)
{
    if (map[coordinates[0]].Length <= coordinates[1] + 1)
    {
        return Terrain.TerrianType.Unknown;
    }
    else
    {
        if (map[coordinates[0]][coordinates[1] + 1] == true)
        {
            return Terrain.TerrianType.Land;
        }
        else return Terrain.TerrianType.Sea;
    }
}

Terrain.TerrianType SeekTowardsLeft(int[] coordinates, bool[][] map)
{
    if (0 > coordinates[1] - 1)
    {
        return Terrain.TerrianType.Unknown;
    }
    else
    {
        if (map[coordinates[0]][coordinates[1] - 1] == true)
        {
            return Terrain.TerrianType.Land;
        }
        else return Terrain.TerrianType.Sea;
    }
}

Terrain.TerrianType SeekTowardsBottom(int[] coordinates, bool[][] map)
{
    if (map.Length <= coordinates[0] + 1)
    {
        return Terrain.TerrianType.Unknown;
    }
    else
    {
        if (map[coordinates[0] + 1][coordinates[1]] == true)
        {
            return Terrain.TerrianType.Land;
        }
        else return Terrain.TerrianType.Sea;
    }
}

Terrain.TerrianType SeekTowardsTop(int[] coordinates, bool[][] map)
{
    if (0 > coordinates[0] - 1)
    {
        return Terrain.TerrianType.Unknown;
    }
    else
    {
        if (map[coordinates[0] - 1][coordinates[1]] == true)
        {
            return Terrain.TerrianType.Land;
        }
        else return Terrain.TerrianType.Sea;
    }
}

static bool[][] CreateLogicMesh(string dataMesh)
{
    var rawMesh = new List<bool[]>();
    string? line;
    var reader = new StringReader(dataMesh);
    var width = -1;
    while ((line = reader.ReadLine()) != null)
    {
        if (width == -1)
        {
            width = line.Length;
        }
        var booleanLine = new bool[width];
        var elements = line.ToCharArray();
        for (int index = 0; index < elements.Length; index++)
        {
            booleanLine[index] =
                elements[index] == '1' ?
                    true :
                    (elements[index] == '0' ?
                        false :
                        throw new InvalidDataException($"Character '{elements[index]}' is invalid in data mesh"));
        }
        rawMesh.Add(booleanLine);
    }
    var logicMesh = rawMesh.ToArray();
    return logicMesh;
}

public class Terrain
{
    public enum TerrianType
    {
        Land,
        Sea,
        Unknown
    }

    public TerrianType? Top { get; set; }
    public TerrianType? Bottom { get; set; }
    public TerrianType? Left { get; set; }
    public TerrianType? Right { get; set; }
    public bool IsInvestigated { get; set; }
    public required int[] Coordinates { get; init; } = new int[2];
    public required TerrianType Type { get; set; }
}
#endif

#if ThirdTask
class Node
{
    public Node? next;
    public Node? previous;
    public object? data;

    public Node() { }
    public Node(object data)
    {
        this.data = data;
    }

    public override string? ToString()
    {
        return data is null ? base.ToString() : data.ToString();
    }
}

interface INaiveList
{
    void add(object element);
    bool insertAdd(object element, int index);
    object? elementAt(int index);
    bool isEmpty();
    void clear();
    string print();
}

class NaiveList : INaiveList
{
    public Node? head;

    public void add(object element)
    {
        if (head == null)
        {
            head = new Node(element);
            return;
        }

        var currentElement = head;
        Node? previousElement = null;
        while (currentElement != null)
        {
            previousElement = currentElement;
            currentElement = currentElement?.next;
        }
        currentElement = new Node(element);
        previousElement!.next = currentElement;
        currentElement.previous = previousElement;
    }

    public void clear()
    {
        if (head == null)
        {
            return;
        }

        var currentElement = head;
        Node? nextElement;
        do
        {
            nextElement = currentElement?.next;
            currentElement!.next = null;
            currentElement!.previous = null;
            currentElement.data = null;
            currentElement = nextElement;
        }
        while (nextElement != null);
        head.data = null;
        head = null;
    }

    public object? elementAt(int index) => NodeElementAt(index)?.data;

    public bool insertAdd(object element, int index)
    {
        var currentElement = NodeElementAt(index);
        if (currentElement == null)
        {
            return false;
        }
        var oldElement = currentElement;
        currentElement = new Node(element);

        var oldElementPrevious = oldElement.previous;
        if (oldElementPrevious != null)
        {
            oldElementPrevious.next = currentElement;
        }
        currentElement.next = oldElement;
        oldElement.previous = currentElement;

        return true;
    }

    public bool isEmpty() => head == null;

    public string print()
    {
        if (head == null)
        {
            return string.Empty;
        }

        var currentElement = head;
        do
        {
            Console.WriteLine(currentElement);
            currentElement = currentElement!.next;
        }
        while (currentElement != null);

        return string.Empty;
    }

    private Node? NodeElementAt(int index)
    {
        if (index == 0)
        {
            return head;
        }

        var counter = 0;
        var currentElement = head;
        while (currentElement != null && counter++ != index)
        {
            currentElement = currentElement!.next;
        }
        if (counter != index && currentElement == null)
        {
            return null;
        }

        return currentElement;
    }
}
#endif