using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class Specimen : ISpecimen<Specimen>
    {
        public Data Config { get; set; }

        public Specimen(Data config, ISpecimenInitializator<Specimen> specimenInitialization)
        {
            this.Nodes = new List<Node>();
            this.Config = config;
            this.Items = new Dictionary<Item, bool>();
            this.SpecimenInitialization = specimenInitialization;
            this.CurrentKnapsackUsage = 0;
        }

        private Dictionary<Item, bool> Items { get; }

        public List<Node> Nodes { get; }

        public ISpecimenInitializator<Specimen> SpecimenInitialization { get; }

        public double CurrentKnapsackUsage { get; set; }

        public double? EvaluationValue { get; set; }

        public double Evaluate()
        {
            if (this.EvaluationValue.HasValue)
            {
                return this.EvaluationValue.Value;
            }

            var profit = 0d;
            foreach(var item in Items)
            {
                if (item.Value)
                {
                    profit += item.Key.Profit;
                }
            }
            var time = 0d;
            var currentWeight = 0d;
            this.UpdateWeight(ref currentWeight, this.Nodes[0]);
            for(int i = 1; i < this.Nodes.Count; i++)
            {
                var distance = this.Config.GetNodeMatrix()[(this.Nodes[i - 1], this.Nodes[i])];
                var currentSpeed = this.Config.MaxSpeed - currentWeight * ((this.Config.MaxSpeed - this.Config.MinSpeed) / this.Config.KnapsackCapacity);
                time += distance * currentSpeed;
                this.UpdateWeight(ref currentWeight, this.Nodes[i]);
            }
            this.EvaluationValue = profit + time;
            return profit - time;
        }

        public bool AddItemToKnapsack(Item item)
        {
            if(this.Config.KnapsackCapacity >= this.CurrentKnapsackUsage + item.Weight)
            {
                this.Items[item] = true;
                this.CurrentKnapsackUsage += item.Weight;
                return true;
            }
            return false;
        }

        public Item[] GetKnapsackItems()
        {
            return this.Items.Where(i => i.Value).Select(i => i.Key).ToArray();
        }

        public void RemoveItemFromKnapsack(Item item)
        {
            this.Items[item] = false;
            this.CurrentKnapsackUsage -= item.Weight;
        }

        public bool CheckIfItemIsInKnapsack(Item item)
        {
            return this.Items.ContainsKey(item);
        }

        private void UpdateWeight(ref double weight, Node node)
        {
            foreach (var item in node.AvailableItems)
            {
                if (this.Items[item])
                {
                    weight += item.Weight;
                }
            }
        }

        private void SetupItemsDictionary()
        {
            if(this.Items.Count != 0)
            {
                return;
            }
            foreach(var item in this.Config.Items)
            {
                this.Items.Add(item, false);
            }
        }

        public void Fix()
        {
            var cities = new HashSet<Node>(this.Config.Nodes);
            foreach(var node in this.Nodes) {
                cities.Remove(node);
            }
            var citiesToRemove = this.Nodes.GroupBy(n => n).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            var index = 0;
            foreach(var city in cities)
            {
                var nodeIndex = this.Nodes.IndexOf(city);
                this.Nodes.RemoveAt(nodeIndex);
                this.Nodes.Insert(nodeIndex, citiesToRemove[index]);
                citiesToRemove.RemoveAt(index++);
            }
        }

        public void Init()
        {
            this.SetupItemsDictionary();
            this.SpecimenInitialization.Initialize(this);
        }

        public Specimen Clone()
        {
            var specimen = new Specimen(this.Config, this.SpecimenInitialization);
            specimen.SetupItemsDictionary();
            foreach(var node in this.Nodes)
            {
                specimen.Nodes.Add(node);
            }
            foreach(var item in this.Items)
            {
                specimen.Items[item.Key] = item.Value;
            }
            return specimen;
        }
    }
}
