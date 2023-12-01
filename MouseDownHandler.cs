using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    // from https://github.com/Shockah/Cobalt-Core-Mods/blob/master/_Shared/MouseDownHandler.cs
    internal sealed class MouseDownHandler : OnMouseDown
    {
        private readonly Action Delegate;

        public MouseDownHandler(Action @delegate)
        {
            this.Delegate = @delegate;
        }

        public void OnMouseDown(G g, Box b)
            => Delegate();
    }
}
