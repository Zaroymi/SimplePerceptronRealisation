using NetRealization;
using NetRealization.Functions;
using NetRealization.Layer;
using NetRealization.Neurons;
using NetRealization.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TryToNet
{
    class Program
    {
        delegate void CallBack(Action func, Action printRes, string name);

        static void Main(string[] args)
        {
            var firstNeuralNetwork = new NeuralNetwork(9, 25, 9);
            var secondNeuralNetwork = new NeuralNetwork(9, 25, 9);

            Training(firstNeuralNetwork, secondNeuralNetwork);

            bool game = true;
            List<int> gameList = new List<int>(9) { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int i = 0;
            while(game)
            {
                i++;
                Console.Clear();
                DrawField(gameList, i);
                ConvertField(gameList);
                NewTurn((i % 2) != 0 ? firstNeuralNetwork : secondNeuralNetwork, gameList);
                Console.Clear();
                DrawField(gameList, i);
                Thread.Sleep(5000);
                if(i == 9)
                {
                    game = false;
                    Console.WriteLine("Game ends");
                }
            }

            Console.ReadLine();
           
        }

        private static void ConvertField(List<int> gameList)
        {
            for(int i = 0; i < gameList.Count; i++)
            {
                gameList[i] *= -1;
            }
        }

        private static void NewTurn(NeuralNetwork firstNeuralNetwork, List<int> gameList)
        {
            firstNeuralNetwork.Process(gameList.ConvertAll((x) => { return Convert.ToDouble(x); }));
            int answer = RightAnswer(firstNeuralNetwork.OutputLayer.Neurons);
            while (answer < 9 &&gameList[answer] != 0)
            {
                answer = RightAnswer(firstNeuralNetwork.OutputLayer.Neurons, answer);
            }
            gameList[answer] = 1;
        }

        private static int RightAnswer(List<INeuron> neu, int? maximumId = null)
        {
            int result = -1;
            if (maximumId != null)
            {
                List<INeuron> newNeu = new List<INeuron>(neu);
                for (int i = 0; i < newNeu.Count; i++)
                {
                    for (int j = i; j < newNeu.Count; j++)
                    {
                        if (newNeu[i].Result < newNeu[j].Result)
                        {
                            INeuron temp = newNeu[i];
                            newNeu[i] = newNeu[j];
                            newNeu[j] = temp;
                        }
                    }
                }
                INeuron maxRes = neu[maximumId.Value];
                int id = newNeu.FindIndex((neur) => neur == maxRes);
                result = neu.FindIndex((neur) => neur == newNeu[id == neu.Count ? id : id + 1]);
            }
            else
            {
                double max = neu.Max(neur => neur.Result);
                result = neu.FindIndex(neur => neur.Result == max);
            }
            return result;
        }

        private static void DrawField(List<int> game, int turn)
        {
            List<int> tempGame = new List<int>(game);
            if (turn % 2 == 0)
                ConvertField(tempGame);
            for(int i = 0; i < 9; i += 3)
            {
                string output = "";
                for (int j = i; j < i + 3; j++)
                {
                    output += tempGame[j] == 1 ? "[X] " : tempGame[j] == 0 ? "[ ] " : "[0] "; 
                }
                Console.WriteLine(output);
            }
        }

        private static void Training(NeuralNetwork firstNeuralNetwork, NeuralNetwork secondNeuralNetwork)
        {
            CallBack callBackMethod = delegate (Action func, Action printRes, string name)
            {
                func();
                printRes();
                Console.WriteLine($"{name} train is ended ");
            };

            Action firstTrain = () =>
            {
                callBackMethod(
                    () => { firstNeuralNetwork.Train(T1(), 500, 1, 0.3); },
                    () => { PrintResNeurons(firstNeuralNetwork.OutputLayer.Neurons); },
                    "A");
            };

            Action secondTrain = () =>
            {
                callBackMethod(
                    () => { secondNeuralNetwork.Train(T2(), 500, 1, 0.3); },
                    () => { PrintResNeurons(secondNeuralNetwork.OutputLayer.Neurons); },
                    "B");
            };

            Thread firstThread = new Thread(() => { firstTrain(); });
            Thread secondThread = new Thread(() => { secondTrain(); });
            firstThread.Start();
            secondThread.Start();
            firstThread.Join();
            secondThread.Join();
        }

        private static void PrintResNeurons(List<INeuron> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"Neu[{i + 1}] Result[{list[i].Result}]");
            }
        }
        private static void A_NeuronEndProcess(object sender, NetRealization.Neurons.NeuronEventArgs e)
        {
            if (sender is OutputLayer)
            {
                string toOut = "";
                if (e.InputWeights != null)
                {
                    toOut += $"Результат : {e.Result.ToString("N")}";
                    if (e.Error != null)
                    {
                        toOut += $" Ошибка : {(e.Error.Value * 100).ToString("N")}%";
                    }
                }
                Console.WriteLine(toOut);
            }
        }

        private static void LayerEndProcess(object sender, NetRealization.Layer.LayerProcessEndEventArgs e)
        {
            Console.WriteLine(e.LayerProcessStatus);

        }

        public static List<TrainSet> T1()
        {
            return new List<TrainSet>()
            {
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 1, 0, 0, 0, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 1, 0, 0, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 1, 0, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 1, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 1, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 1, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 1, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 0, 1, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 1 }) ,

                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 1, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, -1, 0, 1, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 1, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, -1, 0, 1, 0, 1 ,0 ,-1 }, new List<double>() { 0, 0, 0, 0, 0, 1, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {-1, 0, -1, 0, 1, 0, 1 ,0 ,-1 }, new List<double>() { 0, 0, 0, 1, 0, 0, 0, 0, 0 }) ,
            };
        }
        public static List<TrainSet> T2()
        {
            return new List<TrainSet>()
            {
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 1, 0, 0, 0, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 1, 0, 0, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 1, 0, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 1, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 1, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 1, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 1, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 0, 1, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 1 }) ,

                new TrainSet(new List<double>() {0, 0, 0, 0, 0, 0, 0 ,0 ,0 }, new List<double>() { 1, 0, 0, 0, 0, 0, 0, 0, 0 }) ,
                new TrainSet(new List<double>() {0, 0, 1, 0, -1, 0, 0 ,0 ,0 }, new List<double>() { 0, 0, 0, 0, 0, 0, 1, 0, 0 }) ,
                new TrainSet(new List<double>() {-1, 0, 1, 0, -1, 0, 1, 0, 0}, new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 1 }) ,
                new TrainSet(new List<double>() {-1, 0, 1, 0, -1, -1, 1 ,0 ,1 }, new List<double>() { 0, 0, 0, 0, 0, 0, 0, 1, 0 }) ,
            };
        }
    }
}
