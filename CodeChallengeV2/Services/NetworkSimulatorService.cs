using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class NetworkSimulatorService : INetworkSimulatorService
    {
        /// <summary>
        /// Returns all nodes of type Gateway that if removed together with their connected edges would leave nodes of type Device, without edges to any Gateway. 
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public Task<List<Node>> FindCritialGateways(NetworkGraph graph)
        {
            var devDict = new Dictionary<String, List<String>>();

            List<String> devices = graph.Graphs.FirstOrDefault().Nodes.Where(x => x.Type == "Device").Select(x => x.Id).ToList();

            devices.ForEach(dev => devDict[dev] = new List<string>());
            graph.Graphs.FirstOrDefault().Edges.Where(x => x.Relation == "is_connected_to").ToList().ForEach(y => devDict[y.Source].Add(y.Target));

            List<String> criticalGateways = devDict.Where( x => x.Value.Count == 1).Select( y => y.Value.First()).ToList(); 

            return Task.FromResult(graph.Graphs.FirstOrDefault().Nodes.Where(x => x.Type == "Gateway" && criticalGateways.Contains(x.Id)).ToList());
        }
    }
}
