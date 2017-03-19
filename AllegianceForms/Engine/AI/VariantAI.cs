using System.Drawing;

namespace AllegianceForms.Engine.AI
{
    public class VariantAI : BaseAI
    {
        public EVariantAiStyle Style { get; set; }
        public EVariantAiTechFocus Tech { get; set; }
        public EVariantAiResearchFocus Research { get; set; }
        public int FinalTechGroups { get; set; }
        public int CapsBeforeInvading { get; set; }

        public VariantAI(int team, Color teamColour
                , EVariantAiStyle style = EVariantAiStyle.Random, EVariantAiTechFocus tech = EVariantAiTechFocus.Random
                , EVariantAiResearchFocus research = EVariantAiResearchFocus.Random
                , int finalTechGroups = -1, int capsBeforeInvade = -1) : base(team, teamColour)
        {
            Style = (style == EVariantAiStyle.Random ? (EVariantAiStyle)StrategyGame.Random.Next(0,3) : style);
            Tech = (tech == EVariantAiTechFocus.Random ? (EVariantAiTechFocus)StrategyGame.Random.Next(0, 4) : tech);
            Research = (research == EVariantAiResearchFocus.Random ? (EVariantAiResearchFocus)StrategyGame.Random.Next(0, 7) : research);

            FinalTechGroups = (finalTechGroups <= 0 ? StrategyGame.Random.Next(1, 4) : finalTechGroups);
            CapsBeforeInvading = (capsBeforeInvade <= 0 ? StrategyGame.Random.Next(3, 8) : capsBeforeInvade);
        }

        public override void Update()
        {
            if (!Enabled) return;
            _nextActionAllowed--;
            if (_nextActionAllowed > 0) return;

            // ** STYLE
            // Aggressive: standard scout (sectors/3)+1, push cons into enemy sectors, flood enemy sectors with miner and base offense
            // Expansive: more scout (sectors/2)+1, push cons next to enemy sectors, 50% pilots on constant miner and base offense, 25% on miner defense, rest in reserve
            // Defensive: less scount (sectors/4)+1, push conds next to our sectors, 25% pilots on constant miner and base offense, 50% on miner defense, rest in reserve

            // ** MINERS
            // All: try to get friendly sectors with resources * 2) if a miner is docked mine in a random friendly sector with resources.

            // ** TECH
            // Caps: SY & outpost cons. Build full caps & bombers & SY upgrades. Then Starbase scouts & upgrades. Then other tech for upgrades only
            // 1/2/3 techs - start with home tech. Follow research focus (even/single starting with first). Then SY & outpost cons.

            // ** END GAME
            // Caps: When [CapsBeforeInvading] split into [FinalTechGroups] groups, send all pilots to support caps. Constantly build caps & send.
            // Techs: When end game reached, split into [FinalTechGroups] groups, send all pilots to support groups. Build caps & switch when [CapsBeforeInvading] reached.



        }
    }
}
