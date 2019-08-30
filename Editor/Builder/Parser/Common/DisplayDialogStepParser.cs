using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pringles.Parser
{
    /// <summary>
    /// ディスプレイ表示するやつ
    /// </summary>
    public sealed class DisplayDialogStepParser : BuildStepParser<DisplayDialogStep>
    {
        public override string Cmd => "dialog";

        protected override DisplayDialogStep DoParse()
        {
            var title = GetArg("-title");
            var msg = GetArg("-msg");
            return new DisplayDialogStep(title, msg);
        }
    }
}
