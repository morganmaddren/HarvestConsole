using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Statistics.Balance
{
    static class BalanceLibrary
    {
        public static BalanceData GetBalanceData(string version)
        {
            return balanceDicts[version]();
        }

        static Dictionary<string, Func<BalanceData>> balanceDicts = new Dictionary<string, Func<BalanceData>>()
        {
            ["V5.03"] = V53Balance,
            ["V5.02"] = V52Balance,
        };

        static BalanceData V53Balance()
        {
            var data = new Dictionary<string, double>();
            data["cropdewvalue"] = .25;
            data["cropdewcoeff"] = 1.1;
            data["handvalue"] = 1;
            data["handcoeff"] = 1.5;
            data["dewvalue"] = .625;
            data["dewcoeff"] = 1.25;
            data["avgcropcount"] = 5;
            data["avgwpcount"] = 3;
            data["avgcentercropofcolorcount"] = 2;
            data["avgcrophandcost"] = 3.5;
            data["chooseselfmodifier"] = .75;
            data["choosewpmodifier"] = .625;
            data["choosecropmodifier"] = .875;
            data["choosespellmodifier"] = .875;
            data["vpvalue"] = 1;
            data["vpcoeff"] = 1;
            data["plantwp"] = 1.5;
            data["revealwp"] = .125;
            data["burncrop"] = -1.125;
            data["spelldrawvalue"] = .5;
            data["spelldrawcoeff"] = 1.75;
            data["discard1spell"] = -.375;
            data["curseasoncolormodifier"] = .375;
            data["colorcropreq"] = .125;
            data["spellvalue"] = 1;
            data["spellcoeff"] = 1.03;

            for (int hand = 1; hand < 12; hand++)
            {
                for (int dew = 1; dew < 8; dew++)
                {
                    double initial = hand;
                    if (dew > 1)
                        initial = data[$"cropbudget{hand}_{dew - 1}"];

                    data[$"cropbudget{hand}_{dew}"] = initial * data["cropdewcoeff"] + data["cropdewvalue"];
                    data[$"cropnet{hand}_{dew}"] = data[$"cropbudget{hand}_{dew}"] - hand;
                    data[$"cropavgdewvalue{hand}_{dew}"] = data[$"cropnet{hand}_{dew}"] / dew;
                }
            }

            for (int i = 0; i < 7; i++)
                data[$"spellbudget{i}"] = Math.Pow(i, data["spellcoeff"]) * data["spellvalue"] + data["colorcropreq"];

            for (int i = 1; i < 5; i++)
                data[$"gain{i}hand"] = Math.Pow(i, data["handcoeff"]) * data["handvalue"];

            for (int i = 1; i < 7; i++)
            {
                data[$"gain{i}dew"] = Math.Pow(i, data["dewcoeff"]) * data["dewvalue"];
                data[$"gain{i}dewself"] = data[$"gain{i}dew"] * data["chooseselfmodifier"];
            }

            for (int i = 1; i < 11; i++)
                data[$"gain{i}vp"] = Math.Pow(i, data["vpcoeff"]) * data["vpvalue"];

            for (int i = 1; i < 4; i++)
                data[$"gain{i}spell"] = Math.Pow(i, data["spelldrawcoeff"]) * data["spelldrawvalue"];

            data["gain1dewall"] = data["gain1dew"] * data["avgcropcount"];


            data["gain1dewwp"] = data[$"cropavgdewvalue2_2"] * data["choosewpmodifier"];
            data["gain1dewwpall"] = data["gain1dewwp"] * data["avgcropcount"];
            data["harvestwp"] = data[$"cropnet2_2"] * data["choosewpmodifier"];
            data["revealallwp"] = data["revealwp"] * data["avgwpcount"];
            data["optionalburnwp"] = -data[$"cropnet2_2"] * data["choosewpmodifier"];

            return new BalanceData(data);
        }

        static BalanceData V52Balance()
        {
            var data = new Dictionary<string, double>();
            data["cropdewvalue"] = .125;
            data["cropdewcoeff"] = 1.125;
            data["handvalue"] = 1;
            data["handcoeff"] = 1.5;
            data["dewvalue"] = .625;
            data["dewcoeff"] = 1.25;
            data["avgcropcount"] = 5;
            data["avgwpcount"] = 3;
            data["chooseselfmodifier"] = .75;
            data["choosewpmodifier"] = .875;
            data["vpvalue"] = 1;
            data["vpcoeff"] = 1;
            data["plantwp"] = 1.5;
            data["revealwp"] = .125;
            data["burncrop"] = -1.125;
            data["spelldrawvalue"] = .5;
            data["spelldrawcoeff"] = 1.75;
            data["discard1spell"] = -.375;
            data["colorcropreq"] = .125;
            data["spellvalue"] = 1;
            data["spellcoeff"] = 1.03;

            for (int hand = 1; hand < 12; hand++)
            {
                for (int dew = 1; dew < 8; dew++)
                {
                    double initial = hand;
                    if (dew > 1)
                        initial = data[$"cropbudget{hand}_{dew - 1}"];

                    data[$"cropbudget{hand}_{dew}"] = initial * data["cropdewcoeff"] + data["cropdewvalue"];
                    data[$"cropnet{hand}_{dew}"] = data[$"cropbudget{hand}_{dew}"] - hand;
                    data[$"cropavgdewvalue{hand}_{dew}"] = data[$"cropnet{hand}_{dew}"] / dew;
                }
            }

            for (int i = 0; i < 7; i++)
                data[$"spellbudget{i}"] = Math.Pow(i, data["spellcoeff"]) * data["spellvalue"] + data["colorcropreq"];

            for (int i = 1; i < 5; i++)
                data[$"gain{i}hand"] = Math.Pow(i, data["handcoeff"]) * data["handvalue"];

            for (int i = 1; i < 7; i++)
            {
                data[$"gain{i}dew"] = Math.Pow(i, data["dewcoeff"]) * data["dewvalue"];
                data[$"gain{i}dewself"] = data[$"gain{i}dew"] * data["chooseselfmodifier"];
            }

            for (int i = 1; i < 11; i++)
                data[$"gain{i}vp"] = Math.Pow(i, data["vpcoeff"]) * data["vpvalue"];

            for (int i = 1; i < 4; i++)
                data[$"gain{i}spell"] = Math.Pow(i, data["spelldrawcoeff"]) * data["spelldrawvalue"];

            data["gain1dewall"] = data["gain1dew"] * data["avgcropcount"];


            data["gain1dewwp"] = data[$"cropavgdewvalue2_2"] * data["choosewpmodifier"];
            data["gain1dewwpall"] = data["gain1dewwp"] * data["avgcropcount"];
            data["harvestwp"] = data[$"cropnet2_2"] * data["choosewpmodifier"];
            data["revealallwp"] = data["revealwp"] * data["avgwpcount"];
            data["optionalburnwp"] = -data[$"cropnet2_2"] * data["choosewpmodifier"];

            return new BalanceData(data);
        }
    }
}
