using System;
using System.Linq;
using System.Text;

namespace REAPERJapanesePatcher
{
    public class RoutingDialogFixer : IBinaryFixer
    {
        static readonly byte[] RoutingDefaultWidth = BitConverter.GetBytes((Int16)205);
        static readonly byte[] RoutingDefaultHeight = BitConverter.GetBytes((Int16)57);
        static readonly byte[] RoutingNewHeight = BitConverter.GetBytes((Int16)80);
        static readonly byte[] RoutingPadding = BitConverter.GetBytes((Int16)0);

        static readonly byte[] ReaperDialogClassName = Encoding.Unicode.GetBytes("REAPERVirtWndDlgHost");
        static readonly SubsequenceFinder RoutingDialog;

        static RoutingDialogFixer()
        {
            RoutingDialog = new SubsequenceFinder(RoutingDefaultWidth
                                .Concat(RoutingDefaultHeight)
                                .Concat(RoutingPadding)
                                .Concat(ReaperDialogClassName)
                                .ToArray()
                            );
        }


        public void Fix(byte[] bytes)
        {
            var foundPos = RoutingDialog.FindIn(bytes, 0);
            if (!foundPos.HasValue) return;

            Buffer.BlockCopy(RoutingNewHeight.ToArray(), 0, bytes,
                foundPos.Value + RoutingDefaultWidth.Length, RoutingNewHeight.Length);
        }
    }
}
