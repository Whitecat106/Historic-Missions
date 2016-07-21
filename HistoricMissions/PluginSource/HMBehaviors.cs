/*
 * Whitecat Industries Historic Missions CC Extensions for Kerbal Space Program. 
 * 
 * Written by Whitecat106 (Marcus Hehir).
 * 
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 * 
 * This code is released freely into the public domain, exempt from the licence packaged with the Historic Missions Contract Pack. 
 * As such do as you wish with the following code. 
 * 
 * 
 * The following code has been built as an extension for Contract Configurator
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;

using Contracts;
using ContractConfigurator;
using ContractConfigurator.Parameters;
using ContractConfigurator.ExpressionParser;

namespace WhitecatIndustries
{
    public class HMBehaviorExtensions : ContractConfigurator.BehaviourFactory
    {
        public override bool Load(ConfigNode configNode)
        {
            bool valid = base.Load(configNode);

            configNode.AddValue("contractType", "");
            
            return valid;
        }

        public override ContractBehaviour Generate(ConfiguredContract contract)
        {
            return new CompleteContract();
        }
    }

    public class CompleteContract : ContractBehaviour
    {
        ConfigNode node = new ConfigNode();

        protected override void OnAccepted()
        {
            base.OnAccepted();

            double FundsInitial = (Funding.Instance.Funds);
            float RepInitial = Reputation.Instance.reputation;
            float SciInitial = ResearchAndDevelopment.Instance.Science;

            Contracts.Contract HMContract = new Contracts.Contract();

            foreach (Contracts.Contract contract in Contracts.ContractSystem.Instance.Contracts)
            {
                if (contract.Title == node.GetValue("contractType"))
                {
                    HMContract.Complete();
                }
            }

            double NewFunds = Funding.Instance.Funds;
            float NewRep = Reputation.Instance.reputation;
            float NewSci = ResearchAndDevelopment.Instance.Science;

            Funding.Instance.AddFunds(-(NewFunds - FundsInitial), TransactionReasons.Progression); // Reset funds etc. 
            Reputation.Instance.AddReputation(-(NewRep - RepInitial), TransactionReasons.Progression);
            ResearchAndDevelopment.Instance.AddScience(-(NewSci - SciInitial), TransactionReasons.Progression);
        }

        protected override void OnLoad(ConfigNode configNode)
        {
            base.OnLoad(configNode);
            node = configNode; 
        }

        protected override void OnSave(ConfigNode configNode) 
        {
            base.OnSave(configNode);
        }
    }
}
