public class HillClimbing
{
    public List<int> solution;

    public double fitness;

    public List<Double> brickWeights;

    public HillClimbing(List<double> weights)
    {
        Random random = new Random();

        brickWeights = weights;

        solution = new List<int>();

        for(int i = 0; i < brickWeights.Count; i++)
        {
            solution.Add(random.Next(0, 3));
        }

        fitness = CalculateCurrentFitness();
    }


    public static List<double> ReadWeights(string filePath)
    {
        List<double> weights = new List<double>();

        foreach (var line in File.ReadAllLines(filePath))
        {
            weights.Add(double.Parse(line.Trim()));
        }


        return weights;
    }

    // Ensure best solution is maintained instead of it changing when making a small chanage to current solution
    public HillClimbing DeepCopy()
    {
        HillClimbing copy = new HillClimbing(this.brickWeights);
        copy.solution = new List<int>(this.solution);
        copy.fitness = this.fitness;
        return copy;
    }

    public double CalculateCurrentFitness()

    {
        //Total weight for all lorries combined
        double[] lorryWeights = new double[3];

        for (int i = 0; i < solution.Count; i++)
        {
            int lorryIndex = solution[i];
            lorryWeights[lorryIndex] += brickWeights[i];
        }

        double averageWeight = lorryWeights.Average();
        double sumOfSquaredDifferences = lorryWeights.Sum(weight => Math.Pow(weight - averageWeight, 2));
        double Fitness = Math.Sqrt(sumOfSquaredDifferences / lorryWeights.Length);

        return Fitness;
    }

    public void SmallChange()
    {
        Random random = new Random();
        int index = random.Next(solution.Count);
        solution[index] = random.Next(0, 3); 
        fitness = CalculateCurrentFitness();
    }

}

public class Experiment
{
    public static void RunExperiment()
    {
        string FilePath = "/Users/ahmedalaguori/Projects/Random Restart ( Hill climbing )/Random Restart ( Hill climbing )/Dataset1.csv";
        List<double> Weights = HillClimbing.ReadWeights(FilePath);

        int numrestarts = 1000;

        int iterationsperRestart = 1000;

        HillClimbing BestSolution = null;

        double bestFitness = double.MaxValue; 

        for(int restart = 0; restart < numrestarts; restart++)
        {
            HillClimbing currentSolution = new HillClimbing(Weights);
            for(int i = 0; i < iterationsperRestart; i++)
            {
                currentSolution.SmallChange();

                if(currentSolution.fitness <bestFitness)
                {
                    BestSolution = currentSolution.DeepCopy();

                    bestFitness = currentSolution.fitness;
                }

            }

        }

        Double RoundedBestFitnessScore = Math.Round(bestFitness, 2);
        Console.WriteLine("Fitness Score : "+ RoundedBestFitnessScore);
        string solutionString = string.Join(", ", BestSolution.solution);
        Console.WriteLine("Solution : " + solutionString);

    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Experiment.RunExperiment();
        
    }
}
