using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pringles.Parser
{
    /// <summary>
    /// SwitchPlatform表示するやつ
    /// </summary>
    public sealed class SwitchPlatformStepParser : BuildStepParser<PlatformSwitchStep>
    {
        public override string Cmd => "switch";

        protected override PlatformSwitchStep DoParse()
        {
            var platformName = GetArg(ArgsKey.Platform);
            var g = new
            {
                Group = ParserUtils.ToGroup( platformName ),
                Target = ParserUtils.ToTarget( platformName ),
            };
            return new PlatformSwitchStep( g.Group, g.Target );
        }
    }
}
