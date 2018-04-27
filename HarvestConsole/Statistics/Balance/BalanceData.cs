using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Statistics.Balance
{
    class BalanceData
    {
        public Dictionary<string, double> Data { get; private set; }

        public BalanceData(Dictionary<string, double> data)
        {
            this.Data = data;
        }

        public double CostCropEffect(int plantCost, int harvestDewCost, int effectCost, double effectPower)
        {
            double value = plantCost;
            for (int dew = 1; dew <= harvestDewCost; dew++)
            {
                double effect = 0;
                if (effectCost < dew)
                    effect = effectPower;
                
                value = value * Data["cropdewcoeff"] + Data["cropdewvalue"] - effect;
            }

            return Data[$"cropbudget{plantCost}_{harvestDewCost}"] - value;
        }
    }
}
